namespace YAMP_alpha
{
    partial class CompressorEffectDialog
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
            this.Tb_CompRelease = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Tb_CompGain = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.NumUD_PreDelUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Tb_CompFineAtckStrn = new System.Windows.Forms.TrackBar();
            this.Tb_CompAtckStrn = new System.Windows.Forms.TrackBar();
            this.Tb_CompRatio = new System.Windows.Forms.TrackBar();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_UpShift = new System.Windows.Forms.Button();
            this.Btn_DownShift = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.CB_EffectEnableToggle = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.Tb_CompThresh = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_CompRelease)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_CompGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_PreDelUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_CompFineAtckStrn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_CompAtckStrn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_CompRatio)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_CompThresh)).BeginInit();
            this.SuspendLayout();
            // 
            // Tb_CompRelease
            // 
            this.Tb_CompRelease.AutoSize = false;
            this.Tb_CompRelease.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_CompRelease.Location = new System.Drawing.Point(3, 16);
            this.Tb_CompRelease.Maximum = 3000;
            this.Tb_CompRelease.Minimum = 50;
            this.Tb_CompRelease.Name = "Tb_CompRelease";
            this.Tb_CompRelease.Size = new System.Drawing.Size(374, 34);
            this.Tb_CompRelease.TabIndex = 8;
            this.Tb_CompRelease.TickFrequency = 0;
            this.Tb_CompRelease.Value = 51;
            this.Tb_CompRelease.ValueChanged += new System.EventHandler(this.Tb_CompRelease_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Tb_CompGain);
            this.groupBox2.Location = new System.Drawing.Point(12, 157);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(380, 53);
            this.groupBox2.TabIndex = 41;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Gain";
            // 
            // Tb_CompGain
            // 
            this.Tb_CompGain.AutoSize = false;
            this.Tb_CompGain.Dock = System.Windows.Forms.DockStyle.Top;
            this.Tb_CompGain.Location = new System.Drawing.Point(3, 16);
            this.Tb_CompGain.Maximum = 60;
            this.Tb_CompGain.Minimum = -60;
            this.Tb_CompGain.Name = "Tb_CompGain";
            this.Tb_CompGain.Size = new System.Drawing.Size(374, 29);
            this.Tb_CompGain.TabIndex = 8;
            this.Tb_CompGain.TickFrequency = 0;
            this.Tb_CompGain.ValueChanged += new System.EventHandler(this.Tb_CompGain_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(283, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "PreDelay";
            // 
            // NumUD_PreDelUpDown
            // 
            this.NumUD_PreDelUpDown.Location = new System.Drawing.Point(339, 111);
            this.NumUD_PreDelUpDown.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.NumUD_PreDelUpDown.Name = "NumUD_PreDelUpDown";
            this.NumUD_PreDelUpDown.Size = new System.Drawing.Size(53, 20);
            this.NumUD_PreDelUpDown.TabIndex = 39;
            this.NumUD_PreDelUpDown.ValueChanged += new System.EventHandler(this.NumUD_PreDelUpDown_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Tb_CompFineAtckStrn);
            this.groupBox1.Controls.Add(this.Tb_CompAtckStrn);
            this.groupBox1.Location = new System.Drawing.Point(12, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(380, 53);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Attack";
            // 
            // Tb_CompFineAtckStrn
            // 
            this.Tb_CompFineAtckStrn.AutoSize = false;
            this.Tb_CompFineAtckStrn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_CompFineAtckStrn.Location = new System.Drawing.Point(237, 16);
            this.Tb_CompFineAtckStrn.Maximum = 100;
            this.Tb_CompFineAtckStrn.Name = "Tb_CompFineAtckStrn";
            this.Tb_CompFineAtckStrn.Size = new System.Drawing.Size(140, 34);
            this.Tb_CompFineAtckStrn.TabIndex = 9;
            this.Tb_CompFineAtckStrn.TickFrequency = 4;
            this.Tb_CompFineAtckStrn.Value = 1;
            this.Tb_CompFineAtckStrn.ValueChanged += new System.EventHandler(this.Tb_CompFineAtckStrn_ValueChanged);
            // 
            // Tb_CompAtckStrn
            // 
            this.Tb_CompAtckStrn.AutoSize = false;
            this.Tb_CompAtckStrn.Dock = System.Windows.Forms.DockStyle.Left;
            this.Tb_CompAtckStrn.Location = new System.Drawing.Point(3, 16);
            this.Tb_CompAtckStrn.Maximum = 499;
            this.Tb_CompAtckStrn.Minimum = 1;
            this.Tb_CompAtckStrn.Name = "Tb_CompAtckStrn";
            this.Tb_CompAtckStrn.Size = new System.Drawing.Size(234, 34);
            this.Tb_CompAtckStrn.TabIndex = 8;
            this.Tb_CompAtckStrn.TickFrequency = 25;
            this.Tb_CompAtckStrn.Value = 1;
            this.Tb_CompAtckStrn.ValueChanged += new System.EventHandler(this.Tb_CompFineAtckStrn_ValueChanged);
            // 
            // Tb_CompRatio
            // 
            this.Tb_CompRatio.AutoSize = false;
            this.Tb_CompRatio.Dock = System.Windows.Forms.DockStyle.Top;
            this.Tb_CompRatio.Location = new System.Drawing.Point(3, 16);
            this.Tb_CompRatio.Maximum = 100;
            this.Tb_CompRatio.Minimum = 1;
            this.Tb_CompRatio.Name = "Tb_CompRatio";
            this.Tb_CompRatio.Size = new System.Drawing.Size(374, 29);
            this.Tb_CompRatio.TabIndex = 8;
            this.Tb_CompRatio.TickFrequency = 0;
            this.Tb_CompRatio.Value = 1;
            this.Tb_CompRatio.ValueChanged += new System.EventHandler(this.Tb_CompRatio_ValueChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.Tb_CompRelease);
            this.groupBox6.Location = new System.Drawing.Point(12, 275);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(380, 53);
            this.groupBox6.TabIndex = 46;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Release";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(411, 22);
            this.label1.TabIndex = 35;
            this.label1.Text = "Change the values to observe a change in Chorus Effect";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Btn_UpShift
            // 
            this.Btn_UpShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_UpShift.Location = new System.Drawing.Point(-670, 248);
            this.Btn_UpShift.Name = "Btn_UpShift";
            this.Btn_UpShift.Size = new System.Drawing.Size(37, 22);
            this.Btn_UpShift.TabIndex = 37;
            this.Btn_UpShift.Text = ">>";
            this.Btn_UpShift.UseVisualStyleBackColor = true;
            // 
            // Btn_DownShift
            // 
            this.Btn_DownShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_DownShift.Location = new System.Drawing.Point(-713, 248);
            this.Btn_DownShift.Name = "Btn_DownShift";
            this.Btn_DownShift.Size = new System.Drawing.Size(37, 22);
            this.Btn_DownShift.TabIndex = 36;
            this.Btn_DownShift.Text = "<<";
            this.Btn_DownShift.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Tb_CompRatio);
            this.groupBox3.Location = new System.Drawing.Point(12, 216);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(380, 53);
            this.groupBox3.TabIndex = 42;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Ratio";
            // 
            // CB_EffectEnableToggle
            // 
            this.CB_EffectEnableToggle.Location = new System.Drawing.Point(12, 353);
            this.CB_EffectEnableToggle.Name = "CB_EffectEnableToggle";
            this.CB_EffectEnableToggle.Size = new System.Drawing.Size(65, 16);
            this.CB_EffectEnableToggle.TabIndex = 45;
            this.CB_EffectEnableToggle.Text = "Enabled";
            this.CB_EffectEnableToggle.UseVisualStyleBackColor = true;
            this.CB_EffectEnableToggle.CheckedChanged += new System.EventHandler(this.CB_EffectEnableToggle_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.Tb_CompThresh);
            this.groupBox7.Location = new System.Drawing.Point(12, 95);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(265, 53);
            this.groupBox7.TabIndex = 42;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Threshold";
            // 
            // Tb_CompThresh
            // 
            this.Tb_CompThresh.AutoSize = false;
            this.Tb_CompThresh.Dock = System.Windows.Forms.DockStyle.Top;
            this.Tb_CompThresh.Location = new System.Drawing.Point(3, 16);
            this.Tb_CompThresh.Maximum = 0;
            this.Tb_CompThresh.Minimum = -60;
            this.Tb_CompThresh.Name = "Tb_CompThresh";
            this.Tb_CompThresh.Size = new System.Drawing.Size(259, 29);
            this.Tb_CompThresh.TabIndex = 8;
            this.Tb_CompThresh.TickFrequency = 0;
            this.Tb_CompThresh.Value = -20;
            this.Tb_CompThresh.ValueChanged += new System.EventHandler(this.Tb_CompThresh_ValueChanged);
            // 
            // CompressorEffectDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 381);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.NumUD_PreDelUpDown);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Btn_UpShift);
            this.Controls.Add(this.Btn_DownShift);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.CB_EffectEnableToggle);
            this.Name = "CompressorEffectDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CompressorEffectDialog";
            this.Load += new System.EventHandler(this.CompressorEffectDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Tb_CompRelease)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Tb_CompGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_PreDelUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Tb_CompFineAtckStrn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_CompAtckStrn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_CompRatio)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Tb_CompThresh)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar Tb_CompRelease;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TrackBar Tb_CompGain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NumUD_PreDelUpDown;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar Tb_CompAtckStrn;
        private System.Windows.Forms.TrackBar Tb_CompRatio;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_UpShift;
        private System.Windows.Forms.Button Btn_DownShift;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox CB_EffectEnableToggle;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TrackBar Tb_CompThresh;
        private System.Windows.Forms.TrackBar Tb_CompFineAtckStrn;
    }
}