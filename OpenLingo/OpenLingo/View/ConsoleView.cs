using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LingoLib;

namespace OpenLingoClient.View
{
    class ConsoleView : IViewBridge
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
