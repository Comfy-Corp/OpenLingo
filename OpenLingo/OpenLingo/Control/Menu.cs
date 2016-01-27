﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenLingoClient.View;
using LingoLib;

namespace OpenLingoClient.Control
{
    public class Menu
    {
        private static MenuView _screen;
        public static MenuView Screen
        {
            get
            {
                if (_screen == null)
                {
                    _screen = Config.IsDebug ? new MenuView(View.MenuView.ViewTypes.VIEW_CONSOLE) : new MenuView(View.MenuView.ViewTypes.VIEW_FORM);
                }
                return _screen;
            }
            set
            {
                _screen = value;
            }
        }

        public static Menu currentMenu;

        public Menu()
        {
            MenuCommands.Create();
            currentMenu = this;
        }

        public void Heartbeat()
        {

            if (Config.Language == LANGUAGE.NONE)
            {
                switch(Screen.MenuSetLanguagePrompt().Trim().ToLower()){
                    case "nl":
                        Config.Language = LANGUAGE.DUTCH;
                        break;
                    case "en":
                        Config.Language = LANGUAGE.ENGLISH;
                        break;
                    default:
                        Console.WriteLine("Unrecognised input, defaulting to NL. You can change this with the Lang command");
                        Config.Language = LANGUAGE.DUTCH;
                        break;
                 }
            }
            string input = Screen.MenuRead();
            input.ToLower().Trim();
            string command = input.Split()[0];
            string parameters = string.Empty;
            if (input.Contains(' '))
                parameters = input.Substring(input.IndexOf(' '));
            try
            {
                MenuCommands.FindCommand(command).Execute(parameters);
            }
            catch (NullReferenceException)
            {
                //TODO find a place for erros
                Console.WriteLine("Command not found");
            }
        }

        public void CloseMenu()
        {
            Screen.MenuExit();
            Screen = null;
        }

        public void MenuCommand(string command, string parameters)
        {
            MenuCommands.FindCommand(command.ToLower().Trim()).Execute(parameters);
        }

        private static class MenuCommands
        {
            private static List<MenuCommand> CommandsList = new List<MenuCommand>();

            /// Get all names of commands.
            public static string[] CommandNames()
            {
                string[] str = new String[CommandsList.Count];
                for (int i = 0; i < str.Length; i++)
                {
                    str[i] = CommandsList[i].Name;
                }
                return str;
            }

            /// Look for a certain command by name
            public static MenuCommand FindCommand(String commandName)
            {
                MenuCommand result = null;
                foreach (MenuCommand c in MenuCommands.CommandsList)
                {
                    if (c.Name.Trim().ToLower() == commandName.Trim().ToLower())
                    {
                        result = c;
                    }
                }
                return result;
            }

            public static void Create()
            {
                #region Add commands
                CommandsList.Add(new MenuCommand("About",
                        "Fun facts & shout-outs",
                        delegate(string parameters)
                        {

                        }));
                CommandsList.Add(new MenuCommand("Lobby",
                    "Enter the server's player lobby",
                    delegate(string parameters)
                    {
                        if (parameters.Trim().ToLower() == "join")
                        {
                            List<PlayerInfo> peers = Net.LobbyNet.Client.ConnectToLobby();
                            foreach (var peer in peers)
                            {
                                System.Console.WriteLine(peer.Username);
                            }
                        }
                        else if (parameters.Trim().ToLower() == "leave")
                        {
                            Net.LobbyNet.Client.DisconnectFromLobby();
                        }
                    }));
                CommandsList.Add(new MenuCommand("Card",
                        "Generate a card",
                        delegate(string paramers){
                            LingoCard card = new LingoCard(true, LingoCard.generateLingoCardMask());
                            System.Console.WriteLine("Card: ");
                            System.Console.WriteLine(card);

                        }));
                CommandsList.Add(new MenuCommand("Username",
                        "Set a username",
                        delegate(string parameters)
                        {
                            if((parameters != null) || (parameters != ""))
                            {
                                Config.LocalPlayer.Username = parameters;
                                Config.SaveConfig();
                            }
                        }));
                CommandsList.Add(new MenuCommand("NetTest",
                    "Ping the server",
                    delegate(string parameters)
                    {
                        if(!Net.LobbyNet.Client.IsConnected)
                        {
                            if (!Net.LobbyNet.Client.Init())
                            {
                                System.Console.WriteLine("Connection failed");
                                return;
                            }
                            System.Console.WriteLine("Connection established");
                        }
                        Net.LobbyNet.Client.Ping();
                    }));

                CommandsList.Add(new MenuCommand("Help",
                        "This help command.",
                        delegate(string parameters)
                        {
                            foreach (MenuCommand c in CommandsList)
                            {
                                Console.WriteLine(c.ToString());
                                Console.WriteLine("");
                            }
                        }));
                CommandsList.Add(new MenuCommand("Lang",
                        "Change language to argument",
                        delegate(string parameters)
                        {
                            switch (parameters.Trim().ToLower())
                            {
                                case "nl":
                                    Config.Language = LANGUAGE.DUTCH;
                                    Screen.MenuSetLanguageDone();
                                    break;
                                case "en":
                                    Config.Language = LANGUAGE.ENGLISH;
                                    Screen.MenuSetLanguageDone();
                                    break;
                            }

                        }));
                CommandsList.Add(new MenuCommand("Ver",
                        "Display the version of the currently used wordslist",
                        delegate(string parameters)
                        {
                            Screen.MenuDisplayVer();
                        }));
                CommandsList.Add(new MenuCommand("Give",
                        "Display a random word of set letters in set language",
                        delegate(string parameters)
                        {
                            string randomWord = FileManager.GenerateRandomWord(Config.WordLength);
                            if (randomWord != null)
                                Console.WriteLine(randomWord);
                            else
                                Console.WriteLine("Failed.");
                        }));
                CommandsList.Add(new MenuCommand("Play",
                        "Play a bout of Lingo",
                        delegate(string parameters)
                        {
                            GameProcedure p = new GameProcedure();
                            p.PlayMatch();
                        }));
                CommandsList.Add(new MenuCommand("WordLength",
                         "Change the word length to the paremeter",
                         delegate(string parameters)
                         {
                             int temp;
                             if (int.TryParse(parameters, out temp))
                             {
                                 Config.WordLength = temp;
                             }
                         }));
                CommandsList.Add(new MenuCommand("Quit",
                          "Quit the application",
                          delegate(string parameters)
                          {
                              Menu.currentMenu.CloseMenu();
                              MainClass.isRunning = false;
                          }));
                #endregion
            }

            public class MenuCommand
            {
                public string Name;
                public string Description;
                public Behaviour command;

                public delegate void Behaviour(string paramaters);

                public MenuCommand(string Name, string Description, Behaviour Command)
                {
                    this.Name = Name;
                    this.Description = Description;
                    this.command = Command; //Hold command
                }

                public void Execute(string paramaters)
                {
                    command(paramaters); //Do held back command
                }

                public override string ToString()
                {
                    return "-> " + Name + " \n" + "'" + Description + "'";
                }
            }
        }
    }
}