using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LingoLib;

namespace OpenLingoServer
{
    class Server
    {
        private static Server _INSTANCE;
        public static Server INSTANCE 
        { get { if(_INSTANCE==null) _INSTANCE=new Server(); return _INSTANCE; }} 

        //Network properties
        private List<TcpClient> clients = new List<TcpClient>();
        private TcpListener server;

        private Server()
        {
            server = new TcpListener(IPAddress.Any, 6699);
            server.Start();
            Console.WriteLine("Server started");
            LobbyListen();
        }

        public void LobbyListen(){
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                new ClientHandler(client);
            }
        }

        public static X509Certificate getServerCert()
        {
            try
            {
                X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                X509CertificateCollection cert = store.Certificates.Find(X509FindType.FindBySubjectName, "OpenLingo", true);
                return cert[0];
            }
            catch (ArgumentOutOfRangeException)
            {
                System.Console.WriteLine("OpenLingo Root certificate not found");
            }
            return null;
        }

        public static bool ValidateClientCertificate(X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return false;
            return true;
        }
    }

    public class ClientHandler
    {
        private BinaryFormatter formatter = new BinaryFormatter();
        private System.Net.Sockets.TcpClient client;

        public ClientHandler(TcpClient client)
        {
            this.client = client;
            handle();
        }

        private void handle()
        {
            Console.WriteLine("Client connected");
            ThreadPool.QueueUserWorkItem(delegate
            {
                SslStream stream = new SslStream(client.GetStream());
                X509Certificate cert = Server.getServerCert();
                if(cert == null){
                    System.Console.WriteLine("Please install the OpenLingo certificate");
                    return;
                }
                stream.AuthenticateAsServer(cert, false, SslProtocols.Default, false);

                try
                {
                    while (true)
                    {
                        Package received = formatter.Deserialize(stream) as Package;
                        Package reply = ServerCommands.INSTANCE.FindCommand(received.CommandName)(received);
                        reply.queueNumber = received.queueNumber;
                        formatter.Serialize(stream, reply);                 
                    }
                }
                catch(Exception e) {
                    System.Diagnostics.Debug.WriteLine("Trace: " + e.Data);
                }
            });
        }
    }
}
