namespace SocialDesktop
{
    partial class connInfo
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.SSID = new System.Windows.Forms.Label();
            this.security = new System.Windows.Forms.Label();
            this.signalStrength = new System.Windows.Forms.Label();
            this.connectionState = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ipAddress = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SSID
            // 
            this.SSID.Location = new System.Drawing.Point(118, 0);
            this.SSID.Name = "SSID";
            this.SSID.Size = new System.Drawing.Size(100, 20);
            this.SSID.Text = "label1";
            // 
            // security
            // 
            this.security.Location = new System.Drawing.Point(118, 41);
            this.security.Name = "security";
            this.security.Size = new System.Drawing.Size(100, 20);
            this.security.Text = "label1";
            // 
            // signalStrength
            // 
            this.signalStrength.Location = new System.Drawing.Point(118, 81);
            this.signalStrength.Name = "signalStrength";
            this.signalStrength.Size = new System.Drawing.Size(100, 20);
            this.signalStrength.Text = "label1";
            // 
            // connectionState
            // 
            this.connectionState.Location = new System.Drawing.Point(118, 129);
            this.connectionState.Name = "connectionState";
            this.connectionState.Size = new System.Drawing.Size(100, 20);
            this.connectionState.Text = "label1";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 21);
            this.label1.Text = "SSID";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 21);
            this.label2.Text = "Security";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 20);
            this.label3.Text = "Signal strength";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(4, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 30);
            this.label4.Text = "Connectioin state";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 20);
            this.label5.Text = "IP address";
            // 
            // ipAddress
            // 
            this.ipAddress.Location = new System.Drawing.Point(102, 182);
            this.ipAddress.Name = "ipAddress";
            this.ipAddress.Size = new System.Drawing.Size(162, 20);
            this.ipAddress.Text = "label6";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(4, 206);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(260, 40);
            this.label6.Text = "Created by IDWNet Cloud Computing";
            // 
            // connInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(270, 246);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ipAddress);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connectionState);
            this.Controls.Add(this.signalStrength);
            this.Controls.Add(this.security);
            this.Controls.Add(this.SSID);
            this.Name = "connInfo";
            this.Text = "connInfo";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label SSID;
        private System.Windows.Forms.Label security;
        private System.Windows.Forms.Label signalStrength;
        private System.Windows.Forms.Label connectionState;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label ipAddress;
        private System.Windows.Forms.Label label6;
    }
}