using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using LingoLib;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace OpenLingoClient.Control.Net
{
    /**
     * Provide a means of finding other players and constructing a game 
     */
    class LobbyNet
    {
        public class Client
        {
            #region fields
            public static BinaryFormatter formatter = new BinaryFormatter();
            public static TcpClient client;
            public static SslStream stream;
            public static IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            public static int port = 6699;

            public static bool IsConnected;
            #endregion


            public static bool Init()
            {
                bool connected = true;
                try
                {
                    client = new TcpClient(localAddr.ToString(), port);
                    stream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(CertificateValidationCallback));
                    stream.AuthenticateAsClient("OpenLingo");
                }
                catch
                {
                    connected = false;
                }
                IsConnected = true;
                return connected;
            }

            public static void Ping()
            {
                int queueNumber = PackageManager.getInstance().add(new Package(LingoProtocol.PING, "Pong"));
                Package receivedPackage = PackageManager.getInstance().request(queueNumber);
                System.Console.WriteLine("Reply: " + (string) receivedPackage.transmittedObject);
            }

            public static List<PlayerInfo> ConnectToLobby() //Also returns the current users 
            {
                List<PlayerInfo> retVal = new List<PlayerInfo>();
                int queueNumber = PackageManager.getInstance().add(new Package(LingoProtocol.REGISTER_LOBBY, Config.LocalPlayer));
                Package receivedPackage = PackageManager.getInstance().request(queueNumber);
                if (receivedPackage.CommandName != LingoProtocol.OK)
                    return retVal;
                System.Console.WriteLine("Registration Success");
                queueNumber = PackageManager.getInstance().add(new Package(LingoProtocol.REQUEST_LOBBY, Config.LocalPlayer));
                receivedPackage = PackageManager.getInstance().request(queueNumber);
                retVal = receivedPackage.transmittedObject as List<PlayerInfo>;
                return retVal;
            }

            public static bool DisconnectFromLobby()
            {
                int queueNumber = PackageManager.getInstance().add(new Package(LingoProtocol.UNREGISTER_LOBBY, Config.LocalPlayer));
                Package receivedPackage = PackageManager.getInstance().request(queueNumber);
                if (receivedPackage.CommandName != LingoProtocol.OK)
                  return false;
                Console.WriteLine("Unregistration success");
                return true;
            }

            public static bool CertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                    return true;
                return false;
            }

            public static void Disconnect()
            {
                PackageManager.getInstance().add(new Package(LingoProtocol.CONNECTION_END, null));
                stream.Flush();
                stream.Close();
                client.Close();
            }
        }

        public class PackageManager
        {
            #region fields
            private static PackageManager INSTANCE = new PackageManager();

            private List<Package> receivingPackageList = new List<Package>();
            private int currentQueueNumber = 0;

            private object currentQueueNumberLock = new object();

            public bool allowTraffic = true;
            #endregion

            private PackageManager()
            {
                #region initializeReceivingThread
                ThreadPool.QueueUserWorkItem(delegate
                {
                    while (true)
                    {
                        if (allowTraffic)
                        {
                            try
                            {
                                Package package = null;

                                package = (Package)Client.formatter.Deserialize(Client.stream);

                                lock (receivingPackageList)
                                {
                                    receivingPackageList.Add(package);
                                }
                            }
                            catch { }
                        }
                        Thread.Sleep(10);
                    }
                });
                #endregion
            }

            public static PackageManager getInstance()
            {
                return INSTANCE;
            }

            public int add(Package package)
            {
                try
                {
                    lock (currentQueueNumberLock)
                    {
                        package.queueNumber = getUniqueQueueNumber();
                    }
                    lock (Client.stream)
                        Client.formatter.Serialize(Client.stream, package);
                }
                catch
                { }

                return package.queueNumber;
            }

            public Package request(int queueNumber)
            {
                int attempts = 0;
                while (true)
                {
                    lock (receivingPackageList)
                    {
                        try
                        {
                            foreach (Package package in receivingPackageList)
                            {
                                if (package.queueNumber == queueNumber)
                                {
                                    receivingPackageList.Remove(package);
                                    return package;
                                }
                            }
                        }
                        catch
                        { }
                    }
                    Thread.Sleep(10);
                    if (attempts++ > 100)
                        return null;
                }
            }

            private int getUniqueQueueNumber()
            {
                if (currentQueueNumber < int.MaxValue)
                {
                    return currentQueueNumber++;
                }
                else
                {
                    return currentQueueNumber = 0;
                }
            }
        }

        public class PackageReceivedEventArgs : EventArgs
        {
            public Package ReceivedPackage;

            public PackageReceivedEventArgs(Package ReceivedPackage)
            {
                this.ReceivedPackage = ReceivedPackage;
            }
        }
    }

    class HttpHandler
    {
        private static readonly string wordsEnListAddress = "http://zaku2.com/eemonImg/wordsEn.txt"; //Static for now
        private static readonly string wordsNlListAddress = "http://zaku2.com/eemonImg/wordsNl.txt"; //Should use ISO codes
        private static string wordsListAddress
        {
            get
            {
                switch (Config.Language)
                {
                    case LANGUAGE.DUTCH:
                        return wordsNlListAddress;
                    case LANGUAGE.ENGLISH:
                        return wordsEnListAddress;
                    default:
                        return wordsEnListAddress;
                }
            }
            set { wordsListAddress = value; }
        }

        /**
         * True if new version available 
         */
        public static bool WordsListVersionCheck()
        {
            bool retval = false;
            int versionNo;
            if (Config.Language == LingoLib.LANGUAGE.DUTCH) //Only works for Guus's version system
            {
                try
                {
                    using (var client = new WebClient())
                    using (StreamReader downloadStream = new StreamReader(client.OpenRead(wordsListAddress)))
                    {
                        string version = downloadStream.ReadLine();
                        if (version.StartsWith("v="))
                        {
                            if (int.TryParse(version.Substring(2, 1), out versionNo))
                            {
                                if (Config.WordsListVersion < versionNo)
                                {
                                    retval = true;
                                    Config.WordsListVersion = versionNo;
                                    FileManager.WordListDispose();
                                }
                            }
                        }
                    }
                }
                catch (WebException e)
                {
                    if(e.HResult == 404)
                        Console.WriteLine("No wordslist available");
                }
            }
            return retval;
        }

        /**
         * Would not have guessed it was THIS simple.
         * Wireshark captures of this method are stored in project's picture folder
         */
        public static void WordsListAcquire()
        {
            try
            {

                int count = 0;
                string word;
                using (var client = new WebClient())
                using (StreamReader downloadStream = new StreamReader(client.OpenRead(wordsListAddress)))
                {
                    System.Console.WriteLine("Building wordslist...");
                    while ((word = downloadStream.ReadLine()) != null)
                    {
                        if (!word.StartsWith("v="))
                        {
                            FileManager.WordListBuild(word);
                            count++;
                            if ((count % 500 == 0))
                            {

                                int currentLineCursor = Console.CursorTop;
                                Console.SetCursorPosition(0, Console.CursorTop);
                                System.Console.WriteLine("Read approx. " + count + " words...");
                                Console.SetCursorPosition(0, currentLineCursor);
                            }
                        }
                    }

                }
                System.Console.WriteLine("Wordslist expanded with " + count + " words.");
            }
            catch (WebException)
            {
                System.Console.WriteLine("Webrequest failed. Download or create your own wordslist");
            }
        }
    }
}
