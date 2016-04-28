using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingoLib
{
    [Serializable]
    public class PlayerInfo
    {
        //I'm assuming there is no need for an ID
        //Because there will be so little players
        //That names *must* be unique
        public string Username;
        public PlayerInfo TeamMate; //Unimplemented

        public int score; //

        public PlayerState state;

        public PlayerInfo(string username)
        {
            this.Username = username;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            PlayerInfo objAsPlayerInfo = obj as PlayerInfo;
            if (objAsPlayerInfo == null) return false;
            else return (objAsPlayerInfo.Username == this.Username);
        }

        public override int GetHashCode()
        {
            return Username.GetHashCode(); //Usernames must be unique, so they can be used to distinguish PlayerInfo
        }
    }

    public enum PlayerState{
        IN_LOBBY,
        OPENING,
        TURN
    }
}
