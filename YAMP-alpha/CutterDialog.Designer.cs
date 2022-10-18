namespace YAMP_alpha
{
    partial class CutterDialog
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
            this.button1 = new System.Windows.Forms.Button();
            this.TrackSeekBar = new DoubleTrackBar.DoubleTrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnLoadAudio = new System.Windows.Forms.Button();
            this.TrackPathBox = new System.Windows.Forms.TextBox();
            this.Cb_RetainTags = new System.Windows.Forms.CheckBox();
            this.BtnMediaInfo = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(228, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cut";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TrackSeekBar
            // 
            this.TrackSeekBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackSeekBar.BorderColor = System.Drawing.Color.DarkGray;
            this.TrackSeekBar.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.TrackSeekBar.LeftThumbDirection = DoubleTrackBar.DoubleTrackBar.ThumbDirection.Bottom;
            this.TrackSeekBar.Location = new System.Drawing.Point(12, 38);
            this.TrackSeekBar.Maximum = 50;
            this.TrackSeekBar.Minimum = 0;
            this.TrackSeekBar.Name = "TrackSeekBar";
            this.TrackSeekBar.RightThumbDirection = DoubleTrackBar.DoubleTrackBar.ThumbDirection.Bottom;
            this.TrackSeekBar.Size = new System.Drawing.Size(290, 46);
            this.TrackSeekBar.SmallChange = 1;
            this.TrackSeekBar.TabIndex = 2;
            this.TrackSeekBar.ThumbColor = System.Drawing.Color.Transparent;
            this.TrackSeekBar.TickEdgeStyle = System.Windows.Forms.VisualStyles.EdgeStyle.Raised;
            this.TrackSeekBar.TickStyle = DoubleTrackBar.DoubleTrackBar.Tickstyle.BottomRight;
            this.TrackSeekBar.ValueLeft = 0;
            this.TrackSeekBar.ValueRight = 7;
            this.TrackSeekBar.LeftValueChanged += new System.EventHandler(this.doubleTrackBar1_LeftValueChanged);
            this.TrackSeekBar.RightValueChanged += new System.EventHandler(this.doubleTrackBar1_RightValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "From: ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(239, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "To: ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(42, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "...";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(269, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "...";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnLoadAudio
            // 
            this.BtnLoadAudio.Location = new System.Drawing.Point(12, 12);
            this.BtnLoadAudio.Name = "BtnLoadAudio";
            this.BtnLoadAudio.Size = new System.Drawing.Size(52, 23);
            this.BtnLoadAudio.TabIndex = 8;
            this.BtnLoadAudio.Text = "Load";
            this.BtnLoadAudio.UseVisualStyleBackColor = true;
            this.BtnLoadAudio.Click += new System.EventHandler(this.BtnLoadAudio_Click);
            // 
            // TrackPathBox
            // 
            this.TrackPathBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackPathBox.Location = new System.Drawing.Point(70, 14);
            this.TrackPathBox.Name = "TrackPathBox";
            this.TrackPathBox.ReadOnly = true;
            this.TrackPathBox.Size = new System.Drawing.Size(203, 20);
            this.TrackPathBox.TabIndex = 9;
            // 
            // Cb_RetainTags
            // 
            this.Cb_RetainTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cb_RetainTags.AutoSize = true;
            this.Cb_RetainTags.Location = new System.Drawing.Point(12, 120);
            this.Cb_RetainTags.Name = "Cb_RetainTags";
            this.Cb_RetainTags.Size = new System.Drawing.Size(84, 17);
            this.Cb_RetainTags.TabIndex = 11;
            this.Cb_RetainTags.Text = "Retain Tags";
            this.Cb_RetainTags.UseVisualStyleBackColor = true;
            // 
            // BtnMediaInfo
            // 
            this.BtnMediaInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnMediaInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnMediaInfo.Location = new System.Drawing.Point(279, 12);
            this.BtnMediaInfo.Name = "BtnMediaInfo";
            this.BtnMediaInfo.Size = new System.Drawing.Size(24, 23);
            this.BtnMediaInfo.TabIndex = 16;
            this.BtnMediaInfo.Text = "i";
            this.BtnMediaInfo.UseVisualStyleBackColor = true;
            this.BtnMediaInfo.Click += new System.EventHandler(this.AudioInfo_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoSize = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 88);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(290, 19);
            this.panel1.TabIndex = 17;
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.StatusProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 151);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(315, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = false;
            this.StatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.StatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.StatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.StatusLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(0, 3, 1, 2);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(120, 17);
            this.StatusLabel.Text = "Status:";
            // 
            // StatusProgress
            // 
            this.StatusProgress.AutoSize = false;
            this.StatusProgress.Name = "StatusProgress";
            this.StatusProgress.Size = new System.Drawing.Size(185, 16);
            // 
            // CutterDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 173);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BtnMediaInfo);
            this.Controls.Add(this.Cb_RetainTags);
            this.Controls.Add(this.TrackPathBox);
            this.Controls.Add(this.BtnLoadAudio);
            this.Controls.Add(this.TrackSeekBar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "CutterDialog";
            this.Text = "CutterDialog";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private DoubleTrackBar.DoubleTrackBar TrackSeekBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnLoadAudio;
        private System.Windows.Forms.TextBox TrackPathBox;
        private System.Windows.Forms.CheckBox Cb_RetainTags;
        private System.Windows.Forms.Button BtnMediaInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripProgressBar StatusProgress;
    }
}