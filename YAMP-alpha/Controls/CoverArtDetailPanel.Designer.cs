namespace YAMP_alpha.Controls
{
    partial class CoverArtDetailPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tbPicDesc = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.CoverArtBox = new System.Windows.Forms.PictureBox();
            this.CoverArtContextStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.CoverArtBox)).BeginInit();
            this.CoverArtContextStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(194, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Type";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(197, 33);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(236, 21);
            this.comboBox1.TabIndex = 4;
            // 
            // tbPicDesc
            // 
            this.tbPicDesc.Location = new System.Drawing.Point(196, 76);
            this.tbPicDesc.Multiline = true;
            this.tbPicDesc.Name = "tbPicDesc";
            this.tbPicDesc.Size = new System.Drawing.Size(237, 101);
            this.tbPicDesc.TabIndex = 29;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(193, 60);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(60, 13);
            this.label20.TabIndex = 28;
            this.label20.Text = "Description";
            // 
            // CoverArtBox
            // 
            this.CoverArtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CoverArtBox.ContextMenuStrip = this.CoverArtContextStrip;
            this.CoverArtBox.Location = new System.Drawing.Point(16, 17);
            this.CoverArtBox.Name = "CoverArtBox";
            this.CoverArtBox.Size = new System.Drawing.Size(160, 160);
            this.CoverArtBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CoverArtBox.TabIndex = 1;
            this.CoverArtBox.TabStop = false;
            this.CoverArtBox.DoubleClick += new System.EventHandler(this.CoverArtBox_DoubleClick);
            // 
            // CoverArtContextStrip
            // 
            this.CoverArtContextStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.browseToolStripMenuItem});
            this.CoverArtContextStrip.Name = "CoverArtContextStrip";
            this.CoverArtContextStrip.Size = new System.Drawing.Size(113, 48);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // browseToolStripMenuItem
            // 
            this.browseToolStripMenuItem.Name = "browseToolStripMenuItem";
            this.browseToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.browseToolStripMenuItem.Text = "Browse";
            this.browseToolStripMenuItem.Click += new System.EventHandler(this.browseToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(0, 191);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(454, 2);
            this.label2.TabIndex = 30;
            // 
            // CoverArtDetailPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbPicDesc);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CoverArtBox);
            this.Name = "CoverArtDetailPanel";
            this.Size = new System.Drawing.Size(454, 193);
            ((System.ComponentModel.ISupportInitialize)(this.CoverArtBox)).EndInit();
            this.CoverArtContextStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox CoverArtBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox tbPicDesc;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip CoverArtContextStrip;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem browseToolStripMenuItem;
    }
}
