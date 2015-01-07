using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLingoClient.View
{
    /**
     * Secondary singleton non abstract view adapter container.
     * Here we define the other debug output that doesn't appear in regular matches.
     */
    public class MainView
    {
        private static IViewBridge viewInterface;

        public static IViewBridge ViewInterface
        {
            get
            {
                if (viewInterface == null) SetView(ViewStates.VIEW_FORM); //Default debug output to console
                return viewInterface;
            }
            set 
            {
                viewInterface = value;  
            }
        }

        public static void SetView(ViewStates state)
        {
            switch(state)
            {
                case ViewStates.VIEW_CONSOLE:
                    ViewInterface = new ConsoleView();
                    break;
                case ViewStates.VIEW_FORM:
                    throw new NotImplementedException();
                default:
                    //Should not be possible
                    ViewInterface = new ConsoleView();
                    break;
            }
        }
    }

    public enum ViewStates
    {
        VIEW_CONSOLE,
        VIEW_FORM
    }
}
