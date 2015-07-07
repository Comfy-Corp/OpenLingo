using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LingoLib;

namespace OpenLingoClient.Control
{
    //Small abstraction layer for config
    public static class Config
    {
        private static ConfigParameters conf;
        public static bool IsDebug = false; //Changes IO method

        public static LANGUAGE Language
        {
            get { return conf.Language; }
            set { 
                 conf.Language = value;
                 SaveConfig();
                }
        }
        public static int WordsListVersion
        {
            get { return conf.WordsListVersion; }
            set { 
                 conf.WordsListVersion = value;
                 SaveConfig();
                }
        }

        public static int GuessAttempts //Word length may not be 0
        {
            get
            {
                if (conf.GuessAttempts == 0)
                    GuessAttempts = 1;
                return conf.GuessAttempts;
            }
            set
            {
                if (value == 0)
                    conf.GuessAttempts = 1;
                else
                    conf.GuessAttempts = value;
                SaveConfig();
            }
        }

        public static int WordLength //Word length may not be below 1
        {
            get {
                if (conf.WordLength <= 0)
                    WordLength = 1;
                return conf.WordLength;
            }
            set
            {
                if (value <= 0)
                    conf.WordLength = 1;
                else
                    conf.WordLength = value;
                SaveConfig();
            }
        }

        public static Player LocalPlayer
        {
            get {
                if (conf.LocalPlayer == null)
                    LocalPlayer = new Player("Default");
                return conf.LocalPlayer; 
            }
            set
            {
                conf.LocalPlayer = value;
                SaveConfig();
            }
        }

        static Config()
        {
            LoadConfig();
        } 

        static public void LoadConfig()
        {
            conf = FileManager.ConfigLoad();
            if (conf == null)
                conf = new ConfigParameters();
        }

        static public void SaveConfig()
        {
            FileManager.ConfigSave(conf);
        }

    }
}
