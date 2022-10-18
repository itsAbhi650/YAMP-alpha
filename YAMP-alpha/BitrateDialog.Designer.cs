namespace YAMP_alpha
{
    partial class BitrateDialog
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.resampleStatusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.bitrateProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.validBitRateBox = new System.Windows.Forms.ComboBox();
            this.BtnChange = new System.Windows.Forms.Button();
            this.currentBitrateBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnLoad = new System.Windows.Forms.Button();
            this.fileBox = new System.Windows.Forms.TextBox();
            this.Cb_RetainTags = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resampleStatusLbl,
            this.bitrateProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 120);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(319, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 15;
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
            // bitrateProgressBar
            // 
            this.bitrateProgressBar.Name = "bitrateProgressBar";
            this.bitrateProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(106, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Available:";
            // 
            // validBitRateBox
            // 
            this.validBitRateBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.validBitRateBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.validBitRateBox.FormattingEnabled = true;
            this.validBitRateBox.Location = new System.Drawing.Point(109, 53);
            this.validBitRateBox.Name = "validBitRateBox";
            this.validBitRateBox.Size = new System.Drawing.Size(96, 23);
            this.validBitRateBox.TabIndex = 13;
            // 
            // BtnChange
            // 
            this.BtnChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnChange.Location = new System.Drawing.Point(232, 84);
            this.BtnChange.Name = "BtnChange";
            this.BtnChange.Size = new System.Drawing.Size(75, 23);
            this.BtnChange.TabIndex = 12;
            this.BtnChange.Text = "Change";
            this.BtnChange.UseVisualStyleBackColor = true;
            this.BtnChange.Click += new System.EventHandler(this.BtnChange_Click);
            // 
            // currentBitrateBox
            // 
            this.currentBitrateBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentBitrateBox.Location = new System.Drawing.Point(12, 55);
            this.currentBitrateBox.Name = "currentBitrateBox";
            this.currentBitrateBox.ReadOnly = true;
            this.currentBitrateBox.Size = new System.Drawing.Size(69, 21);
            this.currentBitrateBox.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Current:";
            // 
            // BtnLoad
            // 
            this.BtnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnLoad.Location = new System.Drawing.Point(265, 5);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(42, 23);
            this.BtnLoad.TabIndex = 9;
            this.BtnLoad.Text = "Load";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
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
            this.fileBox.TabIndex = 8;
            // 
            // Cb_RetainTags
            // 
            this.Cb_RetainTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cb_RetainTags.AutoSize = true;
            this.Cb_RetainTags.Location = new System.Drawing.Point(13, 90);
            this.Cb_RetainTags.Name = "Cb_RetainTags";
            this.Cb_RetainTags.Size = new System.Drawing.Size(84, 17);
            this.Cb_RetainTags.TabIndex = 16;
            this.Cb_RetainTags.Text = "Retain Tags";
            this.Cb_RetainTags.UseVisualStyleBackColor = true;
            // 
            // BitrateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 142);
            this.Controls.Add(this.Cb_RetainTags);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.validBitRateBox);
            this.Controls.Add(this.BtnChange);
            this.Controls.Add(this.currentBitrateBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnLoad);
            this.Controls.Add(this.fileBox);
            this.Name = "BitrateDialog";
            this.Text = "BitrateDialog";
            this.Load += new System.EventHandler(this.BitrateDialog_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel resampleStatusLbl;
        private System.Windows.Forms.ToolStripProgressBar bitrateProgressBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox validBitRateBox;
        private System.Windows.Forms.Button BtnChange;
        private System.Windows.Forms.TextBox currentBitrateBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnLoad;
        private System.Windows.Forms.TextBox fileBox;
        private System.Windows.Forms.CheckBox Cb_RetainTags;
    }
}