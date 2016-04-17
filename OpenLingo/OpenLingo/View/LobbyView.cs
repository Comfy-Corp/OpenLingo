using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LingoLib;
using OpenLingoClient.Control.Net;

namespace OpenLingoClient.View
{
    public partial class LobbyView : Form
    {
        List<PlayerInfo> DisplayNames;

        public LobbyView()
        {
            InitializeComponent();
            this.ConnectedLabel.ForeColor = (ServerNet.Client.IsConnected) ? Color.Green : Color.Red;
            RefreshLobby();
        }

        public void RefreshLobby()
        {
            if (!OpenLingoClient.Control.Net.ServerNet.Client.TryGetLobbyPlayers(out DisplayNames))
                return;
            string display = "";
            DisplayNames.ForEach(x => display+=x.Username+"\n"); //build string
            playersBox.Text = display;
        }

        private void ConnectedLabel_Click(object sender, EventArgs e)
        {
            ServerNet.Client.RetryConnect();
            this.ConnectedLabel.ForeColor = ServerNet.Client.IsConnected ? Color.Green : Color.Red;
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshLobby();
        }
    }
}
