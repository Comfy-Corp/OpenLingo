using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenLingoClient.Control.Net;
using LingoLib;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace OpenLingoClient.Control
{
    static class FileManager
    {
        private static readonly string directoryPath = Environment.SpecialFolder.ApplicationData.ToString() + "/Lingo";
        private static readonly string configPath = directoryPath + "/your_lingo_config.conf";

        private static string wordsPath
        {
            get { return directoryPath + "/myWords" + LANGUAGE.ENGLISH + ".txt"; }
            set { wordsPath = value; }
        }

        /**
         * In case you would want to add a word to the wordlist (or a lot of words)
         */
        public static void WordListBuild(string line)
        {
            try
            {
                using (var sr = new StreamWriter(wordsPath, true))
                {
                    sr.WriteLine(line);
                }
            }
            catch (FileNotFoundException)
            {
                System.Console.WriteLine("File not found, will have to create");
            }
            catch (DirectoryNotFoundException)
            {
                createDirectory();
            }
        }

        public static void WordListDispose()
        {
            try
            {
                File.Delete(wordsPath);
            }
            catch (FileNotFoundException)
            {
                //This is very probable but not a problem.
            }
        }

        public static bool WordsListContains(string word)
        {
            bool result = false;
            try
            {

                if (HttpHandler.WordsListVersionCheck(getVersionNumber()))
                    getNewWordsList();
                using (var sr = new StreamReader(wordsPath))
                {
                    string line;
                    while (((line = sr.ReadLine()) != null))
                    {
                        if ((line.ToLower().Trim() == word))
                        {
                            result = true; //If we get even anything as a reponse, we've gotten past the important parts.
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                System.Console.WriteLine("Wordlist not found @ " + wordsPath + "\n Attempt online acquire?");
                char response = System.Console.ReadKey().KeyChar;
                if (response == 'y')
                {
                    Console.WriteLine("");
                    HttpHandler.WordsListAcquire();
                }
            }
            catch (DirectoryNotFoundException)
            {
                createDirectory();
            }
            return result;
        }
        /**
         *  Optimised for using little memory
         */
        public static string GenerateRandomWord(int lettersNo)
        {
            string randomWord = null;
            bool result = false;
            int retries = 0;
            while ((result == false) && (retries < 3))
            {
                try
                {
                    if (HttpHandler.WordsListVersionCheck(getVersionNumber()))
                        getNewWordsList();
                    using (var sr = new StreamReader(wordsPath))
                    {
                        int count = 0;
                        var random = new Random();
                        string line;
                        while (((line = sr.ReadLine()) != null))
                        {
                            line = line.Trim();
                            if (line.Length > 0 && 
                                char.IsLower(line[0]) &&
                                (line.Length == lettersNo) &&
                                (char.IsLower(line[0])) &&
                                (random.Next(count++) == 0)) //Word limiting rules
                            {
                                randomWord = line;
                                result = true; //If we get even anything as a reponse, we've gotten past the important parts.
                            }
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    getNewWordsList();
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    createDirectory();
                }

                if (randomWord == null)
                {
                    System.Console.WriteLine("No word generated.");
                    //Create decent catch?
                }
                retries++;
            }
            return randomWord;
        }

        private static void createDirectory()
        {
            System.Console.WriteLine("Invading your AppData @ " + directoryPath);
            Directory.CreateDirectory(directoryPath);
        }

        private static void getNewWordsList()
        {
            System.Console.WriteLine("Wordlist update required @ " + wordsPath + "\n Attempt online acquire? y/n");
            char response = System.Console.ReadKey().KeyChar;
            if (response == 'y')
            {
                Console.WriteLine("");
                HttpHandler.WordsListAcquire();
            }
        }

        private static int getVersionNumber()
        {
            int versionNumber = 0;
            using (var sr = new StreamReader(wordsPath))
            {
                string line = sr.ReadLine();
                if(line.StartsWith("v="))
                {
                    versionNumber = int.Parse(line.Substring(2));
                }
            }
            return versionNumber;
        }
    }
}
