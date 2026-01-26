namespace YAMP_alpha
{
    partial class PeakMeterDialog
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
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.meterControl1 = new YAMP_alpha.Controls.MeterControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.meterControl2 = new YAMP_alpha.Controls.MeterControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.meterControl3 = new YAMP_alpha.Controls.MeterControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 221);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(183, 20);
            this.button1.TabIndex = 2;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.meterControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(61, 221);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Left";
            // 
            // meterControl1
            // 
            this.meterControl1.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.meterControl1.BackgroundColor = System.Drawing.Color.Black;
            this.meterControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.meterControl1.LEDColor = System.Drawing.Color.DeepSkyBlue;
            this.meterControl1.LEDSize = 1;
            this.meterControl1.Level = 0;
            this.meterControl1.Location = new System.Drawing.Point(3, 16);
            this.meterControl1.Maximum = 100;
            this.meterControl1.Name = "meterControl1";
            this.meterControl1.Size = new System.Drawing.Size(55, 202);
            this.meterControl1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.meterControl2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(61, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(59, 221);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Right";
            // 
            // meterControl2
            // 
            this.meterControl2.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.meterControl2.BackgroundColor = System.Drawing.Color.Black;
            this.meterControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.meterControl2.LEDColor = System.Drawing.Color.Lime;
            this.meterControl2.LEDSize = 1;
            this.meterControl2.Level = 0;
            this.meterControl2.Location = new System.Drawing.Point(3, 16);
            this.meterControl2.Maximum = 100;
            this.meterControl2.Name = "meterControl2";
            this.meterControl2.Size = new System.Drawing.Size(53, 202);
            this.meterControl2.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.meterControl3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox3.Location = new System.Drawing.Point(120, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(63, 221);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Average";
            // 
            // meterControl3
            // 
            this.meterControl3.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.meterControl3.BackgroundColor = System.Drawing.Color.Black;
            this.meterControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.meterControl3.LEDColor = System.Drawing.Color.Firebrick;
            this.meterControl3.LEDSize = 1;
            this.meterControl3.Level = 0;
            this.meterControl3.Location = new System.Drawing.Point(3, 16);
            this.meterControl3.Maximum = 100;
            this.meterControl3.Name = "meterControl3";
            this.meterControl3.Size = new System.Drawing.Size(57, 202);
            this.meterControl3.TabIndex = 1;
            // 
            // PeakMeterDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(183, 241);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PeakMeterDialog";
            this.Text = "PeakMeterDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PeakMeterDialog_FormClosing);
            this.Load += new System.EventHandler(this.PeakMeterDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private Controls.MeterControl meterControl1;
        private Controls.MeterControl meterControl2;
        private Controls.MeterControl meterControl3;
    }
}