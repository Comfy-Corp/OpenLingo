using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LingoLib;

namespace The_World_Ends_With_Lingo.Control
{
    class FileReader
    {
        private static string directoryPath =  Environment.SpecialFolder.ApplicationData.ToString() + "/Lingo";
        private static string configPath = directoryPath+"/your_lingo_cofig.conf";
        private static string wordsPath =  directoryPath+"/wordsEn.txt";

        public static void SaveConfig(ConfigParameters config)
        {
            try
            {
                using (var sr = new StreamWriter(configPath))
                {
                    Directory.CreateDirectory(configPath);
                }
            } 
            catch(IOException)
            {
                System.Console.WriteLine("Panic");
            }
        }

        /**
         * In case I would want to add a word to the wordlist (or a lot of words)
         */
        public static void BuildWordList(string line)
        {
            try
            {
                using (var sr = new StreamWriter(wordsPath,true))
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
                System.Console.WriteLine("Invading your AppData @ " + directoryPath);
                Directory.CreateDirectory(directoryPath);
            }
        }

        /**
         *  Optimised for using little memory
         */
        public static string GenerateRandomWord(int lettersNo)
        {
            string randomWord = null;
            bool result = false;
            int retries = 0;
            while((result == false) && (retries < 3))
            {
                try
                {
                    using (var sr = new StreamReader(wordsPath))
                    {
                        int count = 0;
                        var random = new Random();
                        string line;
                        while (((line = sr.ReadLine()) != null))
                        {
                            if ((line.Length == lettersNo) && (random.Next(count++) == 0))
                            {
                                randomWord = line;
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
                        HttpHandler.AcquireWordsList();
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    System.Console.WriteLine("Invading your AppData @ " + directoryPath);
                    Directory.CreateDirectory(directoryPath);
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
    } 
}
