using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenLingoClient.Control;
using LingoLib;

namespace OpenLingoClient
{
    static class MainClass
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args[0] == "-debug")
                View.MainView.SetView(View.ViewStates.VIEW_CONSOLE);
            else
                View.MainView.SetView(View.ViewStates.VIEW_FORM);
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/
            Console.Title = "OpenLingo - Debug";
            Console.ForegroundColor = ConsoleColor.Gray;

            if (Config.LocalPlayer == null)
                Config.LocalPlayer = new Player("Default");
            if (Config.WordLength == 0)  //A word is never allowed to be of length 0 or lower
                Config.WordLength = 5;
            if (Config.GuessAttempts == 0) //Same goes for the amount of attempts, unless I decide to implement -1 as infite guesses
                Config.GuessAttempts = 5;

            string input = "void";
            
            while (Config.Language == LANGUAGE.NONE)
            {
                Console.WriteLine("Language? NL / EN");
                Console.Write('>');
                input = Console.ReadLine();
                switch (input.ToLower())
                {
                    case "nl":
                        Config.Language = LANGUAGE.DUTCH;
                        break;
                    case "en":
                        Config.Language = LANGUAGE.ENGLISH;
                        break;
                }
            }
            Console.WriteLine("\"give\" to generate a new word.");
            Console.WriteLine("\"lang\" followed by EN or NL to change language. ex: lang EN");
            Console.WriteLine("Use numbers to change length.");

            //user IO
            while (input != "quit")
            {
                Console.Write('>');
                input = Console.ReadLine();
                int temp;
                if (!int.TryParse(input, out temp))
                {
                    switch (input.ToLower())
                    {
                        case "lang nl":
                            Config.Language = LANGUAGE.DUTCH;
                            Console.WriteLine("Language is now Dutch.");
                            break;
                        case "lang en":
                            Config.Language = LANGUAGE.ENGLISH;
                            Console.WriteLine("Language is now English.");
                            break;
                        case "lang":
                            Console.WriteLine("Language is "+Config.Language);
                            break;
                        case "give":
                            string randomWord = FileManager.GenerateRandomWord(Config.WordLength);
                            if (randomWord != null)
                                Console.WriteLine(randomWord);
                            else
                                Console.WriteLine("Failed.");
                            break;
                        case "ver":
                            Console.WriteLine("Words list Version: "+Config.WordsListVersion);
                            break;
                        case "play":
                            GameProcedure p = new GameProcedure();
                            p.PlayMatch();
                            break;
                        default:
                            if (FileManager.WordsListContains(input.ToLower().Trim()))
                                Console.WriteLine("Yes");
                            else
                                Console.WriteLine("No");
                            break;
                    }
                }
                else
                {
                    Config.WordLength = temp;
                    Console.WriteLine("Words will be " + Config.WordLength + " letters.");
                }
            }
        }
    }
}
