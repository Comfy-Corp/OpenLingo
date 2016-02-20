using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenLingoClient.View;
using LingoLib;

namespace OpenLingoClient.Control
{
    //Console based menu for debugging
    /*public class Menu
    {

        public static Menu currentMenu;

        public Menu()
        {
            MenuCommands.Create();
            currentMenu = this;
            Console.Title = "OpenLingo - Debug";
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\"give\" to generate a new word.");
            Console.WriteLine("\"lang\" followed by EN or NL to change language. ex: lang EN");
            Console.WriteLine("Use numbers to change length.");
            Console.Write("Current word length: '" + Config.WordLength + "' current language: '");
            switch (Config.Language)
            {
                case LANGUAGE.DUTCH:
                    Console.Write("NL");
                    break;
                case LANGUAGE.ENGLISH:
                    Console.Write("EN");
                    break;
                default:
                    Console.Write("None");
                    break;
            }
            Console.WriteLine("'.");
        }

        public void Heartbeat()
        {
            
            if (Config.Language == LANGUAGE.NONE)
            {
                switch(Console.ReadLine().Trim().ToLower()){
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
            string input = Console.ReadLine();
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
            Console.WriteLine("Bye bye");
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
                            List<PlayerInfo> peers = Net.ServerNet.Client.ConnectToLobby();
                            foreach (var peer in peers)
                            {
                                System.Console.WriteLine(peer.Username);
                            }
                        }
                        else if (parameters.Trim().ToLower() == "leave")
                        {
                            Net.ServerNet.Client.DisconnectFromLobby();
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
                        if(!Net.ServerNet.Client.IsConnected)
                        {
                            if (!Net.ServerNet.Client.Init())
                            {
                                System.Console.WriteLine("Connection failed");
                                return;
                            }
                            System.Console.WriteLine("Connection established");
                        }
                        Net.ServerNet.Client.Ping();
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
    }*/
}
