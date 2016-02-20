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

            Commands[LingoProtocol.REGISTER_LOBBY] = (parameters) =>
            {
                Package retVal = new Package(LingoProtocol.NOK, "Failed to register in the lobby. Reason unknown.");
                Lobby.Players.Add(parameters.transmittedObject as PlayerInfo);
                if (Lobby.Players.Contains(parameters.transmittedObject as PlayerInfo))
                {
                    PlayerInfo a = parameters.transmittedObject as PlayerInfo;
                    System.Console.WriteLine("Registered user: " + a.Username);
                    retVal = new Package(LingoProtocol.OK, "Success");
                }
                return retVal;
            };

            Commands[LingoProtocol.UNREGISTER_LOBBY] = (parameters) =>
            {
                PlayerInfo toRemove = parameters.transmittedObject as PlayerInfo;
                Package retVal = new Package(LingoProtocol.NOK, "Failed to unregister from the lobby. Reason unknown.");
                Lobby.Players.Remove(toRemove);
                if (!Lobby.Players.Contains(toRemove))
                {
                    System.Console.WriteLine("Unregistered user: " + toRemove.Username);
                    retVal = new Package(LingoProtocol.OK, "Success");
                }
                return retVal;
            };

            Commands[LingoProtocol.GET_LOBBY_PLAYERS] = (parameters) =>
            {
                Package retVal = new Package(LingoProtocol.OK, Lobby.Players);
                return retVal;
            };

        }
    }
}
