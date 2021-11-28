namespace YAMP_alpha
{
    partial class ChorusEffectDialog
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
            this.Tb_ChorusWDMixBar = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Tb_ChorusFeedBar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.NumUD_FreqUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_UpShift = new System.Windows.Forms.Button();
            this.Btn_DownShift = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Tb_ChorusDepthBar = new System.Windows.Forms.TrackBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.CmbBx_PhaseBox = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.CmbBx_WaveFormBox = new System.Windows.Forms.ComboBox();
            this.CB_EffectEnableToggle = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.Tb_ChorusDelayBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_ChorusWDMixBar)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_ChorusFeedBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_FreqUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_ChorusDepthBar)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_ChorusDelayBar)).BeginInit();
            this.SuspendLayout();
            // 
            // Tb_ChorusWDMixBar
            // 
            this.Tb_ChorusWDMixBar.AutoSize = false;
            this.Tb_ChorusWDMixBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.Tb_ChorusWDMixBar.Location = new System.Drawing.Point(3, 16);
            this.Tb_ChorusWDMixBar.Maximum = 100;
            this.Tb_ChorusWDMixBar.Name = "Tb_ChorusWDMixBar";
            this.Tb_ChorusWDMixBar.Size = new System.Drawing.Size(353, 29);
            this.Tb_ChorusWDMixBar.TabIndex = 8;
            this.Tb_ChorusWDMixBar.TickFrequency = 0;
            this.Tb_ChorusWDMixBar.ValueChanged += new System.EventHandler(this.Tb_ChorusWDMixBar_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Tb_ChorusFeedBar);
            this.groupBox2.Location = new System.Drawing.Point(12, 93);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(359, 53);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Feedback";
            // 
            // Tb_ChorusFeedBar
            // 
            this.Tb_ChorusFeedBar.AutoSize = false;
            this.Tb_ChorusFeedBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.Tb_ChorusFeedBar.Location = new System.Drawing.Point(3, 16);
            this.Tb_ChorusFeedBar.Maximum = 99;
            this.Tb_ChorusFeedBar.Minimum = -99;
            this.Tb_ChorusFeedBar.Name = "Tb_ChorusFeedBar";
            this.Tb_ChorusFeedBar.Size = new System.Drawing.Size(353, 29);
            this.Tb_ChorusFeedBar.TabIndex = 8;
            this.Tb_ChorusFeedBar.TickFrequency = 0;
            this.Tb_ChorusFeedBar.ValueChanged += new System.EventHandler(this.Tb_ChorusFeedBar_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Frequency";
            // 
            // NumUD_FreqUpDown
            // 
            this.NumUD_FreqUpDown.DecimalPlaces = 2;
            this.NumUD_FreqUpDown.Location = new System.Drawing.Point(75, 168);
            this.NumUD_FreqUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumUD_FreqUpDown.Name = "NumUD_FreqUpDown";
            this.NumUD_FreqUpDown.Size = new System.Drawing.Size(53, 20);
            this.NumUD_FreqUpDown.TabIndex = 27;
            this.NumUD_FreqUpDown.ValueChanged += new System.EventHandler(this.NumUD_FreqUpDown_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Tb_ChorusWDMixBar);
            this.groupBox1.Location = new System.Drawing.Point(12, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(359, 53);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "WetDryMix";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(383, 22);
            this.label1.TabIndex = 23;
            this.label1.Text = "Change the values to observe a change in Chorus Effect";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Btn_UpShift
            // 
            this.Btn_UpShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_UpShift.Location = new System.Drawing.Point(-698, 246);
            this.Btn_UpShift.Name = "Btn_UpShift";
            this.Btn_UpShift.Size = new System.Drawing.Size(37, 22);
            this.Btn_UpShift.TabIndex = 25;
            this.Btn_UpShift.Text = ">>";
            this.Btn_UpShift.UseVisualStyleBackColor = true;
            // 
            // Btn_DownShift
            // 
            this.Btn_DownShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_DownShift.Location = new System.Drawing.Point(-741, 246);
            this.Btn_DownShift.Name = "Btn_DownShift";
            this.Btn_DownShift.Size = new System.Drawing.Size(37, 22);
            this.Btn_DownShift.TabIndex = 24;
            this.Btn_DownShift.Text = "<<";
            this.Btn_DownShift.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Tb_ChorusDepthBar);
            this.groupBox3.Location = new System.Drawing.Point(134, 152);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(237, 53);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Depth";
            // 
            // Tb_ChorusDepthBar
            // 
            this.Tb_ChorusDepthBar.AutoSize = false;
            this.Tb_ChorusDepthBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.Tb_ChorusDepthBar.Location = new System.Drawing.Point(3, 16);
            this.Tb_ChorusDepthBar.Maximum = 100;
            this.Tb_ChorusDepthBar.Name = "Tb_ChorusDepthBar";
            this.Tb_ChorusDepthBar.Size = new System.Drawing.Size(231, 29);
            this.Tb_ChorusDepthBar.TabIndex = 8;
            this.Tb_ChorusDepthBar.TickFrequency = 0;
            this.Tb_ChorusDepthBar.ValueChanged += new System.EventHandler(this.Tb_ChorusDepthBar_ValueChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.CmbBx_PhaseBox);
            this.groupBox4.Location = new System.Drawing.Point(12, 279);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(92, 41);
            this.groupBox4.TabIndex = 31;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Phase";
            // 
            // CmbBx_PhaseBox
            // 
            this.CmbBx_PhaseBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CmbBx_PhaseBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBx_PhaseBox.FormattingEnabled = true;
            this.CmbBx_PhaseBox.Items.AddRange(new object[] {
            "PhaseNegative180",
            "PhaseNegative90",
            "PhaseZero",
            "Phase90",
            "Phase180"});
            this.CmbBx_PhaseBox.Location = new System.Drawing.Point(3, 16);
            this.CmbBx_PhaseBox.Name = "CmbBx_PhaseBox";
            this.CmbBx_PhaseBox.Size = new System.Drawing.Size(86, 21);
            this.CmbBx_PhaseBox.TabIndex = 3;
            this.CmbBx_PhaseBox.SelectedIndexChanged += new System.EventHandler(this.CmbBx_PhaseBox_SelectedIndexChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.CmbBx_WaveFormBox);
            this.groupBox5.Location = new System.Drawing.Point(157, 279);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(92, 41);
            this.groupBox5.TabIndex = 32;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Wave Form";
            // 
            // CmbBx_WaveFormBox
            // 
            this.CmbBx_WaveFormBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CmbBx_WaveFormBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBx_WaveFormBox.FormattingEnabled = true;
            this.CmbBx_WaveFormBox.Items.AddRange(new object[] {
            "Triangle",
            "Sin"});
            this.CmbBx_WaveFormBox.Location = new System.Drawing.Point(3, 16);
            this.CmbBx_WaveFormBox.Name = "CmbBx_WaveFormBox";
            this.CmbBx_WaveFormBox.Size = new System.Drawing.Size(86, 21);
            this.CmbBx_WaveFormBox.TabIndex = 3;
            this.CmbBx_WaveFormBox.SelectedIndexChanged += new System.EventHandler(this.CmbBx_WaveFormBox_SelectedIndexChanged);
            // 
            // CB_EffectEnableToggle
            // 
            this.CB_EffectEnableToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CB_EffectEnableToggle.Location = new System.Drawing.Point(306, 304);
            this.CB_EffectEnableToggle.Name = "CB_EffectEnableToggle";
            this.CB_EffectEnableToggle.Size = new System.Drawing.Size(65, 16);
            this.CB_EffectEnableToggle.TabIndex = 33;
            this.CB_EffectEnableToggle.Text = "Enabled";
            this.CB_EffectEnableToggle.UseVisualStyleBackColor = true;
            this.CB_EffectEnableToggle.CheckedChanged += new System.EventHandler(this.CB_EffectEnableToggle_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.Tb_ChorusDelayBar);
            this.groupBox6.Location = new System.Drawing.Point(12, 211);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(359, 53);
            this.groupBox6.TabIndex = 34;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Input Delay";
            // 
            // Tb_ChorusDelayBar
            // 
            this.Tb_ChorusDelayBar.AutoSize = false;
            this.Tb_ChorusDelayBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.Tb_ChorusDelayBar.Location = new System.Drawing.Point(3, 16);
            this.Tb_ChorusDelayBar.Maximum = 20;
            this.Tb_ChorusDelayBar.Name = "Tb_ChorusDelayBar";
            this.Tb_ChorusDelayBar.Size = new System.Drawing.Size(353, 29);
            this.Tb_ChorusDelayBar.TabIndex = 8;
            this.Tb_ChorusDelayBar.TickFrequency = 0;
            this.Tb_ChorusDelayBar.ValueChanged += new System.EventHandler(this.Tb_ChorusDelayBar_ValueChanged);
            // 
            // ChorusEffectDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 333);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.NumUD_FreqUpDown);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Btn_UpShift);
            this.Controls.Add(this.Btn_DownShift);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.CB_EffectEnableToggle);
            this.Name = "ChorusEffectDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ChorusEffectDialog";
            this.Load += new System.EventHandler(this.ChorusEffectDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Tb_ChorusWDMixBar)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Tb_ChorusFeedBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_FreqUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Tb_ChorusDepthBar)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Tb_ChorusDelayBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar Tb_ChorusWDMixBar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TrackBar Tb_ChorusFeedBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NumUD_FreqUpDown;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_UpShift;
        private System.Windows.Forms.Button Btn_DownShift;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TrackBar Tb_ChorusDepthBar;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox CmbBx_PhaseBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox CmbBx_WaveFormBox;
        private System.Windows.Forms.CheckBox CB_EffectEnableToggle;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TrackBar Tb_ChorusDelayBar;
    }
}