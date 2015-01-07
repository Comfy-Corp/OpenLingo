using System;
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
    class MatchProcedure
    {
        private MatchSession ms;

        public MatchProcedure()
        {
            //ms = new MatchSession("sever", Config.LocalPlayer);
            ms = new MatchSession(FileManager.GenerateRandomWord(Config.WordLength), Config.LocalPlayer);
        }

        public MatchProcedure(MatchSession ms) //Continue previous match session?
        {
            this.ms = ms;
        }

        /**
         * The centerpiece of a Lingo match
         * TODO: return results
         */
        public void PlayMatch()
        {
            ms.maxAttempts = Config.GuessAttempts;
            //Host/server should make this, transmit to client(s)
            char[] clearCondition = new char[ms.CurrentWord.Length];
            for (int i = 0; i < ms.CurrentWord.Length; i++)
            {
                clearCondition[i] = '+';
            }

            View.MainView.ViewInterface.MatchDisplayStart(ms);
            //Console.WriteLine("Answer: {0}", ms.CurrentWord);
            while (ms.Progression != new string(clearCondition) && ms.Attempts.Count < ms.maxAttempts)
            {        
                string guess = View.MainView.ViewInterface.MatchPromptGuess();
                if (guess == "I quit")
                    return;
                else if (guess == "I give up")
                {
                    View.MainView.ViewInterface.MatchDispayAnswer(ms.CurrentWord);
                    return;
                }
                else
                {
                    guess = guess.ToLower();
                }
                if (guess.Length == ms.CurrentWord.Length && FileManager.WordsListContains(guess))
                {
                    ms.Progression = MatchSession.MatchGuess(guess, ms.CurrentWord);
                    View.MainView.ViewInterface.MatchDisplayProgress(ms, guess);
                }
                else
                {
                    View.MainView.ViewInterface.MatchDisplayIncorrect(ms);
                }
                ms.Attempts.Add(new KeyValuePair<string,string>(guess, ms.Progression));
                ms.NextTurn();
            }
            if (ms.Progression == new string(clearCondition))
            {
                View.MainView.ViewInterface.MatchDisplayWin();
            }
            else
            {
                View.MainView.ViewInterface.MatchDisplayLose(ms);
            }
        }
    }
}
