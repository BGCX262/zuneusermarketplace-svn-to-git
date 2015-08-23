using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SocialDesktop
{
    public partial class pswdEntry : Form
    {
        public pswdEntry()
        {
            InitializeComponent();
        }
        public static string pswd = "12345678";
        private void doConnect(object sender, EventArgs e)
        {
            pswd = textBox1.Text;
            textBox1.Text = "";
            Close();
        }

        private void doPaste(object sender, EventArgs e)
        {
            string pswd = (string)Clipboard.GetDataObject().GetData(typeof(string));
            textBox1.Text = pswd;
        }
    }
}