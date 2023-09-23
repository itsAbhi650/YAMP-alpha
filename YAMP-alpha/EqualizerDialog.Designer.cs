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
            this.panel1 = new System.Windows.Forms.Panel();
            this.CmbBx_FftSize = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ChkBx_RollGraph = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.NUD_OffHz = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.CmbBx_ColMap = new System.Windows.Forms.ComboBox();
            this.ChkBx_Dcbl = new System.Windows.Forms.CheckBox();
            this.NUD_Multiplier = new System.Windows.Forms.NumericUpDown();
            this.ChkBx_ResizeSpectro = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.NUD_Brightness = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.CmbBx_ImgMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CmbBx_RotateGraph = new System.Windows.Forms.ComboBox();
            this.Pb_SpectrogramAdv = new System.Windows.Forms.PictureBox();
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
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_OffHz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Multiplier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Brightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_SpectrogramAdv)).BeginInit();
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
            this.splitContainer1.SplitterDistance = 286;
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
            this.tabControl1.Size = new System.Drawing.Size(869, 286);
            this.tabControl1.TabIndex = 0;
            // 
            // TbPg_Scope
            // 
            this.TbPg_Scope.Controls.Add(this.formsPlot1);
            this.TbPg_Scope.Location = new System.Drawing.Point(4, 22);
            this.TbPg_Scope.Name = "TbPg_Scope";
            this.TbPg_Scope.Padding = new System.Windows.Forms.Padding(3);
            this.TbPg_Scope.Size = new System.Drawing.Size(861, 260);
            this.TbPg_Scope.TabIndex = 0;
            this.TbPg_Scope.Text = "Scope";
            this.TbPg_Scope.UseVisualStyleBackColor = true;
            // 
            // formsPlot1
            // 
            this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot1.Location = new System.Drawing.Point(3, 3);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(855, 254);
            this.formsPlot1.TabIndex = 0;
            // 
            // TbPg_Spectrogram
            // 
            this.TbPg_Spectrogram.Controls.Add(this.pictureBox1);
            this.TbPg_Spectrogram.Location = new System.Drawing.Point(4, 22);
            this.TbPg_Spectrogram.Name = "TbPg_Spectrogram";
            this.TbPg_Spectrogram.Padding = new System.Windows.Forms.Padding(3);
            this.TbPg_Spectrogram.Size = new System.Drawing.Size(861, 260);
            this.TbPg_Spectrogram.TabIndex = 1;
            this.TbPg_Spectrogram.Text = "Spectrogram";
            this.TbPg_Spectrogram.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 254);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // TbPg_SpectogramAdv
            // 
            this.TbPg_SpectogramAdv.Controls.Add(this.panel1);
            this.TbPg_SpectogramAdv.Controls.Add(this.Pb_SpectrogramAdv);
            this.TbPg_SpectogramAdv.Location = new System.Drawing.Point(4, 22);
            this.TbPg_SpectogramAdv.Name = "TbPg_SpectogramAdv";
            this.TbPg_SpectogramAdv.Size = new System.Drawing.Size(861, 260);
            this.TbPg_SpectogramAdv.TabIndex = 2;
            this.TbPg_SpectogramAdv.Text = "SpectrogramAdv";
            this.TbPg_SpectogramAdv.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.ChkBx_RollGraph);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.NUD_OffHz);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.CmbBx_ColMap);
            this.panel1.Controls.Add(this.ChkBx_Dcbl);
            this.panel1.Controls.Add(this.NUD_Multiplier);
            this.panel1.Controls.Add(this.ChkBx_ResizeSpectro);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.NUD_Brightness);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.CmbBx_ImgMode);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.CmbBx_RotateGraph);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(733, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(128, 260);
            this.panel1.TabIndex = 17;
            // 
            // CmbBx_FftSize
            // 
            this.CmbBx_FftSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBx_FftSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBx_FftSize.FormattingEnabled = true;
            this.CmbBx_FftSize.Location = new System.Drawing.Point(435, 0);
            this.CmbBx_FftSize.Name = "CmbBx_FftSize";
            this.CmbBx_FftSize.Size = new System.Drawing.Size(95, 20);
            this.CmbBx_FftSize.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(2, 288);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "FftSize";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 301);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(111, 10);
            this.panel2.TabIndex = 17;
            // 
            // ChkBx_RollGraph
            // 
            this.ChkBx_RollGraph.AutoSize = true;
            this.ChkBx_RollGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkBx_RollGraph.Location = new System.Drawing.Point(4, 2);
            this.ChkBx_RollGraph.Name = "ChkBx_RollGraph";
            this.ChkBx_RollGraph.Size = new System.Drawing.Size(75, 17);
            this.ChkBx_RollGraph.TabIndex = 12;
            this.ChkBx_RollGraph.Text = "Roll Graph";
            this.ChkBx_RollGraph.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(2, 250);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "OffsetHz";
            // 
            // NUD_OffHz
            // 
            this.NUD_OffHz.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NUD_OffHz.Location = new System.Drawing.Point(5, 267);
            this.NUD_OffHz.Maximum = new decimal(new int[] {
            32000,
            0,
            0,
            0});
            this.NUD_OffHz.Name = "NUD_OffHz";
            this.NUD_OffHz.Size = new System.Drawing.Size(95, 18);
            this.NUD_OffHz.TabIndex = 15;
            this.NUD_OffHz.ValueChanged += new System.EventHandler(this.NUD_OffHz_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Colormap";
            // 
            // CmbBx_ColMap
            // 
            this.CmbBx_ColMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBx_ColMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBx_ColMap.FormattingEnabled = true;
            this.CmbBx_ColMap.Location = new System.Drawing.Point(5, 76);
            this.CmbBx_ColMap.Name = "CmbBx_ColMap";
            this.CmbBx_ColMap.Size = new System.Drawing.Size(95, 20);
            this.CmbBx_ColMap.TabIndex = 2;
            this.CmbBx_ColMap.SelectedIndexChanged += new System.EventHandler(this.CmbBx_ColMap_SelectedIndexChanged);
            // 
            // ChkBx_Dcbl
            // 
            this.ChkBx_Dcbl.AutoSize = true;
            this.ChkBx_Dcbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkBx_Dcbl.Location = new System.Drawing.Point(4, 38);
            this.ChkBx_Dcbl.Name = "ChkBx_Dcbl";
            this.ChkBx_Dcbl.Size = new System.Drawing.Size(66, 17);
            this.ChkBx_Dcbl.TabIndex = 14;
            this.ChkBx_Dcbl.Text = "Decibels";
            this.ChkBx_Dcbl.UseVisualStyleBackColor = true;
            // 
            // NUD_Multiplier
            // 
            this.NUD_Multiplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NUD_Multiplier.Location = new System.Drawing.Point(5, 115);
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
            this.NUD_Multiplier.Size = new System.Drawing.Size(95, 18);
            this.NUD_Multiplier.TabIndex = 3;
            this.NUD_Multiplier.Value = new decimal(new int[] {
            16000,
            0,
            0,
            0});
            // 
            // ChkBx_ResizeSpectro
            // 
            this.ChkBx_ResizeSpectro.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkBx_ResizeSpectro.Location = new System.Drawing.Point(4, 20);
            this.ChkBx_ResizeSpectro.Name = "ChkBx_ResizeSpectro";
            this.ChkBx_ResizeSpectro.Size = new System.Drawing.Size(105, 17);
            this.ChkBx_ResizeSpectro.TabIndex = 13;
            this.ChkBx_ResizeSpectro.Text = "Adjust OnResize";
            this.ChkBx_ResizeSpectro.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(2, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Multiplier";
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Location = new System.Drawing.Point(4, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 2);
            this.label6.TabIndex = 0;
            // 
            // NUD_Brightness
            // 
            this.NUD_Brightness.DecimalPlaces = 1;
            this.NUD_Brightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NUD_Brightness.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.NUD_Brightness.Location = new System.Drawing.Point(5, 152);
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
            this.NUD_Brightness.Size = new System.Drawing.Size(95, 18);
            this.NUD_Brightness.TabIndex = 5;
            this.NUD_Brightness.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(2, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Brightness";
            // 
            // CmbBx_ImgMode
            // 
            this.CmbBx_ImgMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBx_ImgMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBx_ImgMode.FormattingEnabled = true;
            this.CmbBx_ImgMode.Location = new System.Drawing.Point(5, 228);
            this.CmbBx_ImgMode.Name = "CmbBx_ImgMode";
            this.CmbBx_ImgMode.Size = new System.Drawing.Size(95, 20);
            this.CmbBx_ImgMode.TabIndex = 11;
            this.CmbBx_ImgMode.SelectedIndexChanged += new System.EventHandler(this.CmbBx_ImgMode_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(2, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "RotateFlip";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(2, 212);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Image Mode";
            // 
            // CmbBx_RotateGraph
            // 
            this.CmbBx_RotateGraph.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBx_RotateGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBx_RotateGraph.FormattingEnabled = true;
            this.CmbBx_RotateGraph.Location = new System.Drawing.Point(5, 189);
            this.CmbBx_RotateGraph.Name = "CmbBx_RotateGraph";
            this.CmbBx_RotateGraph.Size = new System.Drawing.Size(95, 20);
            this.CmbBx_RotateGraph.TabIndex = 9;
            // 
            // Pb_SpectrogramAdv
            // 
            this.Pb_SpectrogramAdv.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pb_SpectrogramAdv.Location = new System.Drawing.Point(0, 0);
            this.Pb_SpectrogramAdv.Name = "Pb_SpectrogramAdv";
            this.Pb_SpectrogramAdv.Size = new System.Drawing.Size(733, 260);
            this.Pb_SpectrogramAdv.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Pb_SpectrogramAdv.TabIndex = 7;
            this.Pb_SpectrogramAdv.TabStop = false;
            // 
            // Scope
            // 
            this.Scope.Tick += new System.EventHandler(this.Scope_Tick);
            // 
            // Spectrogram
            // 
            this.Spectrogram.Interval = 40;
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
            this.Controls.Add(this.CmbBx_FftSize);
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_OffHz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Multiplier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Brightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_SpectrogramAdv)).EndInit();
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox CmbBx_FftSize;
        private System.Windows.Forms.Label label8;
    }
}