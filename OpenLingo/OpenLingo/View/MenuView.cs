using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenLingoClient.Control;
using LingoLib;

namespace OpenLingoClient.View
{
    public class MenuView
    {
        private IViewBridge viewInterface;

        private IViewBridge ViewInterface
        {
            get
            {
                if (viewInterface == null) SetView(ViewTypes.VIEW_FORM); //Default debug output to console
                return viewInterface;
            }
            set
            {
                viewInterface = value;
            }
        }

        public MenuView(ViewTypes type)
        {
            SetView(type);
            MenuStart();
        }

        public void SetView(ViewTypes state)
        {
            switch (state)
            {
                case ViewTypes.VIEW_CONSOLE:
                    ViewInterface = new ConsoleView();
                    break;
                case ViewTypes.VIEW_FORM:
                    throw new NotImplementedException();
                default:
                    //Should not be possible
                    ViewInterface = new ConsoleView();
                    break;
            }
        }

        public void MenuStart()
        {
            ViewInterface.MenuStart();
        }

        public string MenuSetLanguagePrompt()
        {
            return viewInterface.MenuSetLanguagePrompt();
        }

        public void MenuSetLanguageDone()
        {
            viewInterface.MenuSetLanguageDone();
        }

        public string MenuRead()
        {
            return viewInterface.MenuRead();
        }

        public void MenuDisplayVer()
        {
            viewInterface.MenuDisplayVer();
        }
        public void MenuDisplayWordlength()
        {
            viewInterface.MenuDisplayWordlength();
        }
        public void MenuExit()
        {
            viewInterface.MenuExit();
        }

        public enum ViewTypes
        {
            VIEW_CONSOLE,
            VIEW_FORM
        }

        private interface IViewBridge
        {
            void MenuStart();
            string MenuRead();
            void MenuExit();
            string MenuSetLanguagePrompt();
            void MenuSetLanguageDone();
            void MenuDisplayVer();
            void MenuDisplayWordlength();
        }

        private class ConsoleView : IViewBridge
        {
            public void MenuStart()
            {
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

            public void MenuExit()
            {
                Console.WriteLine("Bye bye");
            }

            public string MenuSetLanguagePrompt()
            {
                string input;
                Console.WriteLine("Language? NL / EN");
                input = MenuRead();
                return input;
            }

            public void MenuSetLanguageDone()
            {
                Console.WriteLine("The language of words is now: " + Config.Language);
            }

            public void MenuDisplayVer()
            {
                Console.WriteLine("Words list Version: " + Config.WordsListVersion);
            }

            public void MenuDisplayWordlength()
            {
                Console.WriteLine("Lingo words will be " + Config.WordLength + " letters.");
            }

            public string MenuRead()
            {
                Console.Write('>');
                return Console.ReadLine();
                /*
                int temp;
                if (!int.TryParse(input, out temp))
                {
                 * 
                }
                else
                {
                    Config.WordLength = temp;
                    Console.WriteLine("Words will be " + Config.WordLength + " letters.");
                }*/
            }
        }
    }
}
