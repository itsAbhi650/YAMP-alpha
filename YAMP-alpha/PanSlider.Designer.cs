namespace YAMP_alpha
{
    partial class PanSlider
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
            this.Tb_PanSlider = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Pnl_PanBackPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_PanSlider)).BeginInit();
            this.Pnl_PanBackPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tb_PanSlider
            // 
            this.Tb_PanSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_PanSlider.Location = new System.Drawing.Point(23, 1);
            this.Tb_PanSlider.Minimum = -10;
            this.Tb_PanSlider.Name = "Tb_PanSlider";
            this.Tb_PanSlider.Size = new System.Drawing.Size(110, 36);
            this.Tb_PanSlider.TabIndex = 0;
            this.Tb_PanSlider.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.Tb_PanSlider.ValueChanged += new System.EventHandler(this.Tb_PanSlider_ValueChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 36);
            this.label1.TabIndex = 1;
            this.label1.Text = "L";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(133, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 36);
            this.label2.TabIndex = 2;
            this.label2.Text = "R";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Pnl_PanBackPanel
            // 
            this.Pnl_PanBackPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pnl_PanBackPanel.Controls.Add(this.Tb_PanSlider);
            this.Pnl_PanBackPanel.Controls.Add(this.label1);
            this.Pnl_PanBackPanel.Controls.Add(this.label2);
            this.Pnl_PanBackPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pnl_PanBackPanel.Location = new System.Drawing.Point(0, 0);
            this.Pnl_PanBackPanel.Name = "Pnl_PanBackPanel";
            this.Pnl_PanBackPanel.Padding = new System.Windows.Forms.Padding(1);
            this.Pnl_PanBackPanel.Size = new System.Drawing.Size(158, 40);
            this.Pnl_PanBackPanel.TabIndex = 3;
            // 
            // PanSlider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(158, 40);
            this.Controls.Add(this.Pnl_PanBackPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PanSlider";
            this.Text = "PanSlider";
            this.Load += new System.EventHandler(this.PanSlider_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Tb_PanSlider)).EndInit();
            this.Pnl_PanBackPanel.ResumeLayout(false);
            this.Pnl_PanBackPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar Tb_PanSlider;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel Pnl_PanBackPanel;
    }
}