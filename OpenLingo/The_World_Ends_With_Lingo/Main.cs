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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/
            System.Console.Title = "The World Ends With Lingo - Debug";
            string randomWord = FileReader.GenerateRandomWord(5);
            if (randomWord != null) Console.WriteLine(randomWord); else Console.WriteLine("Failed");
            System.Console.ReadKey();
        }
    }
}
