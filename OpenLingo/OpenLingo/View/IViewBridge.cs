using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LingoLib;

namespace OpenLingoClient.View
{
    public interface IViewBridge
    {
        void MatchDisplayStart(MatchSession ms);

        void MatchDispayAnswer(string answer);

        string MatchPromptGuess();

        void MatchDisplayIncorrect(MatchSession ms);

        void MatchDisplayWin();

        void MatchDisplayLose(MatchSession ms);

        void MatchDisplayProgress(MatchSession ms, string guess);
    }
}
