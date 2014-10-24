using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_World_Ends_With_Lingo.Control
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
        public static void PlayGame()
        {
            string gameWord = FileReader.GenerateRandomWord(5);
        }
    }
}
