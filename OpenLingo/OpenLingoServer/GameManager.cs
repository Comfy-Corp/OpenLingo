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
        public Dictionary<List<PlayerInfo>, LingoCard> Teams;
        public int WordLength;


        public GameManager(List<PlayerInfo> players)
        {
            // 1v1 procedure
            if(players[0].TeamMate == null)
            {
                bool isEven = (new Random().Next(0, 2) == 1) ? true : false; 
                List<PlayerInfo> lonelyMan = new List<PlayerInfo>();
                lonelyMan.Add(players[0]);
                this.Teams.Add(lonelyMan, new LingoCard(isEven, LingoCard.generateLingoCardMask()));
                List<PlayerInfo> otherMan = new List<PlayerInfo>();
                otherMan.Add(players[1]);
                this.Teams.Add(otherMan, new LingoCard(!isEven, LingoCard.generateLingoCardMask()));
            }
            else // 2v2 procedure
            {
                throw new NotImplementedException();
            }
            HostMatch();
        }

        public void HostMatch()
        {

        }

    }
}
