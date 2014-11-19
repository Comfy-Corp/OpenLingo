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
            MatchSession ms = new MatchSession(FileManager.GenerateRandomWord(5));
            
        }
    }
}
