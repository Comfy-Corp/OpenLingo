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
    public class ServerNet
    {
        public class Client
        {
            #region fields
            public static IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            public static int port = 6699;
         
            private static PackageManager packageManager = new PackageManager(localAddr, port);

            public static bool IsConnected = false;
            #endregion

            public static void RetryConnect()
            {
                packageManager.Dispose();
                packageManager = new PackageManager(localAddr, port);
            }

            public static void Ping()
            {
                int queueNumber = packageManager.add(new Package(LingoProtocol.PING, "Pong"));
                Package receivedPackage = packageManager.request(queueNumber);
                System.Console.WriteLine("Reply: " + (string) receivedPackage.transmittedObject);
            }

            public static bool RegisterToLobby() //Also returns the current users 
            {
                int queueNumber = packageManager.add(new Package(LingoProtocol.REGISTER_LOBBY, Config.LocalPlayer));
                Package receivedPackage = packageManager.request(queueNumber);
                if (receivedPackage == null || receivedPackage.CommandName != LingoProtocol.OK)
                    return false; 
                System.Console.WriteLine("Registration Success");
                return true;
            }

            public static bool TryGetLobbyPlayers(out List<PlayerInfo> players)
            {
                List<PlayerInfo> retVal = new List<PlayerInfo>();
                int queueNumber = packageManager.add(new Package(LingoProtocol.GET_LOBBY_PLAYERS, Config.LocalPlayer));
                Package receivedPackage = packageManager.request(queueNumber);
                if(receivedPackage == null)
                {
                    players = null;
                    return false;
                }
                retVal = receivedPackage.transmittedObject as List<PlayerInfo>;
                players = retVal;
                return true;
            }

            public static bool DisconnectFromLobby()
            {
                int queueNumber = packageManager.add(new Package(LingoProtocol.UNREGISTER_LOBBY, Config.LocalPlayer));
                Package receivedPackage = packageManager.request(queueNumber);
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

        }
 
        private class PackageManager : IDisposable
        {
            
            #region fields
            private List<Package> receivingPackageList = new List<Package>();
            private int currentQueueNumber = 0;
            public bool isInitialised = false;

            private object currentQueueNumberLock = new object();

            public bool allowTraffic = true;            
            public event EventHandler DisconnectEvent;

            public static BinaryFormatter formatter = new BinaryFormatter();
            public static TcpClient client;
            public static SslStream stream;
            #endregion

            /**
             * A package manager instance attempts to connect to a given ip and port
             * and can send requests to a LingoServer.
             * Traffic is always reactive, which isn't really the best option here.
             * This is more threadsafe though, which is easy to display
             */
            public PackageManager(IPAddress ip, int port)
            {
                Client.IsConnected = Init(ip, port);
                if (!Client.IsConnected)
                    return;
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

                                package = (Package) formatter.Deserialize(stream);

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

            private static bool Init(IPAddress ip, int port)
            {
                try
                {
                    client = new TcpClient(ip.ToString(), port);
                    stream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(Client.CertificateValidationCallback));
                    stream.AuthenticateAsClient("OpenLingo");
                    Client.IsConnected = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Package manager init failed: "+e.Message);
                    Console.WriteLine("Destination "+ip.ToString()+":"+port);
                    return false; //Regard any exceptions as server unreachable.
                }
                return true;
            }


            public int add(Package package)
            {
                if (!isInitialised)
                    return -1;
                try
                {
                    lock (currentQueueNumberLock)
                    {
                        package.queueNumber = getUniqueQueueNumber();
                    }
                    lock (stream)
                        formatter.Serialize(stream, package);
                }
                catch
                { }

                return package.queueNumber;
            }

            public Package request(int queueNumber)
            {
                if (!isInitialised)
                    return null;
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
                    {
                        DisconnectEvent(this,null);
                        return null;
                    }
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

            public void Dispose()
            {
                add(new Package(LingoProtocol.CONNECTION_END, null));
                try
                {
                    if (stream != null)
                    {
                        stream.Flush();
                        stream.Close();
                    }
                    if(client != null)
                        client.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Unable to properly close streams");
                    Console.WriteLine("Reason: "+e.Message);
                }
            }
        }

        /**public class PackageReceivedEventArgs : EventArgs
        {
            public Package ReceivedPackage;

            public PackageReceivedEventArgs(Package ReceivedPackage)
            {
                this.ReceivedPackage = ReceivedPackage;
            }
        }*/
    }
}
