namespace YAMP_alpha.Controls
{
    partial class EQBand
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
            this.EQBox = new System.Windows.Forms.GroupBox();
            this.EQBandBar = new System.Windows.Forms.TrackBar();
            this.EQSideMargin = new System.Windows.Forms.Panel();
            this.EQFooter = new System.Windows.Forms.Label();
            this.EQBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EQBandBar)).BeginInit();
            this.SuspendLayout();
            // 
            // EQBox
            // 
            this.EQBox.Controls.Add(this.EQBandBar);
            this.EQBox.Controls.Add(this.EQSideMargin);
            this.EQBox.Controls.Add(this.EQFooter);
            this.EQBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EQBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EQBox.Location = new System.Drawing.Point(0, 0);
            this.EQBox.Name = "EQBox";
            this.EQBox.Padding = new System.Windows.Forms.Padding(1);
            this.EQBox.Size = new System.Drawing.Size(72, 182);
            this.EQBox.TabIndex = 0;
            this.EQBox.TabStop = false;
            // 
            // EQBandBar
            // 
            this.EQBandBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EQBandBar.Location = new System.Drawing.Point(17, 14);
            this.EQBandBar.Name = "EQBandBar";
            this.EQBandBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.EQBandBar.Size = new System.Drawing.Size(54, 152);
            this.EQBandBar.TabIndex = 2;
            this.EQBandBar.TickFrequency = 5;
            this.EQBandBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.EQBandBar.ValueChanged += new System.EventHandler(this.EQBandBar_ValueChanged);
            this.EQBandBar.Enter += new System.EventHandler(this.EQBandBar_Enter);
            this.EQBandBar.ChangeUICues += new System.Windows.Forms.UICuesEventHandler(this.EQBandBar_ChangeUICues);
            // 
            // EQSideMargin
            // 
            this.EQSideMargin.Dock = System.Windows.Forms.DockStyle.Left;
            this.EQSideMargin.Location = new System.Drawing.Point(1, 14);
            this.EQSideMargin.Name = "EQSideMargin";
            this.EQSideMargin.Size = new System.Drawing.Size(16, 152);
            this.EQSideMargin.TabIndex = 0;
            // 
            // EQFooter
            // 
            this.EQFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EQFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.EQFooter.Location = new System.Drawing.Point(1, 166);
            this.EQFooter.Name = "EQFooter";
            this.EQFooter.Size = new System.Drawing.Size(70, 15);
            this.EQFooter.TabIndex = 1;
            this.EQFooter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.EQFooter.Click += new System.EventHandler(this.EQFooter_Click);
            // 
            // EQBand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EQBox);
            this.Name = "EQBand";
            this.Size = new System.Drawing.Size(72, 182);
            this.SizeChanged += new System.EventHandler(this.EQBand_SizeChanged);
            this.Enter += new System.EventHandler(this.EQBand_FocusChanged);
            this.Leave += new System.EventHandler(this.EQBand_FocusChanged);
            this.EQBox.ResumeLayout(false);
            this.EQBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EQBandBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox EQBox;
        private System.Windows.Forms.TrackBar EQBandBar;
        private System.Windows.Forms.Panel EQSideMargin;
        private System.Windows.Forms.Label EQFooter;
    }
}
