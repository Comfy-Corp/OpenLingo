﻿using System;
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
    //I have come to vastly prefer MVVM over MVC over the years.
    //But I already locked myself by choosing this when I first started.
    public partial class LobbyView : Form
    {
        List<PlayerInfo> DisplayNames;

        public LobbyView()
        {
            InitializeComponent();
            this.ConnectedLabel.ForeColor = (ServerNet.Client.Init()) ? Color.Green : Color.Red;
            RefreshLobbyList();
        }

        public void RefreshLobbyList()
        {
            if ((DisplayNames = ServerNet.Client.ConnectToLobby()) == null)
                return;
            string display = "";
            DisplayNames.ForEach(x => { display += x.Username + "\n"; Console.WriteLine("User: " + x.Username); } ); //build string
            playersBox.Text = display;
        }

        private void ConnectedLabel_Click(object sender, EventArgs e)
        {
            this.ConnectedLabel.ForeColor = (ServerNet.Client.Init()) ? Color.Green : Color.Red;
        }

        private void RefreshButton_Click(object sender, EventArgs e) => 
            RefreshLobbyList();
    }
}
