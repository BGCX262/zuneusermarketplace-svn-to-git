namespace SocialDesktop
{
    partial class netConfigDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ssid = new System.Windows.Forms.ColumnHeader();
            this.strength = new System.Windows.Forms.ColumnHeader();
            this.security = new System.Windows.Forms.ColumnHeader();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.Add(this.ssid);
            this.listView1.Columns.Add(this.strength);
            this.listView1.Columns.Add(this.security);
            listViewItem1.Text = "Sample";
            listViewItem1.SubItems.Add("100");
            listViewItem1.SubItems.Add("None");
            this.listView1.Items.Add(listViewItem1);
            this.listView1.Location = new System.Drawing.Point(42, 29);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(236, 136);
            this.listView1.TabIndex = 0;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.onChanged);
            // 
            // ssid
            // 
            this.ssid.Text = "SSID";
            this.ssid.Width = 60;
            // 
            // strength
            // 
            this.strength.Text = "Signal strength";
            this.strength.Width = 60;
            // 
            // security
            // 
            this.security.Text = "Security";
            this.security.Width = 60;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(42, 191);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 20);
            this.button1.TabIndex = 1;
            this.button1.Text = "Connect";
            this.button1.Click += new System.EventHandler(this.doClickyThing);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(190, 191);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 20);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.Click += new System.EventHandler(this.closeDlg);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(42, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(236, 23);
            this.label1.Text = "Created by IDWNet Cloud Computing";
            // 
            // netConfigDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(306, 227);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Menu = this.mainMenu1;
            this.Name = "netConfigDlg";
            this.Text = "Wireless network configuration";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ssid;
        private System.Windows.Forms.ColumnHeader strength;
        private System.Windows.Forms.ColumnHeader security;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
    }
}