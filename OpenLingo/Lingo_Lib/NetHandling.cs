using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingoLib
{
    [Serializable]
    public class Discover
    {
        public static int version = 0;
        public string user;
        public string gameName;
        public long myIP;
        public int port;
    }
}
