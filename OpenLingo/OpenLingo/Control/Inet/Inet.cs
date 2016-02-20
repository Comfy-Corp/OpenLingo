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
    public class ServerNet
    {
        public class Client
        {
            #region fields
            public static BinaryFormatter formatter = new BinaryFormatter();
            public static TcpClient client;
            public static SslStream stream;
            public static IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            public static int port = 6699;

            public static bool IsConnected = false;
            #endregion


            public static bool Init()
            {
                if (!IsConnected)
                {
                    try
                    {
                        client = new TcpClient(localAddr.ToString(), port);
                        stream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(CertificateValidationCallback));
                        stream.AuthenticateAsClient("OpenLingo");
                        IsConnected = true;
                    }
                    catch(Exception)
                    {
                        IsConnected = false;
                    }
                }
                return IsConnected;
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
                queueNumber = PackageManager.getInstance().add(new Package(LingoProtocol.GET_LOBBY_PLAYERS, Config.LocalPlayer));
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
}
