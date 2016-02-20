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

        private static void createDirectory()
        {
            System.Console.WriteLine("Invading your AppData @ " + directoryPath);
            Directory.CreateDirectory(directoryPath);
        }

        public static void ConfigSave(ConfigParameters config)
        {
            try
            {
                using (var stream = File.Open(configPath, FileMode.Create))
                {
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(stream, config);
                }
            }
            catch (DirectoryNotFoundException)
            {
                createDirectory();
            }
        }

        public static ConfigParameters ConfigLoad()
        {
            ConfigParameters config = null;
            try
            {
                using (var stream = File.Open(configPath, FileMode.Open))
                {
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    try
                    {
                        config = (ConfigParameters)bFormatter.Deserialize(stream);
                    }
                    catch (SerializationException) //This must be a new client verion, reset config
                    {
                        config = null;
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                createDirectory();
            }
            catch (FileNotFoundException)
            {
                config = null; //We'll assign a new one.
            }
            return config;
        }
    }
}
