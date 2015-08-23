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
    public partial class netConfigDlg : Form
    {
        public netConfigDlg()
        {
            InitializeComponent();
            button1.Enabled = false;
            Timer mymer = new Timer();
            listView1.Items.Clear();

            mylection = ((WirelessZeroConfigNetworkInterface)WirelessZeroConfigNetworkInterface.GetAllNetworkInterfaces()[0]).NearbyAccessPoints;
          
            foreach (AccessPoint et in mylection)
            {
                ListViewItem maintum = new ListViewItem(et.Name);
                maintum.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = et.SignalStrength.ToString() });
                maintum.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = et.Privacy.ToString() });
                listView1.Items.Add(maintum);
            }
            //TODO: Update list at an interval; without 'losing focus' on the currently selected value
            mymer.Tick += new EventHandler(mymer_Tick);
        }
        AccessPointCollection mylection;
        void mymer_Tick(object sender, EventArgs e)
        {
            mylection = ((WirelessZeroConfigNetworkInterface)WirelessZeroConfigNetworkInterface.GetAllNetworkInterfaces()[0]).NearbyAccessPoints;
            listView1.Items.Clear();
            foreach (AccessPoint et in mylection)
            {
                ListViewItem maintum = new ListViewItem(et.Name);
                maintum.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = et.SignalStrength.ToString() });
                maintum.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = et.Privacy.ToString() });
                listView1.Items.Add(maintum);
            }
        }

        private void closeDlg(object sender, EventArgs e)
        {
            Close();
        }
        void backgroundNetUpdate()
        {
            System.Threading.Thread.Sleep(9000);
            WirelessZeroConfigNetworkInterface myint = ((WirelessZeroConfigNetworkInterface)WirelessZeroConfigNetworkInterface.GetAllNetworkInterfaces()[0]);
            try
            {
                myint.Bind();
            }
            catch (Exception)
            {
            
            }
        }
        private void doClickyThing(object sender, EventArgs e)
        {
            WirelessZeroConfigNetworkInterface myint = ((WirelessZeroConfigNetworkInterface)WirelessZeroConfigNetworkInterface.GetAllNetworkInterfaces()[0]);
            bool isStructure = false;
            try
            {
                AccessPoint preferred = null;
                foreach (AccessPoint et in myint.PreferredAccessPoints)
                {
                    if (et.Name == mssid)
                    {
                        preferred = et;
                        break;
                    }
                }
                if (preferred == null)
                {
                    AccessPoint mypoint = mylection.FindBySSID(mssid);
                bool infra = false;
                    if(mypoint.InfrastructureMode == InfrastructureMode.Infrastructure) {
                infra = true;
                    }
                    //Check security
                    if (mypoint.Privacy == 1)
                    {
                        pswdEntry mytree = new pswdEntry();
                        mytree.ShowDialog();
                        string pswd = pswdEntry.pswd;
                        pswdEntry.pswd = "";
                      
                        myint.AddPreferredNetwork(mssid, infra, pswd, 1, AuthenticationMode.WPAPSK, WEPStatus.AESEnabled, new EAPParameters());
                  
                    }
                    if (mypoint.Privacy == 0)
                    {
                        myint.AddPreferredNetwork(mssid, infra, new byte[5], 1, AuthenticationMode.Open, WEPStatus.WEPDisabled, new EAPParameters());
                    }
                }
                System.Threading.Thread mthread = new System.Threading.Thread(backgroundNetUpdate);
                mthread.IsBackground = true;
                mthread.Start();
                myint.ConnectToPreferredNetwork(mssid);
                MessageBox.Show("Connection success!");
                Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        string mssid = "";
        private void onChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                mssid = listView1.Items[listView1.SelectedIndices[0]].Text;
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }
    }
}