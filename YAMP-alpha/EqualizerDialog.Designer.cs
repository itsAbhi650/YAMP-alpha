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
            this.TbPg_SpectogramAdv = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.NUD_OffHz = new System.Windows.Forms.NumericUpDown();
            this.ChkBx_Dcbl = new System.Windows.Forms.CheckBox();
            this.ChkBx_ResizeSpectro = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ChkBx_RollGraph = new System.Windows.Forms.CheckBox();
            this.CmbBx_ImgMode = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CmbBx_RotateGraph = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Pb_SpectrogramAdv = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.NUD_Brightness = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.NUD_Multiplier = new System.Windows.Forms.NumericUpDown();
            this.CmbBx_ColMap = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
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
            this.TbPg_SpectogramAdv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_OffHz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_SpectrogramAdv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Brightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Multiplier)).BeginInit();
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
            this.splitContainer1.Size = new System.Drawing.Size(869, 524);
            this.splitContainer1.SplitterDistance = 324;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TbPg_Scope);
            this.tabControl1.Controls.Add(this.TbPg_Spectrogram);
            this.tabControl1.Controls.Add(this.TbPg_SpectogramAdv);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(869, 324);
            this.tabControl1.TabIndex = 0;
            // 
            // TbPg_Scope
            // 
            this.TbPg_Scope.Controls.Add(this.formsPlot1);
            this.TbPg_Scope.Location = new System.Drawing.Point(4, 22);
            this.TbPg_Scope.Name = "TbPg_Scope";
            this.TbPg_Scope.Padding = new System.Windows.Forms.Padding(3);
            this.TbPg_Scope.Size = new System.Drawing.Size(861, 298);
            this.TbPg_Scope.TabIndex = 0;
            this.TbPg_Scope.Text = "Scope";
            this.TbPg_Scope.UseVisualStyleBackColor = true;
            // 
            // formsPlot1
            // 
            this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot1.Location = new System.Drawing.Point(3, 3);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(855, 292);
            this.formsPlot1.TabIndex = 0;
            // 
            // TbPg_Spectrogram
            // 
            this.TbPg_Spectrogram.Controls.Add(this.pictureBox1);
            this.TbPg_Spectrogram.Location = new System.Drawing.Point(4, 22);
            this.TbPg_Spectrogram.Name = "TbPg_Spectrogram";
            this.TbPg_Spectrogram.Padding = new System.Windows.Forms.Padding(3);
            this.TbPg_Spectrogram.Size = new System.Drawing.Size(861, 298);
            this.TbPg_Spectrogram.TabIndex = 1;
            this.TbPg_Spectrogram.Text = "Spectrogram";
            this.TbPg_Spectrogram.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(855, 292);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // TbPg_SpectogramAdv
            // 
            this.TbPg_SpectogramAdv.Controls.Add(this.label7);
            this.TbPg_SpectogramAdv.Controls.Add(this.NUD_OffHz);
            this.TbPg_SpectogramAdv.Controls.Add(this.ChkBx_Dcbl);
            this.TbPg_SpectogramAdv.Controls.Add(this.ChkBx_ResizeSpectro);
            this.TbPg_SpectogramAdv.Controls.Add(this.label6);
            this.TbPg_SpectogramAdv.Controls.Add(this.ChkBx_RollGraph);
            this.TbPg_SpectogramAdv.Controls.Add(this.CmbBx_ImgMode);
            this.TbPg_SpectogramAdv.Controls.Add(this.label5);
            this.TbPg_SpectogramAdv.Controls.Add(this.CmbBx_RotateGraph);
            this.TbPg_SpectogramAdv.Controls.Add(this.label4);
            this.TbPg_SpectogramAdv.Controls.Add(this.Pb_SpectrogramAdv);
            this.TbPg_SpectogramAdv.Controls.Add(this.label3);
            this.TbPg_SpectogramAdv.Controls.Add(this.NUD_Brightness);
            this.TbPg_SpectogramAdv.Controls.Add(this.label2);
            this.TbPg_SpectogramAdv.Controls.Add(this.NUD_Multiplier);
            this.TbPg_SpectogramAdv.Controls.Add(this.CmbBx_ColMap);
            this.TbPg_SpectogramAdv.Controls.Add(this.label1);
            this.TbPg_SpectogramAdv.Location = new System.Drawing.Point(4, 22);
            this.TbPg_SpectogramAdv.Name = "TbPg_SpectogramAdv";
            this.TbPg_SpectogramAdv.Size = new System.Drawing.Size(861, 298);
            this.TbPg_SpectogramAdv.TabIndex = 2;
            this.TbPg_SpectogramAdv.Text = "SpectrogramAdv";
            this.TbPg_SpectogramAdv.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(758, 258);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "OffsetHz";
            // 
            // NUD_OffHz
            // 
            this.NUD_OffHz.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NUD_OffHz.Location = new System.Drawing.Point(761, 275);
            this.NUD_OffHz.Maximum = new decimal(new int[] {
            32000,
            0,
            0,
            0});
            this.NUD_OffHz.Name = "NUD_OffHz";
            this.NUD_OffHz.Size = new System.Drawing.Size(95, 20);
            this.NUD_OffHz.TabIndex = 15;
            this.NUD_OffHz.ValueChanged += new System.EventHandler(this.NUD_OffHz_ValueChanged);
            // 
            // ChkBx_Dcbl
            // 
            this.ChkBx_Dcbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChkBx_Dcbl.AutoSize = true;
            this.ChkBx_Dcbl.Location = new System.Drawing.Point(758, 41);
            this.ChkBx_Dcbl.Name = "ChkBx_Dcbl";
            this.ChkBx_Dcbl.Size = new System.Drawing.Size(67, 17);
            this.ChkBx_Dcbl.TabIndex = 14;
            this.ChkBx_Dcbl.Text = "Decibels";
            this.ChkBx_Dcbl.UseVisualStyleBackColor = true;
            // 
            // ChkBx_ResizeSpectro
            // 
            this.ChkBx_ResizeSpectro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChkBx_ResizeSpectro.AutoSize = true;
            this.ChkBx_ResizeSpectro.Location = new System.Drawing.Point(758, 23);
            this.ChkBx_ResizeSpectro.Name = "ChkBx_ResizeSpectro";
            this.ChkBx_ResizeSpectro.Size = new System.Drawing.Size(104, 17);
            this.ChkBx_ResizeSpectro.TabIndex = 13;
            this.ChkBx_ResizeSpectro.Text = "Adjust OnResize";
            this.ChkBx_ResizeSpectro.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Location = new System.Drawing.Point(758, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 2);
            this.label6.TabIndex = 0;
            // 
            // ChkBx_RollGraph
            // 
            this.ChkBx_RollGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChkBx_RollGraph.AutoSize = true;
            this.ChkBx_RollGraph.Location = new System.Drawing.Point(758, 5);
            this.ChkBx_RollGraph.Name = "ChkBx_RollGraph";
            this.ChkBx_RollGraph.Size = new System.Drawing.Size(76, 17);
            this.ChkBx_RollGraph.TabIndex = 12;
            this.ChkBx_RollGraph.Text = "Roll Graph";
            this.ChkBx_RollGraph.UseVisualStyleBackColor = true;
            // 
            // CmbBx_ImgMode
            // 
            this.CmbBx_ImgMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbBx_ImgMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBx_ImgMode.FormattingEnabled = true;
            this.CmbBx_ImgMode.Location = new System.Drawing.Point(761, 235);
            this.CmbBx_ImgMode.Name = "CmbBx_ImgMode";
            this.CmbBx_ImgMode.Size = new System.Drawing.Size(95, 21);
            this.CmbBx_ImgMode.TabIndex = 11;
            this.CmbBx_ImgMode.SelectedIndexChanged += new System.EventHandler(this.CmbBx_ImgMode_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(758, 219);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Image Mode";
            // 
            // CmbBx_RotateGraph
            // 
            this.CmbBx_RotateGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbBx_RotateGraph.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBx_RotateGraph.FormattingEnabled = true;
            this.CmbBx_RotateGraph.Location = new System.Drawing.Point(761, 196);
            this.CmbBx_RotateGraph.Name = "CmbBx_RotateGraph";
            this.CmbBx_RotateGraph.Size = new System.Drawing.Size(95, 21);
            this.CmbBx_RotateGraph.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(758, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "RotateFlip";
            // 
            // Pb_SpectrogramAdv
            // 
            this.Pb_SpectrogramAdv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Pb_SpectrogramAdv.Location = new System.Drawing.Point(3, 3);
            this.Pb_SpectrogramAdv.Name = "Pb_SpectrogramAdv";
            this.Pb_SpectrogramAdv.Size = new System.Drawing.Size(752, 292);
            this.Pb_SpectrogramAdv.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Pb_SpectrogramAdv.TabIndex = 7;
            this.Pb_SpectrogramAdv.TabStop = false;
//            this.Pb_SpectrogramAdv.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(758, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Brightness";
            // 
            // NUD_Brightness
            // 
            this.NUD_Brightness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NUD_Brightness.DecimalPlaces = 1;
            this.NUD_Brightness.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.NUD_Brightness.Location = new System.Drawing.Point(761, 158);
            this.NUD_Brightness.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NUD_Brightness.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUD_Brightness.Name = "NUD_Brightness";
            this.NUD_Brightness.Size = new System.Drawing.Size(95, 20);
            this.NUD_Brightness.TabIndex = 5;
            this.NUD_Brightness.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(758, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Multiplier";
            // 
            // NUD_Multiplier
            // 
            this.NUD_Multiplier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NUD_Multiplier.Location = new System.Drawing.Point(761, 119);
            this.NUD_Multiplier.Maximum = new decimal(new int[] {
            32000,
            0,
            0,
            0});
            this.NUD_Multiplier.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUD_Multiplier.Name = "NUD_Multiplier";
            this.NUD_Multiplier.Size = new System.Drawing.Size(95, 20);
            this.NUD_Multiplier.TabIndex = 3;
            this.NUD_Multiplier.Value = new decimal(new int[] {
            16000,
            0,
            0,
            0});
            // 
            // CmbBx_ColMap
            // 
            this.CmbBx_ColMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbBx_ColMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBx_ColMap.FormattingEnabled = true;
            this.CmbBx_ColMap.Location = new System.Drawing.Point(761, 79);
            this.CmbBx_ColMap.Name = "CmbBx_ColMap";
            this.CmbBx_ColMap.Size = new System.Drawing.Size(95, 21);
            this.CmbBx_ColMap.TabIndex = 2;
            this.CmbBx_ColMap.SelectedIndexChanged += new System.EventHandler(this.CmbBx_ColMap_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(758, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Colormap";
            // 
            // Scope
            // 
            this.Scope.Tick += new System.EventHandler(this.Scope_Tick);
            // 
            // Spectrogram
            // 
            this.Spectrogram.Interval = 1;
            this.Spectrogram.Tick += new System.EventHandler(this.Spectrogram_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recenterEQToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(869, 24);
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
            this.ClientSize = new System.Drawing.Size(869, 548);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EqualizerDialog";
            this.Text = "EqualizerDialog";
            this.Load += new System.EventHandler(this.EqualizerDialog_Load);
            this.SizeChanged += new System.EventHandler(this.EqualizerDialog_SizeChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.TbPg_Scope.ResumeLayout(false);
            this.TbPg_Spectrogram.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.TbPg_SpectogramAdv.ResumeLayout(false);
            this.TbPg_SpectogramAdv.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_OffHz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_SpectrogramAdv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Brightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Multiplier)).EndInit();
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
        private System.Windows.Forms.TabPage TbPg_SpectogramAdv;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NUD_Brightness;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NUD_Multiplier;
        private System.Windows.Forms.ComboBox CmbBx_ColMap;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox Pb_SpectrogramAdv;
        private System.Windows.Forms.ComboBox CmbBx_RotateGraph;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CmbBx_ImgMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox ChkBx_RollGraph;
        private System.Windows.Forms.CheckBox ChkBx_ResizeSpectro;
        private System.Windows.Forms.CheckBox ChkBx_Dcbl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown NUD_OffHz;
    }
}