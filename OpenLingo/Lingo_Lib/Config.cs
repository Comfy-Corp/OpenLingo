using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingoLib
{
    /**
     * Has room for growth, I couldn't think of many options yet.
     */
    [Serializable]
    public class ConfigParameters
    {
        int WordLength;   //How long are the in-game words?
        int MatchAmount;  //how many matches will be played?
    }
}
