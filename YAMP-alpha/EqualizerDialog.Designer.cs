namespace YAMP_alpha
{
    partial class EqualizerDialog
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TbPg_Scope = new System.Windows.Forms.TabPage();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.TbPg_Spectrogram = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Scope = new System.Windows.Forms.Timer(this.components);
            this.Spectrogram = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.recenterEQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.TbPg_Scope.SuspendLayout();
            this.TbPg_Spectrogram.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(873, 441);
            this.splitContainer1.SplitterDistance = 217;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TbPg_Scope);
            this.tabControl1.Controls.Add(this.TbPg_Spectrogram);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(873, 217);
            this.tabControl1.TabIndex = 0;
            // 
            // TbPg_Scope
            // 
            this.TbPg_Scope.Controls.Add(this.formsPlot1);
            this.TbPg_Scope.Location = new System.Drawing.Point(4, 22);
            this.TbPg_Scope.Name = "TbPg_Scope";
            this.TbPg_Scope.Padding = new System.Windows.Forms.Padding(3);
            this.TbPg_Scope.Size = new System.Drawing.Size(865, 191);
            this.TbPg_Scope.TabIndex = 0;
            this.TbPg_Scope.Text = "Scope";
            this.TbPg_Scope.UseVisualStyleBackColor = true;
            // 
            // formsPlot1
            // 
            this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot1.Location = new System.Drawing.Point(3, 3);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(859, 185);
            this.formsPlot1.TabIndex = 0;
            // 
            // TbPg_Spectrogram
            // 
            this.TbPg_Spectrogram.Controls.Add(this.pictureBox1);
            this.TbPg_Spectrogram.Location = new System.Drawing.Point(4, 22);
            this.TbPg_Spectrogram.Name = "TbPg_Spectrogram";
            this.TbPg_Spectrogram.Padding = new System.Windows.Forms.Padding(3);
            this.TbPg_Spectrogram.Size = new System.Drawing.Size(865, 191);
            this.TbPg_Spectrogram.TabIndex = 1;
            this.TbPg_Spectrogram.Text = "Spectrogram";
            this.TbPg_Spectrogram.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(859, 185);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Scope
            // 
            this.Scope.Tick += new System.EventHandler(this.Scope_Tick);
            // 
            // Spectrogram
            // 
            this.Spectrogram.Tick += new System.EventHandler(this.Spectrogram_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recenterEQToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(873, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // recenterEQToolStripMenuItem
            // 
            this.recenterEQToolStripMenuItem.Name = "recenterEQToolStripMenuItem";
            this.recenterEQToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.recenterEQToolStripMenuItem.Text = "Recenter EQ";
            this.recenterEQToolStripMenuItem.Click += new System.EventHandler(this.recenterEQToolStripMenuItem_Click);
            // 
            // EqualizerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 465);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EqualizerDialog";
            this.Text = "EqualizerDialog";
            this.Load += new System.EventHandler(this.EqualizerDialog_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.TbPg_Scope.ResumeLayout(false);
            this.TbPg_Spectrogram.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TbPg_Scope;
        private System.Windows.Forms.TabPage TbPg_Spectrogram;
        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer Scope;
        private System.Windows.Forms.Timer Spectrogram;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem recenterEQToolStripMenuItem;
    }
}