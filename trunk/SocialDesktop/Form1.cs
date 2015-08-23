using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using P2PFunctions;
using IDWIM;
using OpenNETCF.Net.NetworkInformation;
namespace SocialDesktop
{
    public partial class Form1 : Form
    {
        [DllImport("coredll.dll", CharSet = CharSet.Auto)]
        public static extern int FindWindow(string name, string extval);
        [DllImport("coredll.dll", CharSet = CharSet.Auto)]
        public static extern bool ShowWindow(int hwnd, int nCmdShow);
        [DllImport("coredll.dll", CharSet = CharSet.Auto)]
        public static extern bool EnableWindow(int hwnd, bool enabled);
        const string installpath = "\\Flash2\\Apps";
        public Form1()
        {
            
            int h = FindWindow("HHTaskBar", "");
            ShowWindow(h, 0);
            EnableWindow(h, false);
            
            InitializeComponent();
            
            FormBorderStyle = FormBorderStyle.None;
            textBox1.PasswordChar = "*"[0];
            try
            {
               // pictureBox1.Image = new System.Drawing.Bitmap(installpath + "\\clouds.jpg");
            }
            catch (OutOfMemoryException)
            {
                //Runs out of memory for some strange reason...
            }
                this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
           
        }
        bool isrunning = true;
        private void closeDesktop(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.Image.Dispose();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
            isrunning = false;
            //Unmount the filesystem and exit
            try
            {
                
                FileSystem.Commit();
            }
            catch (Exception er)
            {
            
            }
            int h = FindWindow("HHTaskBar", "");
            ShowWindow(h, 5);
            EnableWindow(h, true);
            Close();
        }

        private void doLogin(object sender, EventArgs e)
        {
            try
            {
                cryptFS.doLogin(textBox1.Text);
                MessageBox.Show("Login succeeded!");
                int h = FindWindow("HHTaskBar", "");
                ShowWindow(h, 5);
                EnableWindow(h, true);
                BackgroundAgent myagent = new BackgroundAgent();

                Close();
                
            }
            catch (Exception er)
            {
                MessageBox.Show("An error occured while logging in."+er.Message);
            }
        }

        private void netConfigClick(object sender, EventArgs e)
        {
            netConfigDlg mydlg = new netConfigDlg();
            mydlg.ShowDialog();
        }

        private void showConInfo(object sender, EventArgs e)
        {
            connInfo myfo = new connInfo();
            myfo.ShowDialog();
        }

        private void showCreateDlg(object sender, EventArgs e)
        {
            createNetwork mynet = new createNetwork();
            mynet.ShowDialog();
            mynet.Dispose();
        }
        bool ischecked = true;
        void thetar()
        {
            while (ischecked)
            {
                try
                {
                    if (((WirelessZeroConfigNetworkInterface)WirelessZeroConfigNetworkInterface.GetAllNetworkInterfaces()[0]).InterfaceOperationalStatus == InterfaceOperationalStatus.NonOperational)
                    {
                        AccessPointCollection mylection = ((WirelessZeroConfigNetworkInterface)WirelessZeroConfigNetworkInterface.GetAllNetworkInterfaces()[0]).NearbyAccessPoints;
                        foreach (AccessPoint e in mylection)
                        {
                            if (e.Privacy == 0)
                            {
                                if (e.Name != ((WirelessZeroConfigNetworkInterface)WirelessZeroConfigNetworkInterface.GetAllNetworkInterfaces()[0]).AssociatedAccessPoint & e.Name !=null)
                                {
                                    bool infra = true;
                                    if (e.InfrastructureMode == InfrastructureMode.Infrastructure)
                                    {
                                        infra = true;
                                    }
                                    ((WirelessZeroConfigNetworkInterface)WirelessZeroConfigNetworkInterface.GetAllNetworkInterfaces()[0]).AddPreferredNetwork(e.Name, infra, new byte[5], 1, AuthenticationMode.Open, WEPStatus.WEPDisabled, new EAPParameters());
                                    ((WirelessZeroConfigNetworkInterface)WirelessZeroConfigNetworkInterface.GetAllNetworkInterfaces()[0]).ConnectToPreferredNetwork(e.Name);
                                }
                            }
                        }
                        System.Threading.Thread.Sleep(5000);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(300);
                    }

                }
                catch (Exception)
                {
                
                }
            }
        }
        private void doStateChange(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                ischecked = true;
                System.Threading.Thread mthread = new System.Threading.Thread(thetar);
                mthread.Start();
            }
            else
            {
                ischecked = false;
            }
        }
    }
}