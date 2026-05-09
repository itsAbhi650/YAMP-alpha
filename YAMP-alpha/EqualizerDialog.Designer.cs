namespace YAMP_alpha
{
    partial class EqualizerDialog
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
            this.components = new System.ComponentModel.Container();
            this.Spectrogram = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.recenterEQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spectrogramONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TbPg_Scope = new System.Windows.Forms.TabPage();
            this.EqCurve = new System.Windows.Forms.PictureBox();
            this.GainMeter = new YAMP_alpha.Controls.MeterControl();
            this.TbPg_Spectrogram = new System.Windows.Forms.TabPage();
            this.Pb_Spectrogram = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.TbPg_Scope.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EqCurve)).BeginInit();
            this.TbPg_Spectrogram.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Spectrogram)).BeginInit();
            this.SuspendLayout();
            // 
            // Spectrogram
            // 
            this.Spectrogram.Interval = 40;
            this.Spectrogram.Tick += new System.EventHandler(this.Spectrogram_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recenterEQToolStripMenuItem,
            this.spectrogramONToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(797, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // recenterEQToolStripMenuItem
            // 
            this.recenterEQToolStripMenuItem.Name = "recenterEQToolStripMenuItem";
            this.recenterEQToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.recenterEQToolStripMenuItem.Text = "Recenter EQ";
            this.recenterEQToolStripMenuItem.Click += new System.EventHandler(this.recenterEQToolStripMenuItem_Click);
            // 
            // spectrogramONToolStripMenuItem
            // 
            this.spectrogramONToolStripMenuItem.CheckOnClick = true;
            this.spectrogramONToolStripMenuItem.Name = "spectrogramONToolStripMenuItem";
            this.spectrogramONToolStripMenuItem.Size = new System.Drawing.Size(114, 20);
            this.spectrogramONToolStripMenuItem.Text = "Spectrogram: OFF";
            this.spectrogramONToolStripMenuItem.CheckedChanged += new System.EventHandler(this.spectrogramONToolStripMenuItem_CheckedChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(797, 524);
            this.splitContainer1.SplitterDistance = 286;
            this.splitContainer1.TabIndex = 0;
            //this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TbPg_Scope);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(797, 286);
            this.tabControl1.TabIndex = 0;
            // 
            // TbPg_Scope
            // 
            this.TbPg_Scope.Controls.Add(this.EqCurve);
            this.TbPg_Scope.Controls.Add(this.GainMeter);
            this.TbPg_Scope.Location = new System.Drawing.Point(4, 22);
            this.TbPg_Scope.Margin = new System.Windows.Forms.Padding(0);
            this.TbPg_Scope.Name = "TbPg_Scope";
            this.TbPg_Scope.Size = new System.Drawing.Size(789, 260);
            this.TbPg_Scope.TabIndex = 0;
            this.TbPg_Scope.Text = "Equalizer";
            this.TbPg_Scope.UseVisualStyleBackColor = true;
            // 
            // EqCurve
            // 
            this.EqCurve.BackColor = System.Drawing.Color.Black;
            this.EqCurve.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.EqCurve.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EqCurve.Location = new System.Drawing.Point(66, 0);
            this.EqCurve.Name = "EqCurve";
            this.EqCurve.Size = new System.Drawing.Size(723, 260);
            this.EqCurve.TabIndex = 0;
            this.EqCurve.TabStop = false;
            // 
            // GainMeter
            // 
            this.GainMeter.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.GainMeter.BackColor = System.Drawing.Color.Red;
            this.GainMeter.BackgroundColor = System.Drawing.Color.Black;
            this.GainMeter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.GainMeter.Dock = System.Windows.Forms.DockStyle.Left;
            this.GainMeter.LEDColor = System.Drawing.Color.DodgerBlue;
            this.GainMeter.LEDSize = 1;
            this.GainMeter.Level = 0;
            this.GainMeter.Location = new System.Drawing.Point(0, 0);
            this.GainMeter.Maximum = 1;
            this.GainMeter.Name = "GainMeter";
            this.GainMeter.Size = new System.Drawing.Size(66, 260);
            this.GainMeter.TabIndex = 0;
            // 
            // TbPg_Spectrogram
            // 
            this.TbPg_Spectrogram.BackColor = System.Drawing.Color.Black;
            this.TbPg_Spectrogram.Controls.Add(this.Pb_Spectrogram);
            this.TbPg_Spectrogram.Location = new System.Drawing.Point(4, 22);
            this.TbPg_Spectrogram.Name = "TbPg_Spectrogram";
            this.TbPg_Spectrogram.Size = new System.Drawing.Size(789, 260);
            this.TbPg_Spectrogram.TabIndex = 1;
            this.TbPg_Spectrogram.Text = "Spectrogram";
            // 
            // Pb_Spectrogram
            // 
            this.Pb_Spectrogram.BackColor = System.Drawing.Color.Black;
            this.Pb_Spectrogram.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Pb_Spectrogram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pb_Spectrogram.Location = new System.Drawing.Point(0, 0);
            this.Pb_Spectrogram.Margin = new System.Windows.Forms.Padding(0);
            this.Pb_Spectrogram.Name = "Pb_Spectrogram";
            this.Pb_Spectrogram.Size = new System.Drawing.Size(789, 260);
            this.Pb_Spectrogram.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pb_Spectrogram.TabIndex = 0;
            this.Pb_Spectrogram.TabStop = false;
            // 
            // EqualizerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 548);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EqualizerDialog";
            this.Text = "EqualizerDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EqualizerDialog_FormClosing);
            this.Load += new System.EventHandler(this.EqualizerDialog_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.TbPg_Scope.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EqCurve)).EndInit();
            this.TbPg_Spectrogram.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Spectrogram)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TbPg_Scope;
        private System.Windows.Forms.TabPage TbPg_Spectrogram;
        private System.Windows.Forms.PictureBox Pb_Spectrogram;
        private System.Windows.Forms.Timer Spectrogram;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem recenterEQToolStripMenuItem;
        private System.Windows.Forms.PictureBox EqCurve;
        private System.Windows.Forms.ToolStripMenuItem spectrogramONToolStripMenuItem;
        private Controls.MeterControl GainMeter;
    }
}