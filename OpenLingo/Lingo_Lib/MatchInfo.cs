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
    public class MatchSession
    {
        public List<Player> Players;
        int turn { public get; private set; } //Matches Players index
        public LinkedList<KeyValuePair<String,String>> attempts; //String attempt, String
        string currentWord { public get; private set; }
        public string progression;

        /**
         * Construct a match in a game of lingo with parameter word as answer
         * player will be the index of which player starts.
         */
        public MatchSession(string currentWord)
        {
            this.currentWord = currentWord;
            turn = new Random().Next() % Players.Count;
            attempts = new LinkedList<KeyValuePair<string, string>>();
            Players = new List<Player>();
            char[] prog = new char[currentWord.Length];
            for (int i = 0; i < currentWord.Length; i++)
            {
                prog[i] = '.';
            }
            progression = new string(prog);
        }

        public void NextTurn()
        {
            turn++;
            turn %= Players.Count;
        }
    }

    /**
     * Player information
     * Stores both match-specific and non-match-specific information
     */
    public class Player
    {
        public int score;
        public string name;
    }

    public enum MatchStates
    {
        MENU = 1,
        IN_GAME = 2,
        GAME_OVER = 3
    }
}
