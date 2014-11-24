using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LingoLib;

namespace OpenLingoClient.Control
{
    /**
     * A game starts with deciding the player who goes first
     * The player who goes first will have a word picked for the first word
     * Computer reveals the first letter to the first player
     * 
     */
    class GameProcedure
    {
        private MatchSession ms;

        public GameProcedure()
        {
            ms = new MatchSession(FileManager.GenerateRandomWord(Config.WordLength), Config.LocalPlayer);
        }

        public GameProcedure(MatchSession ms)
        {
            this.ms = ms;
        }

        /**
         * The Alpha and Omega.
         */
        public void PlayMatch()
        {
            //Host/server should make this, transmit to client(s)
            char[] clearCondition = new char[ms.CurrentWord.Length];
            for (int i = 0; i < ms.CurrentWord.Length; i++)
            {
                clearCondition[i] = '+';
            }
            DisplayProgress();
            while (ms.Progression != new string(clearCondition))
            {
                string guess = Console.ReadLine();
                if (guess == "I quit")
                    return;
                if (guess == "I give up")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Answer: " + ms.CurrentWord);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return;
                }
                else
                {
                    guess = guess.ToLower();
                }
                if (guess.Length == ms.CurrentWord.Length && FileManager.WordsListContains(guess))
                {
                    ms.Progression = MatchSession.MatchGuess(guess, ms.CurrentWord);
                    DisplayProgress(guess);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid guess, lose turn!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Congratulations!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void DisplayProgress()
        {
            for (int i = 0; i < ms.CurrentWord.Length; i++)
            {
                if (ms.Progression[i] == '+')
                    Console.Write(ms.CurrentWord[i]);
                else
                    Console.Write('.');
            }
            Console.Write('\n');
        }

        public void DisplayProgress(string guess)
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
        }
    }
}
