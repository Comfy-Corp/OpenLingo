using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingoLib
{
    public enum LingoProtocol
    {
        PING = 255,             //Parameter: string
        CONNECTION_END = 254,   //Parameter: none
        REGISTER_LOBBY = 253,   //Parameter: PlayerInfo
        UNREGISTER_LOBBY = 252, //Parameter: PlayerInfo 
        REQUEST_MATCHUP = 251,  //Parameter: PlayerInfo
        ACCEPT_MATCHUP = 250,   //Parameter: PlayerInfo
        REQUEST_LOBBY = 249,    //Parameter: List<PlayerInfo>
        NOK = 1,                //Parameter: string
        OK = 0                  //Parameter: string
    }
}
