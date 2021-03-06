﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LingoLib;

namespace OpenLingoClient.Control
{
    /**
     * Handle a match within a game, this is one player
     */
    public class MatchManager
    {
        private MatchSession ms;

        public MatchManager()
        {
            ms = new MatchSession(FileManager.GenerateRandomWord(5));
        }

        public MatchManager(MatchSession ms) //Continue previous match session?
        {
            this.ms = ms;
        }

        /**
         * A match is a single guessing game
         * Returns the result (WIN, LOSE or DRAW)
         */
        public MatchResultType PlayMatch()
        {
            //Create view
            //MatchView display = new MatchView(MatchView.ViewTypes.VIEW_CONSOLE);
            MatchResultType retVal = MatchResultType.DRAW;
            ms.maxAttempts = 5;
            //Host/server should make this, transmit to client(s)
            char[] clearCondition = new char[ms.CurrentWord.Length];
            for (int i = 0; i < ms.CurrentWord.Length; i++)
            {
                clearCondition[i] = '+';
            }

            //Console.WriteLine("Answer: {0}", ms.CurrentWord);
            while (ms.Progression != new string(clearCondition) && ms.Attempts.Count < ms.maxAttempts)
            {
                //Prompt guess
                string guess = "temp";
                if (guess == "I quit")
                    return MatchResultType.LOSE;
                else if (guess == "I give up")
                {
                    //display.MatchDispayAnswer(ms.CurrentWord);
                    return MatchResultType.LOSE;
                }
                else
                {
                    guess = guess.ToLower();
                }
                if (guess.Length == ms.CurrentWord.Length && FileManager.WordsListContains(guess))
                {
                    ms.Progression = MatchSession.MatchGuess(guess, ms.CurrentWord);
                    //display.MatchDisplayProgress(ms, guess);
                }
                else
                {
                    //display.MatchDisplayIncorrect(ms);
                }
                ms.Attempts.Add(new KeyValuePair<string,string>(guess, ms.Progression));
            }
            if (ms.Progression == new string(clearCondition))
            {
                retVal = MatchResultType.WIN;
                //display.MatchDisplayWin();
            }
            else
            {
                retVal = MatchResultType.WIN;
                //I want to move the display function up to GameProcedure,
                //but it should still be able to access the information of the session
                //display.MatchDisplayLose(ms);
            }
            return retVal;
        }
    }
}
