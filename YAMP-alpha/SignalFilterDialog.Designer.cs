namespace YAMP_alpha
{
    partial class SignalFilterDialog
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
            this.toggleFilterCheck = new System.Windows.Forms.CheckBox();
            this.InitParamGroupBox = new System.Windows.Forms.GroupBox();
            this.BtnReset = new System.Windows.Forms.Button();
            this.nudInitBndWdt = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nudInitGain = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudInitFreq = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.FltCtrGroupBox = new System.Windows.Forms.GroupBox();
            this.BndWdtTracker = new System.Windows.Forms.TrackBar();
            this.gainTracker = new System.Windows.Forms.TrackBar();
            this.freqTracker = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.nudBndWdt = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nudFreq = new System.Windows.Forms.NumericUpDown();
            this.nudGain = new System.Windows.Forms.NumericUpDown();
            this.filterGroupBox = new System.Windows.Forms.GroupBox();
            this.radioPeak = new System.Windows.Forms.RadioButton();
            this.radioBandReject = new System.Windows.Forms.RadioButton();
            this.radioLowShelf = new System.Windows.Forms.RadioButton();
            this.radioLowPass = new System.Windows.Forms.RadioButton();
            this.radioHighShelf = new System.Windows.Forms.RadioButton();
            this.radioHighPass = new System.Windows.Forms.RadioButton();
            this.radioBandPass = new System.Windows.Forms.RadioButton();
            this.InitParamGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInitBndWdt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInitGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInitFreq)).BeginInit();
            this.FltCtrGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BndWdtTracker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gainTracker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.freqTracker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBndWdt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFreq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGain)).BeginInit();
            this.filterGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // toggleFilterCheck
            // 
            this.toggleFilterCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toggleFilterCheck.AutoSize = true;
            this.toggleFilterCheck.Location = new System.Drawing.Point(233, 351);
            this.toggleFilterCheck.Name = "toggleFilterCheck";
            this.toggleFilterCheck.Size = new System.Drawing.Size(98, 17);
            this.toggleFilterCheck.TabIndex = 15;
            this.toggleFilterCheck.Text = "Enable Filtering";
            this.toggleFilterCheck.UseVisualStyleBackColor = true;
            this.toggleFilterCheck.CheckedChanged += new System.EventHandler(this.toggleFilterCheck_CheckedChanged);
            // 
            // InitParamGroupBox
            // 
            this.InitParamGroupBox.Controls.Add(this.BtnReset);
            this.InitParamGroupBox.Controls.Add(this.nudInitBndWdt);
            this.InitParamGroupBox.Controls.Add(this.label5);
            this.InitParamGroupBox.Controls.Add(this.nudInitGain);
            this.InitParamGroupBox.Controls.Add(this.label4);
            this.InitParamGroupBox.Controls.Add(this.nudInitFreq);
            this.InitParamGroupBox.Controls.Add(this.label6);
            this.InitParamGroupBox.Enabled = false;
            this.InitParamGroupBox.Location = new System.Drawing.Point(12, 10);
            this.InitParamGroupBox.Name = "InitParamGroupBox";
            this.InitParamGroupBox.Size = new System.Drawing.Size(106, 171);
            this.InitParamGroupBox.TabIndex = 14;
            this.InitParamGroupBox.TabStop = false;
            this.InitParamGroupBox.Text = "Reset Parameters";
            // 
            // BtnReset
            // 
            this.BtnReset.Location = new System.Drawing.Point(18, 141);
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.Size = new System.Drawing.Size(64, 23);
            this.BtnReset.TabIndex = 16;
            this.BtnReset.Text = "Reset";
            this.BtnReset.UseVisualStyleBackColor = true;
            this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // nudInitBndWdt
            // 
            this.nudInitBndWdt.Location = new System.Drawing.Point(18, 115);
            this.nudInitBndWdt.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudInitBndWdt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInitBndWdt.Name = "nudInitBndWdt";
            this.nudInitBndWdt.Size = new System.Drawing.Size(63, 20);
            this.nudInitBndWdt.TabIndex = 14;
            this.nudInitBndWdt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Gain";
            // 
            // nudInitGain
            // 
            this.nudInitGain.Location = new System.Drawing.Point(18, 76);
            this.nudInitGain.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudInitGain.Name = "nudInitGain";
            this.nudInitGain.Size = new System.Drawing.Size(63, 20);
            this.nudInitGain.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Frequency";
            // 
            // nudInitFreq
            // 
            this.nudInitFreq.Location = new System.Drawing.Point(18, 37);
            this.nudInitFreq.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudInitFreq.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInitFreq.Name = "nudInitFreq";
            this.nudInitFreq.Size = new System.Drawing.Size(63, 20);
            this.nudInitFreq.TabIndex = 9;
            this.nudInitFreq.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Band Width";
            // 
            // FltCtrGroupBox
            // 
            this.FltCtrGroupBox.Controls.Add(this.BndWdtTracker);
            this.FltCtrGroupBox.Controls.Add(this.gainTracker);
            this.FltCtrGroupBox.Controls.Add(this.freqTracker);
            this.FltCtrGroupBox.Controls.Add(this.label3);
            this.FltCtrGroupBox.Controls.Add(this.nudBndWdt);
            this.FltCtrGroupBox.Controls.Add(this.label2);
            this.FltCtrGroupBox.Controls.Add(this.label1);
            this.FltCtrGroupBox.Controls.Add(this.nudFreq);
            this.FltCtrGroupBox.Controls.Add(this.nudGain);
            this.FltCtrGroupBox.Enabled = false;
            this.FltCtrGroupBox.Location = new System.Drawing.Point(124, 10);
            this.FltCtrGroupBox.Name = "FltCtrGroupBox";
            this.FltCtrGroupBox.Size = new System.Drawing.Size(207, 335);
            this.FltCtrGroupBox.TabIndex = 13;
            this.FltCtrGroupBox.TabStop = false;
            this.FltCtrGroupBox.Text = "Filter Controls";
            // 
            // BndWdtTracker
            // 
            this.BndWdtTracker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.BndWdtTracker.AutoSize = false;
            this.BndWdtTracker.Location = new System.Drawing.Point(144, 51);
            this.BndWdtTracker.Maximum = 1000;
            this.BndWdtTracker.Minimum = 1;
            this.BndWdtTracker.Name = "BndWdtTracker";
            this.BndWdtTracker.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.BndWdtTracker.Size = new System.Drawing.Size(42, 255);
            this.BndWdtTracker.TabIndex = 16;
            this.BndWdtTracker.TickFrequency = 10;
            this.BndWdtTracker.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.BndWdtTracker.Value = 1;
            this.BndWdtTracker.ValueChanged += new System.EventHandler(this.BndWdt_ValueChanged);
            // 
            // gainTracker
            // 
            this.gainTracker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gainTracker.AutoSize = false;
            this.gainTracker.Location = new System.Drawing.Point(83, 51);
            this.gainTracker.Maximum = 50;
            this.gainTracker.Name = "gainTracker";
            this.gainTracker.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.gainTracker.Size = new System.Drawing.Size(42, 255);
            this.gainTracker.TabIndex = 15;
            this.gainTracker.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.gainTracker.ValueChanged += new System.EventHandler(this.Gain_ValueChanged);
            // 
            // freqTracker
            // 
            this.freqTracker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.freqTracker.AutoSize = false;
            this.freqTracker.Location = new System.Drawing.Point(22, 51);
            this.freqTracker.Maximum = 10000;
            this.freqTracker.Minimum = 1;
            this.freqTracker.Name = "freqTracker";
            this.freqTracker.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.freqTracker.Size = new System.Drawing.Size(43, 255);
            this.freqTracker.TabIndex = 14;
            this.freqTracker.TickFrequency = 100;
            this.freqTracker.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.freqTracker.Value = 1;
            this.freqTracker.ValueChanged += new System.EventHandler(this.Freq_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(137, 309);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Bandwidth";
            // 
            // nudBndWdt
            // 
            this.nudBndWdt.Location = new System.Drawing.Point(138, 25);
            this.nudBndWdt.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudBndWdt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBndWdt.Name = "nudBndWdt";
            this.nudBndWdt.Size = new System.Drawing.Size(55, 20);
            this.nudBndWdt.TabIndex = 11;
            this.nudBndWdt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBndWdt.ValueChanged += new System.EventHandler(this.BndWdt_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(90, 309);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Gain";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 309);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Frequency";
            // 
            // nudFreq
            // 
            this.nudFreq.Location = new System.Drawing.Point(16, 25);
            this.nudFreq.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudFreq.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFreq.Name = "nudFreq";
            this.nudFreq.Size = new System.Drawing.Size(55, 20);
            this.nudFreq.TabIndex = 7;
            this.nudFreq.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFreq.ValueChanged += new System.EventHandler(this.Freq_ValueChanged);
            // 
            // nudGain
            // 
            this.nudGain.Location = new System.Drawing.Point(77, 25);
            this.nudGain.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudGain.Name = "nudGain";
            this.nudGain.Size = new System.Drawing.Size(55, 20);
            this.nudGain.TabIndex = 9;
            this.nudGain.ValueChanged += new System.EventHandler(this.Gain_ValueChanged);
            // 
            // filterGroupBox
            // 
            this.filterGroupBox.Controls.Add(this.radioPeak);
            this.filterGroupBox.Controls.Add(this.radioBandReject);
            this.filterGroupBox.Controls.Add(this.radioLowShelf);
            this.filterGroupBox.Controls.Add(this.radioLowPass);
            this.filterGroupBox.Controls.Add(this.radioHighShelf);
            this.filterGroupBox.Controls.Add(this.radioHighPass);
            this.filterGroupBox.Controls.Add(this.radioBandPass);
            this.filterGroupBox.Enabled = false;
            this.filterGroupBox.Location = new System.Drawing.Point(12, 187);
            this.filterGroupBox.Name = "filterGroupBox";
            this.filterGroupBox.Size = new System.Drawing.Size(106, 181);
            this.filterGroupBox.TabIndex = 12;
            this.filterGroupBox.TabStop = false;
            this.filterGroupBox.Text = "Filters";
            // 
            // radioPeak
            // 
            this.radioPeak.AutoSize = true;
            this.radioPeak.Location = new System.Drawing.Point(6, 156);
            this.radioPeak.Name = "radioPeak";
            this.radioPeak.Size = new System.Drawing.Size(50, 17);
            this.radioPeak.TabIndex = 6;
            this.radioPeak.TabStop = true;
            this.radioPeak.Tag = "6";
            this.radioPeak.Text = "Peak";
            this.radioPeak.UseVisualStyleBackColor = true;
            this.radioPeak.CheckedChanged += new System.EventHandler(this.radioFilter_CheckedChanged);
            // 
            // radioBandReject
            // 
            this.radioBandReject.AutoSize = true;
            this.radioBandReject.Location = new System.Drawing.Point(6, 133);
            this.radioBandReject.Name = "radioBandReject";
            this.radioBandReject.Size = new System.Drawing.Size(54, 17);
            this.radioBandReject.TabIndex = 5;
            this.radioBandReject.TabStop = true;
            this.radioBandReject.Tag = "5";
            this.radioBandReject.Text = "Notch";
            this.radioBandReject.UseVisualStyleBackColor = true;
            this.radioBandReject.CheckedChanged += new System.EventHandler(this.radioFilter_CheckedChanged);
            // 
            // radioLowShelf
            // 
            this.radioLowShelf.AutoSize = true;
            this.radioLowShelf.Location = new System.Drawing.Point(6, 110);
            this.radioLowShelf.Name = "radioLowShelf";
            this.radioLowShelf.Size = new System.Drawing.Size(72, 17);
            this.radioLowShelf.TabIndex = 4;
            this.radioLowShelf.TabStop = true;
            this.radioLowShelf.Tag = "4";
            this.radioLowShelf.Text = "Low-Shelf";
            this.radioLowShelf.UseVisualStyleBackColor = true;
            this.radioLowShelf.CheckedChanged += new System.EventHandler(this.radioFilter_CheckedChanged);
            // 
            // radioLowPass
            // 
            this.radioLowPass.AutoSize = true;
            this.radioLowPass.Location = new System.Drawing.Point(6, 88);
            this.radioLowPass.Name = "radioLowPass";
            this.radioLowPass.Size = new System.Drawing.Size(71, 17);
            this.radioLowPass.TabIndex = 3;
            this.radioLowPass.TabStop = true;
            this.radioLowPass.Tag = "3";
            this.radioLowPass.Text = "Low-Pass";
            this.radioLowPass.UseVisualStyleBackColor = true;
            this.radioLowPass.CheckedChanged += new System.EventHandler(this.radioFilter_CheckedChanged);
            // 
            // radioHighShelf
            // 
            this.radioHighShelf.AutoSize = true;
            this.radioHighShelf.Location = new System.Drawing.Point(6, 65);
            this.radioHighShelf.Name = "radioHighShelf";
            this.radioHighShelf.Size = new System.Drawing.Size(74, 17);
            this.radioHighShelf.TabIndex = 2;
            this.radioHighShelf.TabStop = true;
            this.radioHighShelf.Tag = "2";
            this.radioHighShelf.Text = "High-Shelf";
            this.radioHighShelf.UseVisualStyleBackColor = true;
            this.radioHighShelf.CheckedChanged += new System.EventHandler(this.radioFilter_CheckedChanged);
            // 
            // radioHighPass
            // 
            this.radioHighPass.AutoSize = true;
            this.radioHighPass.Location = new System.Drawing.Point(6, 42);
            this.radioHighPass.Name = "radioHighPass";
            this.radioHighPass.Size = new System.Drawing.Size(73, 17);
            this.radioHighPass.TabIndex = 1;
            this.radioHighPass.TabStop = true;
            this.radioHighPass.Tag = "1";
            this.radioHighPass.Text = "High-Pass";
            this.radioHighPass.UseVisualStyleBackColor = true;
            this.radioHighPass.CheckedChanged += new System.EventHandler(this.radioFilter_CheckedChanged);
            // 
            // radioBandPass
            // 
            this.radioBandPass.AutoSize = true;
            this.radioBandPass.Location = new System.Drawing.Point(6, 19);
            this.radioBandPass.Name = "radioBandPass";
            this.radioBandPass.Size = new System.Drawing.Size(76, 17);
            this.radioBandPass.TabIndex = 0;
            this.radioBandPass.TabStop = true;
            this.radioBandPass.Tag = "0";
            this.radioBandPass.Text = "Band-Pass";
            this.radioBandPass.UseVisualStyleBackColor = true;
            this.radioBandPass.CheckedChanged += new System.EventHandler(this.radioFilter_CheckedChanged);
            // 
            // SignalFilterDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 375);
            this.Controls.Add(this.toggleFilterCheck);
            this.Controls.Add(this.FltCtrGroupBox);
            this.Controls.Add(this.filterGroupBox);
            this.Controls.Add(this.InitParamGroupBox);
            this.Name = "SignalFilterDialog";
            this.Text = "SignalFilterDialog";
            this.Load += new System.EventHandler(this.SignalFilterDialog_Load);
            this.InitParamGroupBox.ResumeLayout(false);
            this.InitParamGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInitBndWdt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInitGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInitFreq)).EndInit();
            this.FltCtrGroupBox.ResumeLayout(false);
            this.FltCtrGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BndWdtTracker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gainTracker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.freqTracker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBndWdt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFreq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGain)).EndInit();
            this.filterGroupBox.ResumeLayout(false);
            this.filterGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox toggleFilterCheck;
        private System.Windows.Forms.GroupBox InitParamGroupBox;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.NumericUpDown nudInitBndWdt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudInitGain;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudInitFreq;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox FltCtrGroupBox;
        private System.Windows.Forms.TrackBar BndWdtTracker;
        private System.Windows.Forms.TrackBar gainTracker;
        private System.Windows.Forms.TrackBar freqTracker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudBndWdt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudFreq;
        private System.Windows.Forms.NumericUpDown nudGain;
        private System.Windows.Forms.GroupBox filterGroupBox;
        private System.Windows.Forms.RadioButton radioPeak;
        private System.Windows.Forms.RadioButton radioBandReject;
        private System.Windows.Forms.RadioButton radioLowShelf;
        private System.Windows.Forms.RadioButton radioLowPass;
        private System.Windows.Forms.RadioButton radioHighShelf;
        private System.Windows.Forms.RadioButton radioHighPass;
        private System.Windows.Forms.RadioButton radioBandPass;
    }
}