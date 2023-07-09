namespace YAMP_alpha
{
    partial class EQBandConfigDialog
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bandLabel = new System.Windows.Forms.Label();
            this.RightBandWidth = new YAMP_alpha.Controls.EQBand();
            this.rightGainDb = new YAMP_alpha.Controls.EQBand();
            this.LeftBandWidth = new YAMP_alpha.Controls.EQBand();
            this.leftGainDb = new YAMP_alpha.Controls.EQBand();
            this.avgGain = new YAMP_alpha.Controls.EQBand();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(109, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(394, 243);
            this.panel1.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox2.Controls.Add(this.RightBandWidth);
            this.groupBox2.Controls.Add(this.rightGainDb);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(195, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(195, 239);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Right Channel";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Controls.Add(this.LeftBandWidth);
            this.groupBox1.Controls.Add(this.leftGainDb);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(195, 239);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Left Channel";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.bandLabel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(503, 37);
            this.panel2.TabIndex = 4;
            // 
            // bandLabel
            // 
            this.bandLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.bandLabel.Font = new System.Drawing.Font("Century Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandLabel.Location = new System.Drawing.Point(0, 0);
            this.bandLabel.Name = "bandLabel";
            this.bandLabel.Size = new System.Drawing.Size(441, 35);
            this.bandLabel.TabIndex = 0;
            this.bandLabel.Text = "Band Impact";
            this.bandLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RightBandWidth
            // 
            this.RightBandWidth.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.RightBandWidth.BandMax = 40;
            this.RightBandWidth.BandMin = 1;
            this.RightBandWidth.BandValue = 1;
            this.RightBandWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightBandWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RightBandWidth.FooterBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.RightBandWidth.FooterForeColor = System.Drawing.SystemColors.ControlText;
            this.RightBandWidth.FooterText = "";
            this.RightBandWidth.FooterVisible = true;
            this.RightBandWidth.Location = new System.Drawing.Point(97, 19);
            this.RightBandWidth.Margin = new System.Windows.Forms.Padding(4);
            this.RightBandWidth.Name = "RightBandWidth";
            this.RightBandWidth.ShowBandValueInFooter = true;
            this.RightBandWidth.Size = new System.Drawing.Size(95, 217);
            this.RightBandWidth.TabIndex = 2;
            this.RightBandWidth.Text = "BandWidth";
            this.RightBandWidth.TickFrequency = 5;
            this.RightBandWidth.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.RightBandWidth.ValueChanged += new System.EventHandler(this.RightBandWidth_ValueChanged);
            // 
            // rightGainDb
            // 
            this.rightGainDb.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.rightGainDb.BandMax = 50;
            this.rightGainDb.BandMin = -50;
            this.rightGainDb.BandValue = 0;
            this.rightGainDb.Dock = System.Windows.Forms.DockStyle.Left;
            this.rightGainDb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rightGainDb.FooterBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.rightGainDb.FooterForeColor = System.Drawing.SystemColors.ControlText;
            this.rightGainDb.FooterText = "";
            this.rightGainDb.FooterVisible = true;
            this.rightGainDb.Location = new System.Drawing.Point(3, 19);
            this.rightGainDb.Margin = new System.Windows.Forms.Padding(4);
            this.rightGainDb.Name = "rightGainDb";
            this.rightGainDb.ShowBandValueInFooter = true;
            this.rightGainDb.Size = new System.Drawing.Size(94, 217);
            this.rightGainDb.TabIndex = 2;
            this.rightGainDb.Text = "Gain";
            this.rightGainDb.TickFrequency = 5;
            this.rightGainDb.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.rightGainDb.ValueChanged += new System.EventHandler(this.rightGainDb_ValueChanged);
            // 
            // LeftBandWidth
            // 
            this.LeftBandWidth.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.LeftBandWidth.BandMax = 40;
            this.LeftBandWidth.BandMin = 1;
            this.LeftBandWidth.BandValue = 1;
            this.LeftBandWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftBandWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeftBandWidth.FooterBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.LeftBandWidth.FooterForeColor = System.Drawing.SystemColors.ControlText;
            this.LeftBandWidth.FooterText = "";
            this.LeftBandWidth.FooterVisible = true;
            this.LeftBandWidth.Location = new System.Drawing.Point(97, 19);
            this.LeftBandWidth.Margin = new System.Windows.Forms.Padding(4);
            this.LeftBandWidth.Name = "LeftBandWidth";
            this.LeftBandWidth.ShowBandValueInFooter = true;
            this.LeftBandWidth.Size = new System.Drawing.Size(95, 217);
            this.LeftBandWidth.TabIndex = 2;
            this.LeftBandWidth.Text = "BandWidth";
            this.LeftBandWidth.TickFrequency = 5;
            this.LeftBandWidth.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.LeftBandWidth.ValueChanged += new System.EventHandler(this.LeftBandWidth_ValueChanged);
            // 
            // leftGainDb
            // 
            this.leftGainDb.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.leftGainDb.BandMax = 50;
            this.leftGainDb.BandMin = -50;
            this.leftGainDb.BandValue = 0;
            this.leftGainDb.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftGainDb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.leftGainDb.FooterBackColor = System.Drawing.SystemColors.ControlLightLight;
            this.leftGainDb.FooterForeColor = System.Drawing.SystemColors.ControlText;
            this.leftGainDb.FooterText = "";
            this.leftGainDb.FooterVisible = true;
            this.leftGainDb.Location = new System.Drawing.Point(3, 19);
            this.leftGainDb.Margin = new System.Windows.Forms.Padding(4);
            this.leftGainDb.Name = "leftGainDb";
            this.leftGainDb.ShowBandValueInFooter = false;
            this.leftGainDb.Size = new System.Drawing.Size(94, 217);
            this.leftGainDb.TabIndex = 1;
            this.leftGainDb.Text = "Gain";
            this.leftGainDb.TickFrequency = 5;
            this.leftGainDb.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.leftGainDb.ValueChanged += new System.EventHandler(this.leftGainDb_ValueChanged);
            // 
            // avgGain
            // 
            this.avgGain.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.avgGain.BandMax = 50;
            this.avgGain.BandMin = -50;
            this.avgGain.BandValue = 0;
            this.avgGain.Dock = System.Windows.Forms.DockStyle.Left;
            this.avgGain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.avgGain.FooterBackColor = System.Drawing.SystemColors.Control;
            this.avgGain.FooterForeColor = System.Drawing.SystemColors.ControlText;
            this.avgGain.FooterText = "TEXT";
            this.avgGain.FooterVisible = true;
            this.avgGain.Location = new System.Drawing.Point(0, 37);
            this.avgGain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.avgGain.Name = "avgGain";
            this.avgGain.ShowBandValueInFooter = true;
            this.avgGain.Size = new System.Drawing.Size(109, 243);
            this.avgGain.TabIndex = 0;
            this.avgGain.Text = "Average Gain";
            this.avgGain.TickFrequency = 5;
            this.avgGain.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.avgGain.ValueChanged += new System.EventHandler(this.avgGain_ValueChanged);
            // 
            // EQBandConfigDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 280);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.avgGain);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EQBandConfigDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EQBandConfigDialog";
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.EQBand avgGain;
        private Controls.EQBand leftGainDb;
        private Controls.EQBand rightGainDb;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Controls.EQBand RightBandWidth;
        private System.Windows.Forms.GroupBox groupBox1;
        private Controls.EQBand LeftBandWidth;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label bandLabel;
    }
}