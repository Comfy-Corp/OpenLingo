using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LingoLib;

namespace OpenLingoClient.Control
{
    /**
     * State machine controlling the window contents.
     * WHAT IS THIS EVEN FOR OH GOD REMOVE THIS RELIC OF THE PAST PLEASE
     */
    class GameStateMachine
    {
        private static GameStateMachine INSTANCE;

        public MatchSession Match;

        public int applicationState;

        public static GameStateMachine GetInstance(){
            if(INSTANCE == null)
                INSTANCE = new GameStateMachine();
            return INSTANCE;
        }
    }


}
