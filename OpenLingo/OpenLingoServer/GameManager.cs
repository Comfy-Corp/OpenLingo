using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LingoLib;

namespace OpenLingoServer
{
    public class GameManager
    {
        public Tuple<PlayerInfo, PlayerInfo> Players;

        public GameManager(PlayerInfo PlayerA, PlayerInfo PlayerB)
        {
            Players = new Tuple<PlayerInfo, PlayerInfo>(PlayerA, PlayerB);

        }
    }
}
