using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingoLib
{
    /// <summary>
    /// The possible packages that are sent between the client and server
    /// 
    /// TODO; This might have benefit from being split into Server Protocol and Client Protocol
    /// DON'T FIX WHAT ISN'T BROKEN STUPID AAAAAH
    /// </summary>
    public enum LingoProtocol
    {
        /* Lobby packages */    //             Server             Client        Description
        REGISTER_LOBBY,         //Parameter:      -             PlayerInfo      Used by the client to make server aware of presence
        UNREGISTER_LOBBY,       //Parameter:      -             PlayerInfo      Used by client to make a clean shutdown, stops zombies from appearing in the lobby
        REQUEST_MATCHUP,        //Parameter: PlayerInfo         PlayerInfo      Used by the client to make intent clear, tells the server what player was selected
                                //                                              Also used by server to tell a client it has been selected for a match by another player
        ACCEPT_MATCHUP,         //Parameter: PlayerInfo         PlayerInfo      Reverse of Request_matchup
        GET_LOBBY_PLAYERS,      //Parameter: List<PlayerInfo>   null            Allows a client to access the lobby
        REQUEST_TEAMUP,         //Parameter:      -             PlayerInfo      Nice-to-have feature, in which 2 vs 2 matches are possible
        START_GAME,             //Parameter: Tuple<PlayerInfo>      -           Extra confirmation that both parties are going to start playing
        ALREADY_REGISTERED,     //Parameter: List<PlayerInfo>       -           Registration error if the player is already registered on the lobby
        /* Game packages */
        TRANSFER_CARDS,         //Parameter: LingoCard          null            Tell the clients what the Lingo cards look like, and which are already 
        PROMPT_GUESS,           //Parameter: MatchSession       string          Server asks the client to send a guess. Reply with the guess.

        /* Common packages */
        PING,                   //Parameter: string             string          Check alive, check connectivity
        CONNECTION_END,         //Parameter: none               PlayerInfo      Gracefully end connection. Close sockets/streams
        NOK,                    //Parameter: string             string          Problem detected, comes with description
        OK                      //Parameter: string             string          Everything is fine.
    }
}
