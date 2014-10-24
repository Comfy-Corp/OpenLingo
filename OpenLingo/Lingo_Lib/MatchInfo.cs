using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace LingoLib
{
    /**
     * Game masterclass
     * Contains general information on the progression of an active Lingo match.
     * This'll have to be synced between players, therefore placed in the lib
     */
    public class GameSession
    {
        public List<Player> Players;
        private int turn; //Matches Players index
        LinkedList<KeyValuePair<String,String>> attempts; //String attempt, String
        string currentWord; 
        string progression;

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
        public NetHandler playerAddress;
        public int score;
        public string name;
    }

    public enum GameStates
    {
        MENU = 1,
        IN_GAME = 2,
        GAME_OVER = 3
    }
}
