namespace YAMP_alpha
{
    partial class DistortionEffectDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Tb_DistortGain = new System.Windows.Forms.TrackBar();
            this.Tb_DistortPostEQBW = new System.Windows.Forms.TrackBar();
            this.Tb_DistortPostEQCF = new System.Windows.Forms.TrackBar();
            this.Tb_DistortPLC = new System.Windows.Forms.TrackBar();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.Tb_DistortEdge = new System.Windows.Forms.TrackBar();
            this.Cb_DistortEffectEnable = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_DistortGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_DistortPostEQBW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_DistortPostEQCF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_DistortPLC)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_DistortEdge)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(379, 22);
            this.label1.TabIndex = 10;
            this.label1.Text = "Change the value (Hz) to observe a change in Distortion effect";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Tb_DistortGain);
            this.groupBox1.Location = new System.Drawing.Point(9, 117);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(357, 62);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Gain";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Tb_DistortPostEQBW);
            this.groupBox2.Location = new System.Drawing.Point(9, 196);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(357, 62);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PostEQBandwidth";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Tb_DistortPostEQCF);
            this.groupBox3.Location = new System.Drawing.Point(9, 275);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(357, 62);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PostEQCenterFrequency";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.Tb_DistortPLC);
            this.groupBox4.Location = new System.Drawing.Point(9, 354);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(357, 62);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "PreLowpassCutoff";
            // 
            // Tb_DistortGain
            // 
            this.Tb_DistortGain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_DistortGain.Location = new System.Drawing.Point(3, 16);
            this.Tb_DistortGain.Maximum = 0;
            this.Tb_DistortGain.Minimum = -60;
            this.Tb_DistortGain.Name = "Tb_DistortGain";
            this.Tb_DistortGain.Size = new System.Drawing.Size(351, 43);
            this.Tb_DistortGain.TabIndex = 0;
            this.Tb_DistortGain.ValueChanged += new System.EventHandler(this.Tb_DistortGain_ValueChanged);
            // 
            // Tb_DistortPostEQBW
            // 
            this.Tb_DistortPostEQBW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_DistortPostEQBW.Location = new System.Drawing.Point(3, 16);
            this.Tb_DistortPostEQBW.Maximum = 8000;
            this.Tb_DistortPostEQBW.Minimum = 100;
            this.Tb_DistortPostEQBW.Name = "Tb_DistortPostEQBW";
            this.Tb_DistortPostEQBW.Size = new System.Drawing.Size(351, 43);
            this.Tb_DistortPostEQBW.TabIndex = 1;
            this.Tb_DistortPostEQBW.TickFrequency = 100;
            this.Tb_DistortPostEQBW.Value = 100;
            this.Tb_DistortPostEQBW.ValueChanged += new System.EventHandler(this.Tb_DistortPostEQBW_ValueChanged);
            // 
            // Tb_DistortPostEQCF
            // 
            this.Tb_DistortPostEQCF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_DistortPostEQCF.Location = new System.Drawing.Point(3, 16);
            this.Tb_DistortPostEQCF.Maximum = 8000;
            this.Tb_DistortPostEQCF.Minimum = 100;
            this.Tb_DistortPostEQCF.Name = "Tb_DistortPostEQCF";
            this.Tb_DistortPostEQCF.Size = new System.Drawing.Size(351, 43);
            this.Tb_DistortPostEQCF.TabIndex = 1;
            this.Tb_DistortPostEQCF.TickFrequency = 100;
            this.Tb_DistortPostEQCF.Value = 100;
            this.Tb_DistortPostEQCF.ValueChanged += new System.EventHandler(this.Tb_DistortPostEQCF_ValueChanged);
            // 
            // Tb_DistortPLC
            // 
            this.Tb_DistortPLC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_DistortPLC.Location = new System.Drawing.Point(3, 16);
            this.Tb_DistortPLC.Maximum = 8000;
            this.Tb_DistortPLC.Minimum = 100;
            this.Tb_DistortPLC.Name = "Tb_DistortPLC";
            this.Tb_DistortPLC.Size = new System.Drawing.Size(351, 43);
            this.Tb_DistortPLC.TabIndex = 1;
            this.Tb_DistortPLC.TickFrequency = 100;
            this.Tb_DistortPLC.Value = 100;
            this.Tb_DistortPLC.ValueChanged += new System.EventHandler(this.Tb_DistortPLC_ValueChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.Tb_DistortEdge);
            this.groupBox5.Location = new System.Drawing.Point(9, 38);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(357, 62);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Edge";
            // 
            // Tb_DistortEdge
            // 
            this.Tb_DistortEdge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_DistortEdge.Location = new System.Drawing.Point(3, 16);
            this.Tb_DistortEdge.Maximum = 100;
            this.Tb_DistortEdge.Name = "Tb_DistortEdge";
            this.Tb_DistortEdge.Size = new System.Drawing.Size(351, 43);
            this.Tb_DistortEdge.TabIndex = 0;
            this.Tb_DistortEdge.ValueChanged += new System.EventHandler(this.Tb_DistortEdge_ValueChanged);
            // 
            // Cb_DistortEffectEnable
            // 
            this.Cb_DistortEffectEnable.AutoSize = true;
            this.Cb_DistortEffectEnable.Location = new System.Drawing.Point(298, 436);
            this.Cb_DistortEffectEnable.Name = "Cb_DistortEffectEnable";
            this.Cb_DistortEffectEnable.Size = new System.Drawing.Size(65, 17);
            this.Cb_DistortEffectEnable.TabIndex = 14;
            this.Cb_DistortEffectEnable.Text = "Enabled";
            this.Cb_DistortEffectEnable.UseVisualStyleBackColor = true;
            this.Cb_DistortEffectEnable.CheckedChanged += new System.EventHandler(this.Cb_DistortEffectEnable_CheckedChanged);
            // 
            // DistortionEffectDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 465);
            this.Controls.Add(this.Cb_DistortEffectEnable);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "DistortionEffectDialog";
            this.Text = "DistortionEffectDialog";
            this.Load += new System.EventHandler(this.DistortionEffectDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_DistortGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_DistortPostEQBW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_DistortPostEQCF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_DistortPLC)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_DistortEdge)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar Tb_DistortGain;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TrackBar Tb_DistortPostEQBW;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TrackBar Tb_DistortPostEQCF;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TrackBar Tb_DistortPLC;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TrackBar Tb_DistortEdge;
        private System.Windows.Forms.CheckBox Cb_DistortEffectEnable;
    }
}