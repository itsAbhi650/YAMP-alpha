namespace YAMP_alpha
{
    partial class NewMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewMain));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_PlayNext = new System.Windows.Forms.Button();
            this.Btn_SkipSecFwrd = new System.Windows.Forms.Button();
            this.Btn_SkipSecBack = new System.Windows.Forms.Button();
            this.Btn_PlayPrev = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.VolumeTracker = new System.Windows.Forms.TrackBar();
            this.DurationTracker = new System.Windows.Forms.TrackBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Lbl_PlayerLabel = new System.Windows.Forms.Label();
            this.Lbl_Duration = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadFileStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadDirStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.ExitStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.peakMtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waveformNAudioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vUMeterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.effectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pitchShifterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.echoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gargleEffectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flangerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chorusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wavesReverbToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.equalizerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.distortionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeSampleRateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PlayTimer = new System.Windows.Forms.Timer(this.components);
            this.waveformPainter1 = new NAudio.Gui.WaveformPainter();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeTracker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DurationTracker)).BeginInit();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.Btn_PlayNext);
            this.panel1.Controls.Add(this.Btn_SkipSecFwrd);
            this.panel1.Controls.Add(this.Btn_SkipSecBack);
            this.panel1.Controls.Add(this.Btn_PlayPrev);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.VolumeTracker);
            this.panel1.Controls.Add(this.DurationTracker);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 331);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(384, 95);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Location = new System.Drawing.Point(227, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(2, 29);
            this.label2.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Location = new System.Drawing.Point(99, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(2, 29);
            this.label1.TabIndex = 11;
            // 
            // Btn_PlayNext
            // 
            this.Btn_PlayNext.Location = new System.Drawing.Point(194, 34);
            this.Btn_PlayNext.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_PlayNext.Name = "Btn_PlayNext";
            this.Btn_PlayNext.Size = new System.Drawing.Size(30, 30);
            this.Btn_PlayNext.TabIndex = 10;
            this.Btn_PlayNext.Tag = "1";
            this.Btn_PlayNext.Text = ">>";
            this.Btn_PlayNext.UseVisualStyleBackColor = true;
            this.Btn_PlayNext.Click += new System.EventHandler(this.Btns_TrackShift_Click);
            // 
            // Btn_SkipSecFwrd
            // 
            this.Btn_SkipSecFwrd.Location = new System.Drawing.Point(164, 34);
            this.Btn_SkipSecFwrd.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_SkipSecFwrd.Name = "Btn_SkipSecFwrd";
            this.Btn_SkipSecFwrd.Size = new System.Drawing.Size(30, 30);
            this.Btn_SkipSecFwrd.TabIndex = 9;
            this.Btn_SkipSecFwrd.Tag = "5";
            this.Btn_SkipSecFwrd.Text = "->";
            this.Btn_SkipSecFwrd.UseVisualStyleBackColor = true;
            this.Btn_SkipSecFwrd.Click += new System.EventHandler(this.BtnSkipSec_Click);
            // 
            // Btn_SkipSecBack
            // 
            this.Btn_SkipSecBack.Location = new System.Drawing.Point(134, 34);
            this.Btn_SkipSecBack.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_SkipSecBack.Name = "Btn_SkipSecBack";
            this.Btn_SkipSecBack.Size = new System.Drawing.Size(30, 30);
            this.Btn_SkipSecBack.TabIndex = 8;
            this.Btn_SkipSecBack.Tag = "-5";
            this.Btn_SkipSecBack.Text = "<-";
            this.Btn_SkipSecBack.UseVisualStyleBackColor = true;
            this.Btn_SkipSecBack.Click += new System.EventHandler(this.BtnSkipSec_Click);
            // 
            // Btn_PlayPrev
            // 
            this.Btn_PlayPrev.Location = new System.Drawing.Point(104, 34);
            this.Btn_PlayPrev.Margin = new System.Windows.Forms.Padding(0);
            this.Btn_PlayPrev.Name = "Btn_PlayPrev";
            this.Btn_PlayPrev.Size = new System.Drawing.Size(30, 30);
            this.Btn_PlayPrev.TabIndex = 7;
            this.Btn_PlayPrev.Tag = "-1";
            this.Btn_PlayPrev.Text = "<<";
            this.Btn_PlayPrev.UseVisualStyleBackColor = true;
            this.Btn_PlayPrev.Click += new System.EventHandler(this.Btns_TrackShift_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(66, 34);
            this.button4.Margin = new System.Windows.Forms.Padding(0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(30, 30);
            this.button4.TabIndex = 6;
            this.button4.Text = "[ ]";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(36, 34);
            this.button3.Margin = new System.Windows.Forms.Padding(0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(30, 30);
            this.button3.TabIndex = 5;
            this.button3.Text = "II";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 34);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(30, 30);
            this.button2.TabIndex = 3;
            this.button2.Text = ">";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(258, 38);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(41, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Mute";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // VolumeTracker
            // 
            this.VolumeTracker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VolumeTracker.AutoSize = false;
            this.VolumeTracker.LargeChange = 0;
            this.VolumeTracker.Location = new System.Drawing.Point(305, 38);
            this.VolumeTracker.Maximum = 100;
            this.VolumeTracker.Name = "VolumeTracker";
            this.VolumeTracker.Size = new System.Drawing.Size(76, 22);
            this.VolumeTracker.TabIndex = 3;
            this.VolumeTracker.TickFrequency = 0;
            this.VolumeTracker.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // DurationTracker
            // 
            this.DurationTracker.AutoSize = false;
            this.DurationTracker.Dock = System.Windows.Forms.DockStyle.Top;
            this.DurationTracker.Location = new System.Drawing.Point(0, 0);
            this.DurationTracker.Name = "DurationTracker";
            this.DurationTracker.Size = new System.Drawing.Size(384, 27);
            this.DurationTracker.TabIndex = 2;
            this.DurationTracker.TickFrequency = 0;
            this.DurationTracker.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.Lbl_PlayerLabel);
            this.panel2.Controls.Add(this.Lbl_Duration);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 67);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(384, 28);
            this.panel2.TabIndex = 1;
            // 
            // Lbl_PlayerLabel
            // 
            this.Lbl_PlayerLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Lbl_PlayerLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Lbl_PlayerLabel.Location = new System.Drawing.Point(3, 3);
            this.Lbl_PlayerLabel.Name = "Lbl_PlayerLabel";
            this.Lbl_PlayerLabel.Size = new System.Drawing.Size(288, 22);
            this.Lbl_PlayerLabel.TabIndex = 0;
            this.Lbl_PlayerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Lbl_Duration
            // 
            this.Lbl_Duration.Dock = System.Windows.Forms.DockStyle.Right;
            this.Lbl_Duration.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Lbl_Duration.Location = new System.Drawing.Point(291, 3);
            this.Lbl_Duration.Name = "Lbl_Duration";
            this.Lbl_Duration.Size = new System.Drawing.Size(90, 22);
            this.Lbl_Duration.TabIndex = 1;
            this.Lbl_Duration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.effectsToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip1.Size = new System.Drawing.Size(384, 19);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadFileStripMenuItem,
            this.LoadDirStripMenuItem,
            this.toolStripSeparator,
            this.ExitStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(29, 19);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // LoadFileStripMenuItem
            // 
            this.LoadFileStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("LoadFileStripMenuItem.Image")));
            this.LoadFileStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LoadFileStripMenuItem.Name = "LoadFileStripMenuItem";
            this.LoadFileStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.LoadFileStripMenuItem.Text = "Load File";
            this.LoadFileStripMenuItem.Click += new System.EventHandler(this.LoadFileStripMenuItem_Click);
            // 
            // LoadDirStripMenuItem
            // 
            this.LoadDirStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("LoadDirStripMenuItem.Image")));
            this.LoadDirStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LoadDirStripMenuItem.Name = "LoadDirStripMenuItem";
            this.LoadDirStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.LoadDirStripMenuItem.Text = "Load Directory";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(148, 6);
            // 
            // ExitStripMenuItem
            // 
            this.ExitStripMenuItem.Name = "ExitStripMenuItem";
            this.ExitStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.ExitStripMenuItem.Text = "E&xit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playlistToolStripMenuItem,
            this.peakMtToolStripMenuItem,
            this.waveformNAudioToolStripMenuItem,
            this.vUMeterToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 19);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // playlistToolStripMenuItem
            // 
            this.playlistToolStripMenuItem.Name = "playlistToolStripMenuItem";
            this.playlistToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.playlistToolStripMenuItem.Text = "Playlist";
            this.playlistToolStripMenuItem.Click += new System.EventHandler(this.playlistToolStripMenuItem_Click);
            // 
            // peakMtToolStripMenuItem
            // 
            this.peakMtToolStripMenuItem.Name = "peakMtToolStripMenuItem";
            this.peakMtToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.peakMtToolStripMenuItem.Text = "Peak Meter";
            this.peakMtToolStripMenuItem.Click += new System.EventHandler(this.peakMtToolStripMenuItem_Click);
            // 
            // waveformNAudioToolStripMenuItem
            // 
            this.waveformNAudioToolStripMenuItem.Name = "waveformNAudioToolStripMenuItem";
            this.waveformNAudioToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.waveformNAudioToolStripMenuItem.Text = "Waveform (NAudio)";
            this.waveformNAudioToolStripMenuItem.Click += new System.EventHandler(this.waveformNAudioToolStripMenuItem_Click);
            // 
            // vUMeterToolStripMenuItem
            // 
            this.vUMeterToolStripMenuItem.Name = "vUMeterToolStripMenuItem";
            this.vUMeterToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.vUMeterToolStripMenuItem.Text = "VU Meter";
            this.vUMeterToolStripMenuItem.Click += new System.EventHandler(this.vUMeterToolStripMenuItem_Click);
            // 
            // effectsToolStripMenuItem
            // 
            this.effectsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pitchShifterToolStripMenuItem,
            this.echoToolStripMenuItem,
            this.gargleEffectToolStripMenuItem,
            this.flangerToolStripMenuItem,
            this.chorusToolStripMenuItem,
            this.compressorToolStripMenuItem,
            this.wavesReverbToolStripMenuItem,
            this.equalizerToolStripMenuItem,
            this.distortionToolStripMenuItem});
            this.effectsToolStripMenuItem.Name = "effectsToolStripMenuItem";
            this.effectsToolStripMenuItem.Size = new System.Drawing.Size(54, 19);
            this.effectsToolStripMenuItem.Text = "Effects";
            // 
            // pitchShifterToolStripMenuItem
            // 
            this.pitchShifterToolStripMenuItem.Name = "pitchShifterToolStripMenuItem";
            this.pitchShifterToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.pitchShifterToolStripMenuItem.Text = "Pitch Shifter";
            this.pitchShifterToolStripMenuItem.Click += new System.EventHandler(this.pitchShifterToolStripMenuItem_Click);
            // 
            // echoToolStripMenuItem
            // 
            this.echoToolStripMenuItem.Name = "echoToolStripMenuItem";
            this.echoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.echoToolStripMenuItem.Text = "Echo";
            this.echoToolStripMenuItem.Click += new System.EventHandler(this.echoToolStripMenuItem_Click);
            // 
            // gargleEffectToolStripMenuItem
            // 
            this.gargleEffectToolStripMenuItem.Name = "gargleEffectToolStripMenuItem";
            this.gargleEffectToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.gargleEffectToolStripMenuItem.Text = "Gargle";
            this.gargleEffectToolStripMenuItem.Click += new System.EventHandler(this.gargleEffectToolStripMenuItem_Click);
            // 
            // flangerToolStripMenuItem
            // 
            this.flangerToolStripMenuItem.Name = "flangerToolStripMenuItem";
            this.flangerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.flangerToolStripMenuItem.Text = "Flanger";
            this.flangerToolStripMenuItem.Click += new System.EventHandler(this.flangerToolStripMenuItem_Click);
            // 
            // chorusToolStripMenuItem
            // 
            this.chorusToolStripMenuItem.Name = "chorusToolStripMenuItem";
            this.chorusToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.chorusToolStripMenuItem.Text = "Chorus";
            this.chorusToolStripMenuItem.Click += new System.EventHandler(this.chorusToolStripMenuItem_Click);
            // 
            // compressorToolStripMenuItem
            // 
            this.compressorToolStripMenuItem.Name = "compressorToolStripMenuItem";
            this.compressorToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.compressorToolStripMenuItem.Text = "Compressor";
            this.compressorToolStripMenuItem.Click += new System.EventHandler(this.compressorToolStripMenuItem_Click);
            // 
            // wavesReverbToolStripMenuItem
            // 
            this.wavesReverbToolStripMenuItem.Name = "wavesReverbToolStripMenuItem";
            this.wavesReverbToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.wavesReverbToolStripMenuItem.Text = "Waves Reverb";
            this.wavesReverbToolStripMenuItem.Click += new System.EventHandler(this.wavesReverbToolStripMenuItem_Click);
            // 
            // equalizerToolStripMenuItem
            // 
            this.equalizerToolStripMenuItem.Name = "equalizerToolStripMenuItem";
            this.equalizerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.equalizerToolStripMenuItem.Text = "Equalizer";
            this.equalizerToolStripMenuItem.Click += new System.EventHandler(this.equalizerToolStripMenuItem_Click);
            // 
            // distortionToolStripMenuItem
            // 
            this.distortionToolStripMenuItem.Name = "distortionToolStripMenuItem";
            this.distortionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.distortionToolStripMenuItem.Text = "Distortion";
            this.distortionToolStripMenuItem.Click += new System.EventHandler(this.distortionToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeSampleRateToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 19);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // changeSampleRateToolStripMenuItem
            // 
            this.changeSampleRateToolStripMenuItem.Name = "changeSampleRateToolStripMenuItem";
            this.changeSampleRateToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.changeSampleRateToolStripMenuItem.Text = "Change sample rate";
            this.changeSampleRateToolStripMenuItem.Click += new System.EventHandler(this.changeSampleRateToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(384, 283);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // PlayTimer
            // 
            this.PlayTimer.Tick += new System.EventHandler(this.PlayTimer_Tick);
            // 
            // waveformPainter1
            // 
            this.waveformPainter1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.waveformPainter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.waveformPainter1.Location = new System.Drawing.Point(0, 0);
            this.waveformPainter1.Name = "waveformPainter1";
            this.waveformPainter1.Size = new System.Drawing.Size(384, 25);
            this.waveformPainter1.TabIndex = 3;
            this.waveformPainter1.Text = "waveformPainter1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 19);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.waveformPainter1);
            this.splitContainer1.Size = new System.Drawing.Size(384, 312);
            this.splitContainer1.SplitterDistance = 283;
            this.splitContainer1.TabIndex = 4;
            // 
            // NewMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 426);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "NewMain";
            this.Text = "NewMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewMain_FormClosing);
            this.Load += new System.EventHandler(this.NewMain_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VolumeTracker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DurationTracker)).EndInit();
            this.panel2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadFileStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadDirStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem ExitStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_PlayNext;
        private System.Windows.Forms.Button Btn_SkipSecFwrd;
        private System.Windows.Forms.Button Btn_SkipSecBack;
        private System.Windows.Forms.Button Btn_PlayPrev;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TrackBar VolumeTracker;
        private System.Windows.Forms.TrackBar DurationTracker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer PlayTimer;
        private System.Windows.Forms.ToolStripMenuItem effectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pitchShifterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem echoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem peakMtToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gargleEffectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flangerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chorusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compressorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wavesReverbToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeSampleRateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waveformNAudioToolStripMenuItem;
        private NAudio.Gui.WaveformPainter waveformPainter1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem vUMeterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem equalizerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playlistToolStripMenuItem;
        private System.Windows.Forms.Label Lbl_PlayerLabel;
        private System.Windows.Forms.Label Lbl_Duration;
        private System.Windows.Forms.ToolStripMenuItem distortionToolStripMenuItem;
    }
}