namespace YAMP_alpha
{
    partial class WavesReverbEffectDialog
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
            this.Tb_WaveRevInGain = new System.Windows.Forms.TrackBar();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_UpShift = new System.Windows.Forms.Button();
            this.Btn_DownShift = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Tb_WaveRev2TimeFine = new System.Windows.Forms.TrackBar();
            this.Tb_WaveRev2Time = new System.Windows.Forms.TrackBar();
            this.CB_EffectEnableToggle = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Tb_WaveRevHFRTR = new System.Windows.Forms.TrackBar();
            this.Tb_WaveRev2Mix = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_WaveRevInGain)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_WaveRev2TimeFine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_WaveRev2Time)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_WaveRevHFRTR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_WaveRev2Mix)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tb_WaveRevInGain
            // 
            this.Tb_WaveRevInGain.AutoSize = false;
            this.Tb_WaveRevInGain.Dock = System.Windows.Forms.DockStyle.Top;
            this.Tb_WaveRevInGain.Location = new System.Drawing.Point(3, 16);
            this.Tb_WaveRevInGain.Maximum = 0;
            this.Tb_WaveRevInGain.Minimum = -96;
            this.Tb_WaveRevInGain.Name = "Tb_WaveRevInGain";
            this.Tb_WaveRevInGain.Size = new System.Drawing.Size(374, 29);
            this.Tb_WaveRevInGain.TabIndex = 8;
            this.Tb_WaveRevInGain.TickFrequency = 3;
            this.Tb_WaveRevInGain.Value = -20;
            this.Tb_WaveRevInGain.ValueChanged += new System.EventHandler(this.Tb_WaveRevInGain_ValueChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.Tb_WaveRevInGain);
            this.groupBox7.Location = new System.Drawing.Point(14, 93);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(380, 53);
            this.groupBox7.TabIndex = 54;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "InGain";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(409, 22);
            this.label1.TabIndex = 47;
            this.label1.Text = "Change the values to observe a change in Chorus Effect";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Btn_UpShift
            // 
            this.Btn_UpShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_UpShift.Location = new System.Drawing.Point(-704, 289);
            this.Btn_UpShift.Name = "Btn_UpShift";
            this.Btn_UpShift.Size = new System.Drawing.Size(37, 22);
            this.Btn_UpShift.TabIndex = 49;
            this.Btn_UpShift.Text = ">>";
            this.Btn_UpShift.UseVisualStyleBackColor = true;
            // 
            // Btn_DownShift
            // 
            this.Btn_DownShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_DownShift.Location = new System.Drawing.Point(-747, 289);
            this.Btn_DownShift.Name = "Btn_DownShift";
            this.Btn_DownShift.Size = new System.Drawing.Size(37, 22);
            this.Btn_DownShift.TabIndex = 48;
            this.Btn_DownShift.Text = "<<";
            this.Btn_DownShift.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Tb_WaveRev2TimeFine);
            this.groupBox3.Controls.Add(this.Tb_WaveRev2Time);
            this.groupBox3.Location = new System.Drawing.Point(14, 214);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(380, 96);
            this.groupBox3.TabIndex = 55;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Reverb Time";
            // 
            // Tb_WaveRev2TimeFine
            // 
            this.Tb_WaveRev2TimeFine.AutoSize = false;
            this.Tb_WaveRev2TimeFine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_WaveRev2TimeFine.Location = new System.Drawing.Point(3, 56);
            this.Tb_WaveRev2TimeFine.Maximum = 1000;
            this.Tb_WaveRev2TimeFine.Name = "Tb_WaveRev2TimeFine";
            this.Tb_WaveRev2TimeFine.Size = new System.Drawing.Size(374, 37);
            this.Tb_WaveRev2TimeFine.TabIndex = 9;
            this.Tb_WaveRev2TimeFine.TickFrequency = 25;
            this.Tb_WaveRev2TimeFine.Value = 1;
            this.Tb_WaveRev2TimeFine.ValueChanged += new System.EventHandler(this.Tb_WaveRevRevTime_ValueChanged);
            // 
            // Tb_WaveRev2Time
            // 
            this.Tb_WaveRev2Time.AutoSize = false;
            this.Tb_WaveRev2Time.Dock = System.Windows.Forms.DockStyle.Top;
            this.Tb_WaveRev2Time.Location = new System.Drawing.Point(3, 16);
            this.Tb_WaveRev2Time.Maximum = 2999;
            this.Tb_WaveRev2Time.Minimum = 1;
            this.Tb_WaveRev2Time.Name = "Tb_WaveRev2Time";
            this.Tb_WaveRev2Time.Size = new System.Drawing.Size(374, 40);
            this.Tb_WaveRev2Time.TabIndex = 8;
            this.Tb_WaveRev2Time.TickFrequency = 222;
            this.Tb_WaveRev2Time.Value = 1;
            this.Tb_WaveRev2Time.ValueChanged += new System.EventHandler(this.Tb_WaveRevRevTime_ValueChanged);
            // 
            // CB_EffectEnableToggle
            // 
            this.CB_EffectEnableToggle.Location = new System.Drawing.Point(14, 316);
            this.CB_EffectEnableToggle.Name = "CB_EffectEnableToggle";
            this.CB_EffectEnableToggle.Size = new System.Drawing.Size(65, 16);
            this.CB_EffectEnableToggle.TabIndex = 56;
            this.CB_EffectEnableToggle.Text = "Enabled";
            this.CB_EffectEnableToggle.UseVisualStyleBackColor = true;
            this.CB_EffectEnableToggle.CheckedChanged += new System.EventHandler(this.CB_EffectEnableToggle_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Tb_WaveRevHFRTR);
            this.groupBox1.Location = new System.Drawing.Point(14, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(380, 53);
            this.groupBox1.TabIndex = 50;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "high-frequency RT Ratio";
            // 
            // Tb_WaveRevHFRTR
            // 
            this.Tb_WaveRevHFRTR.AutoSize = false;
            this.Tb_WaveRevHFRTR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_WaveRevHFRTR.Location = new System.Drawing.Point(3, 16);
            this.Tb_WaveRevHFRTR.Maximum = 999;
            this.Tb_WaveRevHFRTR.Minimum = 1;
            this.Tb_WaveRevHFRTR.Name = "Tb_WaveRevHFRTR";
            this.Tb_WaveRevHFRTR.Size = new System.Drawing.Size(374, 34);
            this.Tb_WaveRevHFRTR.TabIndex = 8;
            this.Tb_WaveRevHFRTR.TickFrequency = 25;
            this.Tb_WaveRevHFRTR.Value = 1;
            this.Tb_WaveRevHFRTR.ValueChanged += new System.EventHandler(this.Tb_WaveRevHFRTR_ValueChanged);
            // 
            // Tb_WaveRev2Mix
            // 
            this.Tb_WaveRev2Mix.AutoSize = false;
            this.Tb_WaveRev2Mix.Dock = System.Windows.Forms.DockStyle.Top;
            this.Tb_WaveRev2Mix.Location = new System.Drawing.Point(3, 16);
            this.Tb_WaveRev2Mix.Maximum = 0;
            this.Tb_WaveRev2Mix.Minimum = -96;
            this.Tb_WaveRev2Mix.Name = "Tb_WaveRev2Mix";
            this.Tb_WaveRev2Mix.Size = new System.Drawing.Size(374, 29);
            this.Tb_WaveRev2Mix.TabIndex = 8;
            this.Tb_WaveRev2Mix.TickFrequency = 3;
            this.Tb_WaveRev2Mix.Value = -48;
            this.Tb_WaveRev2Mix.ValueChanged += new System.EventHandler(this.Tb_WaveRev2Mix_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Tb_WaveRev2Mix);
            this.groupBox2.Location = new System.Drawing.Point(14, 155);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(380, 53);
            this.groupBox2.TabIndex = 53;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Reverb Mix";
            // 
            // WavesReverbEffectDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 344);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Btn_UpShift);
            this.Controls.Add(this.Btn_DownShift);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.CB_EffectEnableToggle);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "WavesReverbEffectDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WavesReverbEffectDialog";
            this.Load += new System.EventHandler(this.WavesReverbEffectDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Tb_WaveRevInGain)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Tb_WaveRev2TimeFine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_WaveRev2Time)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Tb_WaveRevHFRTR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_WaveRev2Mix)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar Tb_WaveRevInGain;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_UpShift;
        private System.Windows.Forms.Button Btn_DownShift;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TrackBar Tb_WaveRev2Time;
        private System.Windows.Forms.CheckBox CB_EffectEnableToggle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar Tb_WaveRevHFRTR;
        private System.Windows.Forms.TrackBar Tb_WaveRev2Mix;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TrackBar Tb_WaveRev2TimeFine;
    }
}