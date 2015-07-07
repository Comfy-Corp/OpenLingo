using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LingoLib;

namespace OpenLingoClient.View
{
    /**
     * Secondary singleton non abstract view adapter container.
     * Here we define the other debug output that doesn't appear in regular matches.
     */
    public class MatchView
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

        public MatchView(ViewTypes state)
        {
            SetView(state);
        }

        public void SetView(ViewTypes state)
        {
            switch(state)
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

        public void MatchDisplayStart(MatchSession ms)
        {
            ViewInterface.MatchDisplayStart(ms);
        }

        public void MatchDispayAnswer(string answer)
        {
            viewInterface.MatchDispayAnswer(answer);
        }

        public string MatchPromptGuess()
        {
            return viewInterface.MatchPromptGuess();
        }

        public void MatchDisplayIncorrect(MatchSession ms)
        {
            viewInterface.MatchDisplayIncorrect(ms);
        }

        public void MatchDisplayWin()
        {
            viewInterface.MatchDisplayWin();
        }

        public void MatchDisplayLose(MatchSession ms)
        {
            viewInterface.MatchDisplayLose(ms);
        }

        public void MatchDisplayProgress(MatchSession ms, string guess)
        {
            viewInterface.MatchDisplayProgress(ms, guess);
        }

        public enum ViewTypes
        {
            VIEW_CONSOLE,
            VIEW_FORM
        }

        private interface IViewBridge
        {
            void MatchDisplayStart(MatchSession ms);

            void MatchDispayAnswer(string answer);

            string MatchPromptGuess();

            void MatchDisplayIncorrect(MatchSession ms);

            void MatchDisplayWin();

            void MatchDisplayLose(MatchSession ms);

            void MatchDisplayProgress(MatchSession ms, string guess);
        }

        private class ConsoleView : IViewBridge
        {
            public void MatchDisplayStart(MatchSession ms)
            {
                Console.WriteLine("Starting a match of lingo \n Good luck");
                Console.WriteLine("Try #1");
                for (int i = 0; i < ms.CurrentWord.Length; i++)
                {
                    if (ms.Progression[i] == '+')
                        Console.Write(ms.CurrentWord[i]);
                    else
                        Console.Write('.');
                }
                Console.Write('\n');
            }

            public void MatchDispayAnswer(string answer)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Answer: " + answer);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            public string MatchPromptGuess()
            {
                return Console.ReadLine();
            }

            public void MatchDisplayIncorrect(MatchSession ms)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid guess, lose turn!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Try #" + (ms.Attempts.Count + 1));
            }

            public void MatchDisplayWin()
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Congratulations!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            public void MatchDisplayLose(MatchSession ms)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Too bad, the answer was: " + ms.CurrentWord);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            public void MatchDisplayProgress(MatchSession ms, string guess)
            {
                for (int i = 0; i < ms.CurrentWord.Length; i++)
                {
                    if (ms.Progression[i] == '+')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(guess[i]);
                    }
                    else if (ms.Progression[i] == '-')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(guess[i]);
                    }
                    else // Assume '.'
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(guess[i]);
                    }
                    System.Threading.Thread.Sleep(100);
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write('\n');
                Console.WriteLine("Try #" + (ms.Attempts.Count + 1));
            }
        }
    }
}
