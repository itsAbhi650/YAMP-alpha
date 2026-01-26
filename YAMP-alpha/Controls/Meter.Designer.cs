namespace YAMP_alpha.Controls
{
    partial class MeterControl
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                
                // Clean up animation timer from main class
                if (_animationTimer != null)
                {
                    _animationTimer.Stop();
                    _animationTimer.Dispose();
                }
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
            this.MeterBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.MeterBox)).BeginInit();
            this.SuspendLayout();
            // 
            // MeterBox
            // 
            this.MeterBox.BackColor = System.Drawing.Color.Black;
            this.MeterBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MeterBox.Location = new System.Drawing.Point(0, 0);
            this.MeterBox.Name = "MeterBox";
            this.MeterBox.Size = new System.Drawing.Size(60, 260);
            this.MeterBox.TabIndex = 0;
            this.MeterBox.TabStop = false;
            // 
            // MeterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.Controls.Add(this.MeterBox);
            this.Name = "MeterControl";
            this.Size = new System.Drawing.Size(60, 260);
            this.SizeChanged += new System.EventHandler(this.MeterControl_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.MeterBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox MeterBox;
    }
}
