using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenLingoClient.Control;


namespace OpenLingoClient
{
    static class MainClass
    {
        public static bool isRunning = true;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            /*if(args.Length > 0 && args[0].Equals("-debug"))
                Config.IsDebug = true;*/
            Control.Net.ServerNet.Client.Init();
            //Control.Menu menu = new Control.Menu();
            //Debug console menu
            //while(isRunning) 
            //    menu.Heartbeat();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new View.LobbyView());
        }
    }
}
