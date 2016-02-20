using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using LingoLib;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace OpenLingoClient.Control.Net
{
    class HttpHandler
    {
        //I have not thought about where to handle things yet
        private static LANGUAGE tempHardCodedLanguage = LANGUAGE.ENGLISH; 

        private static readonly string wordsEnListAddress = "http://zaku2.com/eemonImg/wordsEn.txt"; //Static for now
        private static readonly string wordsNlListAddress = "http://zaku2.com/eemonImg/wordsNl.txt"; //Should use ISO codes
        private static string wordsListAddress
        {
            get
            {
                switch (tempHardCodedLanguage)
                {
                    case LANGUAGE.DUTCH:
                        return wordsNlListAddress;
                    case LANGUAGE.ENGLISH:
                        return wordsEnListAddress;
                    default:
                        return wordsEnListAddress;
                }
            }
            set { wordsListAddress = value; }
        }

        /**
         * True if new version available 
         */
        public static bool WordsListVersionCheck(int localVersion)
        {
            bool retval = false;
            int versionNo;
            if (tempHardCodedLanguage == LingoLib.LANGUAGE.DUTCH) //Only works for Guus's version system
            {
                try
                {
                    using (var client = new WebClient())
                    using (StreamReader downloadStream = new StreamReader(client.OpenRead(wordsListAddress)))
                    {
                        string version = downloadStream.ReadLine();
                        if (version.StartsWith("v="))
                        {
                            if (int.TryParse(version.Substring(2, 1), out versionNo))
                            {
                                if (localVersion < versionNo)
                                {
                                    retval = true;
                                    localVersion = versionNo;
                                    FileManager.WordListDispose();
                                }
                            }
                        }
                    }
                }
                catch (WebException e)
                {
                    if(e.HResult == 404)
                        Console.WriteLine("No wordslist available");
                }
            }
            return retval;
        }

        /**
         * Would not have guessed it was THIS simple.
         * Wireshark captures of this method are stored in project's picture folder
         */
        public static void WordsListAcquire()
        {
            try
            {

                int count = 0;
                string word;
                using (var client = new WebClient())
                using (StreamReader downloadStream = new StreamReader(client.OpenRead(wordsListAddress)))
                {
                    System.Console.WriteLine("Building wordslist...");
                    while ((word = downloadStream.ReadLine()) != null)
                    {
                        if (!word.StartsWith("v="))
                        {
                            FileManager.WordListBuild(word);
                            count++;
                            if ((count % 500 == 0))
                            {

                                int currentLineCursor = Console.CursorTop;
                                Console.SetCursorPosition(0, Console.CursorTop);
                                System.Console.WriteLine("Read approx. " + count + " words...");
                                Console.SetCursorPosition(0, currentLineCursor);
                            }
                        }
                    }

                }
                System.Console.WriteLine("Wordslist expanded with " + count + " words.");
            }
            catch (WebException)
            {
                System.Console.WriteLine("Webrequest failed. Download or create your own wordslist");
            }
        }
    }
}
