using System;
using System.Collections.Generic;
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

        //Want to hear a fun thing you can do with this?
        //Imagine the following
        //Make a live interpreter for the server, and hot swap the commands.
        //And then we're Erlang-like and have (moderate) live patching of commands.
        //Never gonna make that though
        private void GenerateCommands()
        {
            Commands[LingoProtocol.PING] = (parameters) =>
            {
                Package retVal = new Package(LingoProtocol.PING, "Pong");
                Console.WriteLine("Received " + (string) parameters.transmittedObject);
                return retVal;
            };

            Commands[LingoProtocol.REGISTER_LOBBY] = (parameters) =>
            {
                Package retVal = new Package(LingoProtocol.NOK, "Failed to register in the lobby. Reason unknown.");
                PlayerInfo newPlayer = parameters.transmittedObject as PlayerInfo;
                if (!Lobby.AddUnique(parameters.transmittedObject as PlayerInfo))
                    retVal = new Package(LingoProtocol.ALREADY_REGISTERED, "The user \"" + newPlayer.Username+"\" has already been registered.");
                else if (Lobby.Players.Contains(parameters.transmittedObject as PlayerInfo))
                {
                    PlayerInfo a = parameters.transmittedObject as PlayerInfo;
                    Console.WriteLine("Registered user: " + a.Username);
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
                    Console.WriteLine("Unregistered user: " + toRemove.Username);
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
