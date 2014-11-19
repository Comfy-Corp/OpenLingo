using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LingoLib;

namespace OpenLingoClient.Control
{
    //Small abstraction layer for config
    static class Config
    {
        private static ConfigParameters conf;

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
