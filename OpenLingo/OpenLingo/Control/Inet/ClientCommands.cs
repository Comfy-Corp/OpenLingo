using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using LingoLib;

namespace OpenLingoServer
{
    public class ClientCommands
    {
        private Dictionary<String, Func<Object, Package>> Commands
            = new Dictionary<string, Func<Object, Package>>(); //I miss using var

        private static ClientCommands _INSTANCE;
        public static ClientCommands INSTANCE
        { get { if (_INSTANCE == null) _INSTANCE = new ClientCommands(); return _INSTANCE; } }

        public ClientCommands()
        {
            GenerateCommands();
        }

        public Func<Object, Package> FindCommand(string CommandName)
        {
            return Commands[CommandName];
        }

        private void GenerateCommands()
        {
            Commands["Ping"] = (parameters) =>
            {
                Package retVal = new Package(LingoProtocol.PING, "Pong");               
                System.Diagnostics.Debug.WriteLine("Received " + (string)parameters);
                return retVal;
            };
        }
    }
}
