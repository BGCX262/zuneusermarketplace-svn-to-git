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
    public partial class connInfo : Form
    {
        public connInfo()
        {
            InitializeComponent();
            myface = (WirelessZeroConfigNetworkInterface)WirelessZeroConfigNetworkInterface.GetAllNetworkInterfaces()[0];
            Timer mymer = new Timer();
            mymer.Tick += new EventHandler(mymer_Tick);
            mymer.Interval = 200;
            mymer.Enabled = true;
            
        }

        void mymer_Tick(object sender, EventArgs e)
        {
            try
            {
                SSID.Text = myface.AssociatedAccessPoint;
            }
            catch (Exception)
            {
                SSID.Text = "N/A";
            }
            try
            {
                security.Text = myface.AuthenticationMode.ToString() + "/" + myface.WEPStatus.ToString();
            }
            catch (Exception)
            {
                security.Text = "N/A";
            }
            try
            {
                signalStrength.Text = myface.SignalStrength.Decibels.ToString();
            }
            catch (Exception)
            {
                signalStrength.Text = "N/A";
            }
            try
            {
                connectionState.Text = myface.InterfaceOperationalStatus.ToString();
            }
            catch (Exception)
            {
                connectionState.Text = "N/A";
            }
            try
            {
                ipAddress.Text = myface.GetIPProperties().UnicastAddresses[0].Address.ToString();

            }
            catch (Exception)
            {
                ipAddress.Text = "N/A";
            }
            }
        WirelessZeroConfigNetworkInterface myface;
    }
}