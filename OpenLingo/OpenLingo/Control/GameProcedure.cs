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
        /**
         * The Alpha and Omega.
         */
        public static void PlayMatch()
        {
            //Host/server should make this, transmit to client(s)
            MatchSession ms = new MatchSession(FileManager.GenerateRandomWord(Config.WordLength), Config.LocalPlayer);
            Console.WriteLine(ms.Progression);
            char[] clearCondition = new char[ms.CurrentWord.Length];
            for (int i = 0; i < ms.CurrentWord.Length; i++)
			{
			    clearCondition[i] = '+';
			}
            while (ms.Progression != new string(clearCondition))
            {
                string guess = Console.ReadLine();
                if (guess == "QUIT")
                    break;
                if (guess == "GIVE UP")
                    Console.WriteLine("Answer: "+ms.CurrentWord);
                else
                    guess = guess.ToLower();
                if (guess.Length == ms.CurrentWord.Length && FileManager.WordsListContains(guess))
                {
                    ms.Progression = MatchSession.MatchGuess(guess, ms.CurrentWord);
                    Console.WriteLine(ms.Progression+" :Progression");
                }
                else
                {
                    Console.WriteLine("Invalid guess, lose turn!");
                }
            }
        }
    }
}
