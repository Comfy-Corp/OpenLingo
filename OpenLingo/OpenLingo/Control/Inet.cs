using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using LingoLib;

namespace OpenLingoClient.Control.Net
{
    /**
     * Provide a means of finding other players and constructing a game 
     */
    class LobbyNet
    {

    }

    class HttpHandler
    {
        private static readonly string wordsEnListAddress = "http://www-01.sil.org/linguistics/wordlists/english/wordlist/wordsEn.txt";
        private static readonly string wordsNlListAddress = "https://reupload.nl/Nederlands.txt";
        private static string wordsListAddress
        {
            get
            {
                switch (Config.Language)
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
        public static bool WordsListVersionCheck()
        {
            bool retval = false;
            int versionNo;
            if (Config.Language == LingoLib.LANGUAGE.DUTCH) //Only works for Guus's version system
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
                                if (Config.WordsListVersion < versionNo)
                                {
                                    retval = true;
                                    Config.WordsListVersion = versionNo;
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
