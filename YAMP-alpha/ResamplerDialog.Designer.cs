namespace YAMP_alpha
{
    partial class ResamplerDialog
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
            this.fileBox = new System.Windows.Forms.TextBox();
            this.BtnLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.currentSampleBox = new System.Windows.Forms.TextBox();
            this.BtnResample = new System.Windows.Forms.Button();
            this.validSampleBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.resampleStatusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.resampleProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileBox
            // 
            this.fileBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileBox.Location = new System.Drawing.Point(12, 6);
            this.fileBox.Name = "fileBox";
            this.fileBox.ReadOnly = true;
            this.fileBox.Size = new System.Drawing.Size(247, 21);
            this.fileBox.TabIndex = 0;
            // 
            // BtnLoad
            // 
            this.BtnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnLoad.Location = new System.Drawing.Point(265, 5);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(42, 23);
            this.BtnLoad.TabIndex = 1;
            this.BtnLoad.Text = "Load";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Current:";
            // 
            // currentSampleBox
            // 
            this.currentSampleBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentSampleBox.Location = new System.Drawing.Point(12, 55);
            this.currentSampleBox.Name = "currentSampleBox";
            this.currentSampleBox.ReadOnly = true;
            this.currentSampleBox.Size = new System.Drawing.Size(69, 21);
            this.currentSampleBox.TabIndex = 3;
            // 
            // BtnResample
            // 
            this.BtnResample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnResample.Location = new System.Drawing.Point(232, 52);
            this.BtnResample.Name = "BtnResample";
            this.BtnResample.Size = new System.Drawing.Size(75, 23);
            this.BtnResample.TabIndex = 4;
            this.BtnResample.Text = "Resample";
            this.BtnResample.UseVisualStyleBackColor = true;
            this.BtnResample.Click += new System.EventHandler(this.BtnResample_Click);
            // 
            // validSampleBox
            // 
            this.validSampleBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.validSampleBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.validSampleBox.FormattingEnabled = true;
            this.validSampleBox.Location = new System.Drawing.Point(109, 53);
            this.validSampleBox.Name = "validSampleBox";
            this.validSampleBox.Size = new System.Drawing.Size(96, 23);
            this.validSampleBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(106, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Available:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resampleStatusLbl,
            this.resampleProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 88);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(319, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // resampleStatusLbl
            // 
            this.resampleStatusLbl.AutoSize = false;
            this.resampleStatusLbl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.resampleStatusLbl.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.resampleStatusLbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.resampleStatusLbl.Name = "resampleStatusLbl";
            this.resampleStatusLbl.Size = new System.Drawing.Size(85, 17);
            this.resampleStatusLbl.Text = "Status";
            // 
            // resampleProgressBar
            // 
            this.resampleProgressBar.Name = "resampleProgressBar";
            this.resampleProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // ResamplerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 110);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.validSampleBox);
            this.Controls.Add(this.BtnResample);
            this.Controls.Add(this.currentSampleBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnLoad);
            this.Controls.Add(this.fileBox);
            this.Name = "ResamplerDialog";
            this.Text = "ResamplerDialog";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fileBox;
        private System.Windows.Forms.Button BtnLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox currentSampleBox;
        private System.Windows.Forms.Button BtnResample;
        private System.Windows.Forms.ComboBox validSampleBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel resampleStatusLbl;
        private System.Windows.Forms.ToolStripProgressBar resampleProgressBar;
    }
}