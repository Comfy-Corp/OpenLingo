using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LingoLib;

namespace OpenLingoServer
{
    public class Lobby
    {
        public static List<PlayerInfo> Players = new List<PlayerInfo>();

        public static List<string> GetPlayerNames()
        {
            List<string> usernames = new List<string>();
            Players.ForEach(player => usernames.Add(player.Username));
            return usernames;
        }
    }
}
