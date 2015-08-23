using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Windows.Forms;
using System.IO;
using System.Drawing;
using IDWIM;
namespace SocialDesktop
{
    class BackgroundAgent
    {
        void thetar()
        {

            NotifyIcon mycon = new NotifyIcon();
            
            Stream mstream = File.Open("Flash2/apps/appslogo.ico",FileMode.Open);
            mycon.Icon = new System.Drawing.Icon(mstream);
            mycon.Visible = true;
            mycon.DoubleClick+=new EventHandler(mycon_DoubleClick);
            Application2.Run();
        }

        void mycon_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        void mycon_DoubleClick(object sender, EventArgs e)
        {
            browser myser = new browser("http://127.0.0.1:3299/");
            myser.ShowDialog();
        }
        void webtar()
        {
            appserver myserver = new appserver("/flash2/apps");
            
        }
        public BackgroundAgent()
        {
            System.Threading.Thread mthread = new System.Threading.Thread(thetar);
            
            mthread.Start();
            System.Threading.Thread webthread = new System.Threading.Thread(webtar);
            webthread.Start();
        }
    }
}
