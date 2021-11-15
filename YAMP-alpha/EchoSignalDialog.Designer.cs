namespace YAMP_alpha
{
    partial class EchoSignalDialog
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Btn_UpShift = new System.Windows.Forms.Button();
            this.Btn_DownShift = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Tb_EchoShiftingBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_EchoShiftingBar)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(61, 75);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(27, 20);
            this.textBox1.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Shift by";
            // 
            // Btn_UpShift
            // 
            this.Btn_UpShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_UpShift.Location = new System.Drawing.Point(210, 73);
            this.Btn_UpShift.Name = "Btn_UpShift";
            this.Btn_UpShift.Size = new System.Drawing.Size(47, 23);
            this.Btn_UpShift.TabIndex = 10;
            this.Btn_UpShift.Text = ">";
            this.Btn_UpShift.UseVisualStyleBackColor = true;
            this.Btn_UpShift.Click += new System.EventHandler(this.Btn_UpShift_Click);
            // 
            // Btn_DownShift
            // 
            this.Btn_DownShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_DownShift.Location = new System.Drawing.Point(157, 73);
            this.Btn_DownShift.Name = "Btn_DownShift";
            this.Btn_DownShift.Size = new System.Drawing.Size(47, 23);
            this.Btn_DownShift.TabIndex = 9;
            this.Btn_DownShift.Text = "<";
            this.Btn_DownShift.UseVisualStyleBackColor = true;
            this.Btn_DownShift.Click += new System.EventHandler(this.Btn_DownShift_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Change the value to observe a change in echo";
            // 
            // Tb_EchoShiftingBar
            // 
            this.Tb_EchoShiftingBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tb_EchoShiftingBar.AutoSize = false;
            this.Tb_EchoShiftingBar.Location = new System.Drawing.Point(16, 34);
            this.Tb_EchoShiftingBar.Maximum = 100;
            this.Tb_EchoShiftingBar.Name = "Tb_EchoShiftingBar";
            this.Tb_EchoShiftingBar.Size = new System.Drawing.Size(248, 29);
            this.Tb_EchoShiftingBar.TabIndex = 7;
            this.Tb_EchoShiftingBar.TickFrequency = 0;
            // 
            // EchoSignalDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 106);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Btn_UpShift);
            this.Controls.Add(this.Btn_DownShift);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Tb_EchoShiftingBar);
            this.Name = "EchoSignalDialog";
            this.Text = "EchoSignalDialog";
            this.Load += new System.EventHandler(this.EchoSignalDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Tb_EchoShiftingBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Btn_UpShift;
        private System.Windows.Forms.Button Btn_DownShift;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar Tb_EchoShiftingBar;
    }
}