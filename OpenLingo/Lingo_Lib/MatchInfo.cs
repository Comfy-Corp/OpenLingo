using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace LingoLib
{
    /**
     * Match masterclass
     * A match is a subpart to a game, multiple matches are held
     * Contains general information on the progression of an active Lingo match.
     * This'll have to be synced between players, therefore placed in the lib
     */
    [Serializable]
    public class MatchSession
    {
        public List<KeyValuePair<String, String>> Attempts; //String attempt, String progression
        public string CurrentWord { get; private set; }     //Answer to the riddle
        public string Progression;                          //The latest valid progress made
        public int maxAttempts;                             //Amount of attempts possible untill LOSE condition

        /**
         * Construct a match in a game of lingo with parameter word as answer
         * player will be the index of which player starts.
         */
        public MatchSession(string currentWord)
        {
            this.CurrentWord = currentWord;
            Attempts = new List<KeyValuePair<string, string>>();
            char[] prog = new char[currentWord.Length];
            prog[0] = '+';
            for (int i = 1; i < currentWord.Length; i++)
            {
                prog[i] = '.';
            }
            Progression = new string(prog);
        }

        /**
         * + Right letter at right place
         * - Right letter at wrong place
         * . Wrong letter at wrong place
         *   Wrong place, wrong time
         */
        public static string MatchGuess(string guess, string currentWord)
        {
            char[] tempGuess = guess.ToLower().Trim().ToCharArray();
            char[] tempCurrentWord = currentWord.ToLower().Trim().ToCharArray();
            char[] retVal = new char[guess.Length];

            for (int i = 0; i < tempGuess.Length; i++)
            {
                if (tempGuess[i] == tempCurrentWord[i])
                {
                    retVal[i] = '+';
                    tempGuess[i] = ' ';
                    tempCurrentWord[i] = ' ';
                }
            }
            for (int i = 0; i < tempGuess.Length; i++)
            {
                if (tempGuess[i] != ' ')
                {
                    int matchPosition = Array.IndexOf(tempCurrentWord, tempGuess[i]);
                    if (matchPosition != -1)
                    {
                        retVal[i] = '-';
                        tempGuess[i] = ' ';
                        tempCurrentWord[matchPosition] = ' ';
                    }
                }
            }
            for (int i = 0; i < retVal.Length; i++)
            {
                if (Char.IsLetter(retVal[i]))
                    retVal[i] = '+';
            }
            return new string(retVal);
        }
    }

    /**
     * Player information
     * Stores both match-specific and non-match-specific information
     */
    [Serializable]
    public class Player
    {
        public int Score;
        public string Name;

        public Player(string name)
        {
            Name = name;
        }
    }

    public enum MatchResultType
    {
        WIN  = 1,
        DRAW = 2,
        LOSE = 3
    }
}
