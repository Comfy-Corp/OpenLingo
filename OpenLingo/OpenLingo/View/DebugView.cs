using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLingoClient.View
{
    /**
        * Singleton non abstract view adapter container.
        * This is mostly for the seperation of console(debug) and form (definitive)
        * We could add runtime switching, but consoles don't work that way in C#
        */
    public class DebugView
    {
        public static bool isVerbose
        {
            get;
            private set;
        }
        private static IDebugViewBridge debugInterface;

        public static IDebugViewBridge DebugInterface
        {
            get
            {
                if (debugInterface == null) SetDebugView(ViewStates.VIEW_CONSOLE);
                return debugInterface;
            }
            set
            {
                debugInterface = value;
            }
        }

        public static void SetDebugView(ViewStates state)
        {
            switch (state)
            {
                case ViewStates.VIEW_CONSOLE:
                    DebugInterface = new ConsoleDebugView();
                    break;
                case ViewStates.VIEW_FORM:
                    throw new NotImplementedException();
                default: //Should not be possible                
                    DebugInterface = new ConsoleDebugView();
                    break;
            }
        }

        public static void ToggleVerbose()
        {
            isVerbose = !isVerbose;
        }
    }
}
