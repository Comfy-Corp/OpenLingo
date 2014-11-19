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
        public List<Player> Players;
        public int Turn { get; private set; } //Matches Players index
        public LinkedList<KeyValuePair<String, String>> Attempts; //String attempt, String progression
        public string CurrentWord { get; private set; }
        public string Progression;

        /**
         * Construct a match in a game of lingo with parameter word as answer
         * player will be the index of which player starts.
         */
        public MatchSession(string currentWord, Player host)
        {
            this.CurrentWord = currentWord;
            Players = new List<Player>();
            Players.Add(host); //Host first
            Turn = new Random().Next() % Players.Count;
            Attempts = new LinkedList<KeyValuePair<string, string>>();
            char[] prog = new char[currentWord.Length];
            prog[0] = currentWord[0];
            for (int i = 1; i < currentWord.Length; i++)
            {
                prog[i] = '.';
            }
            Progression = new string(prog);
        }

        public void NextTurn()
        {
            Turn++;
            Turn %= Players.Count;
        }

        /**
         * + Right letter at right place
         * - Right letter at wrong place
         * . Wrong letter at wrong place
         *   Wrong place, wrong time
         */
        public static string MatchGuess(string guess, string currentWord)
        {
            char[] prog = new char[currentWord.Length];
            char[] temp = currentWord.ToCharArray();
            
            for (int i = 0; i < currentWord.Length; i++)
            {
                if (temp[i] == guess[i])
                {
                    prog[i] = '+';
                    temp[i] = '.';
                }
                else
                {
                    if (temp.Contains(guess[i]))
                    {
                        prog[i] = '-';
                        temp[currentWord.IndexOf(guess[i])] = '.';
                    }
                    else
                    {
                        prog[i] = '.';
                    }
                }
                currentWord = new string(temp);
            }
            return new string(prog);
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

    public enum MatchStates
    {
        MENU = 1,
        IN_GAME = 2,
        GAME_OVER = 3
    }
}
