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
        public LANGUAGE Language;
        public int WordsListVersion;
        public int WordLength;   //How long are the in-game words?
        public int MatchAmount;  //how many matches will be played?

        public ConfigParameters()
        {
            this.Language = LANGUAGE.NONE;
        }
    }

    public enum LANGUAGE
	{
	    DUTCH,
        ENGLISH,
        NONE
	}
}
