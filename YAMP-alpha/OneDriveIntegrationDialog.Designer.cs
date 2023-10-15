namespace YAMP_alpha
{
    partial class OneDriveIntegrationDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AuthenticationBrowser = new System.Windows.Forms.WebBrowser();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.AccessTokenValidLabel = new System.Windows.Forms.Label();
            this.AccessTokenValidTextBox = new System.Windows.Forms.TextBox();
            this.RefreshTokenLabel = new System.Windows.Forms.Label();
            this.RefreshTokenTextBox = new System.Windows.Forms.TextBox();
            this.AuthorizationCodeLabel = new System.Windows.Forms.Label();
            this.AuthorizationCodeTextBox = new System.Windows.Forms.TextBox();
            this.CurrentUrlLabel = new System.Windows.Forms.Label();
            this.CurrentUrlTextBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AccessTokenValidLabel);
            this.panel1.Controls.Add(this.AccessTokenValidTextBox);
            this.panel1.Controls.Add(this.RefreshTokenLabel);
            this.panel1.Controls.Add(this.RefreshTokenTextBox);
            this.panel1.Controls.Add(this.AuthorizationCodeLabel);
            this.panel1.Controls.Add(this.AuthorizationCodeTextBox);
            this.panel1.Controls.Add(this.CurrentUrlLabel);
            this.panel1.Controls.Add(this.CurrentUrlTextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(489, 222);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 541);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(489, 55);
            this.panel2.TabIndex = 1;
            // 
            // AuthenticationBrowser
            // 
            this.AuthenticationBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AuthenticationBrowser.Location = new System.Drawing.Point(0, 222);
            this.AuthenticationBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.AuthenticationBrowser.Name = "AuthenticationBrowser";
            this.AuthenticationBrowser.Size = new System.Drawing.Size(489, 319);
            this.AuthenticationBrowser.TabIndex = 2;
            this.AuthenticationBrowser.Url = new System.Uri("", System.UriKind.Relative);
            this.AuthenticationBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.AuthenticationBrowser_Navigated);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(275, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(374, 17);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei Light", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "OneDrive";
            // 
            // AccessTokenValidLabel
            // 
            this.AccessTokenValidLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AccessTokenValidLabel.AutoSize = true;
            this.AccessTokenValidLabel.Location = new System.Drawing.Point(11, 172);
            this.AccessTokenValidLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.AccessTokenValidLabel.Name = "AccessTokenValidLabel";
            this.AccessTokenValidLabel.Size = new System.Drawing.Size(118, 13);
            this.AccessTokenValidLabel.TabIndex = 24;
            this.AccessTokenValidLabel.Text = "Access Token Valid Till";
            // 
            // AccessTokenValidTextBox
            // 
            this.AccessTokenValidTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccessTokenValidTextBox.Location = new System.Drawing.Point(10, 188);
            this.AccessTokenValidTextBox.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.AccessTokenValidTextBox.Name = "AccessTokenValidTextBox";
            this.AccessTokenValidTextBox.ReadOnly = true;
            this.AccessTokenValidTextBox.Size = new System.Drawing.Size(467, 20);
            this.AccessTokenValidTextBox.TabIndex = 23;
            // 
            // RefreshTokenLabel
            // 
            this.RefreshTokenLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RefreshTokenLabel.AutoSize = true;
            this.RefreshTokenLabel.Location = new System.Drawing.Point(12, 137);
            this.RefreshTokenLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.RefreshTokenLabel.Name = "RefreshTokenLabel";
            this.RefreshTokenLabel.Size = new System.Drawing.Size(78, 13);
            this.RefreshTokenLabel.TabIndex = 22;
            this.RefreshTokenLabel.Text = "Refresh Token";
            // 
            // RefreshTokenTextBox
            // 
            this.RefreshTokenTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshTokenTextBox.Location = new System.Drawing.Point(11, 152);
            this.RefreshTokenTextBox.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.RefreshTokenTextBox.Name = "RefreshTokenTextBox";
            this.RefreshTokenTextBox.ReadOnly = true;
            this.RefreshTokenTextBox.Size = new System.Drawing.Size(467, 20);
            this.RefreshTokenTextBox.TabIndex = 21;
            // 
            // AuthorizationCodeLabel
            // 
            this.AuthorizationCodeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AuthorizationCodeLabel.AutoSize = true;
            this.AuthorizationCodeLabel.Location = new System.Drawing.Point(11, 100);
            this.AuthorizationCodeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.AuthorizationCodeLabel.Name = "AuthorizationCodeLabel";
            this.AuthorizationCodeLabel.Size = new System.Drawing.Size(96, 13);
            this.AuthorizationCodeLabel.TabIndex = 20;
            this.AuthorizationCodeLabel.Text = "Authorization Code";
            // 
            // AuthorizationCodeTextBox
            // 
            this.AuthorizationCodeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AuthorizationCodeTextBox.Location = new System.Drawing.Point(10, 115);
            this.AuthorizationCodeTextBox.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.AuthorizationCodeTextBox.Name = "AuthorizationCodeTextBox";
            this.AuthorizationCodeTextBox.ReadOnly = true;
            this.AuthorizationCodeTextBox.Size = new System.Drawing.Size(467, 20);
            this.AuthorizationCodeTextBox.TabIndex = 19;
            // 
            // CurrentUrlLabel
            // 
            this.CurrentUrlLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CurrentUrlLabel.AutoSize = true;
            this.CurrentUrlLabel.Location = new System.Drawing.Point(12, 58);
            this.CurrentUrlLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CurrentUrlLabel.Name = "CurrentUrlLabel";
            this.CurrentUrlLabel.Size = new System.Drawing.Size(66, 13);
            this.CurrentUrlLabel.TabIndex = 18;
            this.CurrentUrlLabel.Text = "Current URL";
            // 
            // CurrentUrlTextBox
            // 
            this.CurrentUrlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentUrlTextBox.Location = new System.Drawing.Point(11, 74);
            this.CurrentUrlTextBox.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.CurrentUrlTextBox.Name = "CurrentUrlTextBox";
            this.CurrentUrlTextBox.ReadOnly = true;
            this.CurrentUrlTextBox.Size = new System.Drawing.Size(467, 20);
            this.CurrentUrlTextBox.TabIndex = 17;
            // 
            // OneDriveIntegrationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 596);
            this.Controls.Add(this.AuthenticationBrowser);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "OneDriveIntegrationDialog";
            this.Text = "OneDriveIntegrationDialog";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.WebBrowser AuthenticationBrowser;
        private System.Windows.Forms.Label AccessTokenValidLabel;
        private System.Windows.Forms.TextBox AccessTokenValidTextBox;
        private System.Windows.Forms.Label RefreshTokenLabel;
        private System.Windows.Forms.TextBox RefreshTokenTextBox;
        private System.Windows.Forms.Label AuthorizationCodeLabel;
        private System.Windows.Forms.TextBox AuthorizationCodeTextBox;
        private System.Windows.Forms.Label CurrentUrlLabel;
        private System.Windows.Forms.TextBox CurrentUrlTextBox;
    }
}