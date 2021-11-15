namespace YAMP_alpha
{
    partial class PitchShiftDialog
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
            this.Tb_PitchShiftingBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_DownShift = new System.Windows.Forms.Button();
            this.Btn_UpShift = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_PitchShiftingBar)).BeginInit();
            this.SuspendLayout();
            // 
            // Tb_PitchShiftingBar
            // 
            this.Tb_PitchShiftingBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tb_PitchShiftingBar.AutoSize = false;
            this.Tb_PitchShiftingBar.Location = new System.Drawing.Point(12, 32);
            this.Tb_PitchShiftingBar.Maximum = 100;
            this.Tb_PitchShiftingBar.Minimum = -100;
            this.Tb_PitchShiftingBar.Name = "Tb_PitchShiftingBar";
            this.Tb_PitchShiftingBar.Size = new System.Drawing.Size(248, 29);
            this.Tb_PitchShiftingBar.TabIndex = 0;
            this.Tb_PitchShiftingBar.TickFrequency = 0;
            this.Tb_PitchShiftingBar.Value = -100;
            this.Tb_PitchShiftingBar.ValueChanged += new System.EventHandler(this.Tb_PitchShiftingBar_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Change the value to observe a change in pitch";
            // 
            // Btn_DownShift
            // 
            this.Btn_DownShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_DownShift.Location = new System.Drawing.Point(153, 71);
            this.Btn_DownShift.Name = "Btn_DownShift";
            this.Btn_DownShift.Size = new System.Drawing.Size(47, 23);
            this.Btn_DownShift.TabIndex = 3;
            this.Btn_DownShift.Text = "<";
            this.Btn_DownShift.UseVisualStyleBackColor = true;
            this.Btn_DownShift.Click += new System.EventHandler(this.Btn_DownShift_Click);
            // 
            // Btn_UpShift
            // 
            this.Btn_UpShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_UpShift.Location = new System.Drawing.Point(206, 71);
            this.Btn_UpShift.Name = "Btn_UpShift";
            this.Btn_UpShift.Size = new System.Drawing.Size(47, 23);
            this.Btn_UpShift.TabIndex = 4;
            this.Btn_UpShift.Text = ">";
            this.Btn_UpShift.UseVisualStyleBackColor = true;
            this.Btn_UpShift.Click += new System.EventHandler(this.Btn_UpShift_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Shift by";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(57, 73);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(27, 20);
            this.textBox1.TabIndex = 6;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // PitchShiftDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 106);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Btn_UpShift);
            this.Controls.Add(this.Btn_DownShift);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Tb_PitchShiftingBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "PitchShiftDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PitchShiftDialog";
            this.Load += new System.EventHandler(this.PitchShiftDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Tb_PitchShiftingBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar Tb_PitchShiftingBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_DownShift;
        private System.Windows.Forms.Button Btn_UpShift;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
    }
}