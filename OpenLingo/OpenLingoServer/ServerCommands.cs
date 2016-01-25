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
    public class ServerCommands
    {
        private Dictionary<LingoProtocol, Func<Package, Package>> Commands //Function parameters: Received package->Reply Package
            = new Dictionary<LingoProtocol, Func<Package, Package>>(); //I miss using var

        private static ServerCommands _INSTANCE;
        public static ServerCommands INSTANCE
        { get { if (_INSTANCE == null) _INSTANCE = new ServerCommands(); return _INSTANCE; } } 

        public ServerCommands()
        {
            GenerateCommands();
        }

        public Func<Package, Package> FindCommand(LingoProtocol CommandName)
        {
            return Commands[CommandName];
        }

        private void GenerateCommands()
        {
            Commands[LingoProtocol.PING] = (parameters) =>
            {
                Package retVal = new Package(LingoProtocol.PING, "Pong");
                System.Console.WriteLine("Received " + (string) parameters.transmittedObject);
                return retVal;
            };
        }
    }
}
