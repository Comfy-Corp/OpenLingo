using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenLingoClient.Control;
using LingoLib;

namespace OpenLingoClient.ViewOld
{
    /**
     * I announce this method deprecated
     * It was impractical for the scope of this project
     *
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
                
            }

            public void MenuDisplayWordlength()
            {
                Console.WriteLine("Lingo words will be " + Config.WordLength + " letters.");
            }

            public string MenuRead()
            {
                Console.Write('>');
                return Console.ReadLine();
            }
        }
    }*/
}
