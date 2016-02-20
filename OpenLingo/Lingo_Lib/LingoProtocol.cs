using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingoLib
{
    public enum LingoProtocol
    {
        /* Lobby packages */    //             Server             Client
        REGISTER_LOBBY,         //Parameter:      -             PlayerInfo
        UNREGISTER_LOBBY,       //Parameter:      -             PlayerInfo 
        REQUEST_MATCHUP,        //Parameter: PlayerInfo         PlayerInfo
        ACCEPT_MATCHUP,         //Parameter: PlayerInfo         PlayerInfo
        GET_LOBBY_PLAYERS,      //Parameter: List<PlayerInfo>   null
        REQUEST_TEAMUP,         //Parameter:      -             PlayerInfo
        START_GAME,             //Parameter: List<PlayerInfo>   
        /* Game packages */
        TRANSFER_CARD,          //Parameter: LingoCard          null
        PROMPT_GUESS,           //Parameter: MatchSession       string

        /* Common packages */
        PING,                   //Parameter: string             string
        CONNECTION_END,         //Parameter: none               PlayerInfo
        NOK,                    //Parameter: string             string
        OK                      //Parameter: string             string
    }
}
