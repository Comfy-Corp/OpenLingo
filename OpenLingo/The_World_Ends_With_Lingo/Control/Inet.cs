using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace OpenLingoClient.Control.Net
{
    /**
     * Provide a means of finding other players and constructing a game 
     * We'll be using our own pseudo Session Description Protocol to become aware of others
     */
    class LobbyNet
    {

    }

    class HttpHandler
    {
        private static string wordsListAddress = "http://www-01.sil.org/linguistics/wordlists/english/wordlist/wordsEn.txt";

        /**
         * Would not have guessed it was THIS simple.
         * Wireshark captures of this method are stored in project's picture folder
         */
        public static void AcquireWordsList()
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
                        FileReader.BuildWordList(word);
                        count++;
                        if ((count % 100 == 0))
                        {
                            
                            int currentLineCursor = Console.CursorTop;
                            Console.SetCursorPosition(0, Console.CursorTop);
                            System.Console.WriteLine("Read approx. " + count + " words...");
                            Console.SetCursorPosition(0, currentLineCursor);
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
