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
    public partial class browser : Form
    {
        public browser(string URL)
        {
            InitializeComponent();
            webBrowser1.Navigate(new Uri(URL));
            
        }
    }
}