using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Net.NetworkInformation;
namespace SocialDesktop
{
    public partial class createNetwork : Form
    {
        public createNetwork()
        {
            InitializeComponent();
        }

        private void doCreateNetwork(object sender, EventArgs e)
        {
            WirelessZeroConfigNetworkInterface myface = (WirelessZeroConfigNetworkInterface)WirelessZeroConfigNetworkInterface.GetAllNetworkInterfaces()[0];
            myface.AddPreferredNetwork(textBox1.Text, false, new byte[5], 1, AuthenticationMode.Open, WEPStatus.WEPDisabled, new EAPParameters());
            myface.ConnectToPreferredNetwork(textBox1.Text);
            MessageBox.Show("Network creation successful");
            Close();
        }
    }
}