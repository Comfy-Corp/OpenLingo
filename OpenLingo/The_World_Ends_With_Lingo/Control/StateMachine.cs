using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LingoLib;

namespace The_World_Ends_With_Lingo.Control
{
    /**
     * State machine controlling the window contents.
     */
    class GameStateMachine
    {
        private static GameStateMachine INSTANCE;

        public GameSession Match;

        public int applicationState;

        public static GameStateMachine GetInstance(){
            if(INSTANCE == null)
                INSTANCE = new GameStateMachine();
            return INSTANCE;
        }
    }


}
