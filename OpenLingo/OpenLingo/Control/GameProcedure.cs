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
     * A card is generator of 25 even or uneven numbers
     * 8 numbers on the card are randomly removed
     * The player who goes first will have a word picked for the first word
     * Computer reveals the first letter to the first player
     * 
     * TODO: This might be handier to handle on the server, 
     *       Same for the match procedure.
     */
    class GameProcedure
    {
        int[] PlayerCard;
        //If you use UtcNow you'd expect a long. You get another DateTime instead.
        Random Random = new Random((int) DateTime.UtcNow.Ticks); 

        public GameProcedure()
        {
            PlayerCard = GenerateCard(true); 
            StrikeOff();
        }

        /**
         * Generate the card of even or uneven numbers
         */
        public int[] GenerateCard(bool isEven)
        {
            int[] playerCard = new int[25]; //25 numbers total
            
            int count = 1;
            int index = (isEven) ? 2 : 1;
            
            while(count < 25)
            {
                index+=2;
                if(Random.Next(0,2) == 1)
                {
                    playerCard[count] = index;
                    count++;
                }
                index = (index>70) ? ((isEven) ? 2 : 1) : index;
            }
            return playerCard;
        }

        // Select 8 from the current set playercard and strike off.
        public void StrikeOff()
        {
            
        }

        public void CheckLingo()
        {

        }

        public void PlayMatch()
        {
            MatchProcedure match = new MatchProcedure(); //fresh
            match.PlayMatch();
        }

    }
}
