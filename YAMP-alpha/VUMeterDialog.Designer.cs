namespace YAMP_alpha
{
    partial class VUMeterDialog
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
            System.Windows.Forms.AGaugeRange aGaugeRange25 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange26 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange27 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange28 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange29 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange30 = new System.Windows.Forms.AGaugeRange();
            this.aGauge1 = new System.Windows.Forms.AGauge();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.aGauge2 = new System.Windows.Forms.AGauge();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // aGauge1
            // 
            this.aGauge1.BaseArcColor = System.Drawing.Color.Gray;
            this.aGauge1.BaseArcRadius = 80;
            this.aGauge1.BaseArcStart = 180;
            this.aGauge1.BaseArcSweep = 180;
            this.aGauge1.BaseArcWidth = 2;
            this.aGauge1.Center = new System.Drawing.Point(110, 110);
            this.aGauge1.Dock = System.Windows.Forms.DockStyle.Fill;
            aGaugeRange25.Color = System.Drawing.Color.Lime;
            aGaugeRange25.EndValue = 0.5F;
            aGaugeRange25.InnerRadius = 70;
            aGaugeRange25.InRange = false;
            aGaugeRange25.Name = "GaugeRange1";
            aGaugeRange25.OuterRadius = 80;
            aGaugeRange25.StartValue = 0F;
            aGaugeRange26.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            aGaugeRange26.EndValue = 0.8F;
            aGaugeRange26.InnerRadius = 70;
            aGaugeRange26.InRange = false;
            aGaugeRange26.Name = "GaugeRange2";
            aGaugeRange26.OuterRadius = 80;
            aGaugeRange26.StartValue = 0.5F;
            aGaugeRange27.Color = System.Drawing.Color.Red;
            aGaugeRange27.EndValue = 1F;
            aGaugeRange27.InnerRadius = 70;
            aGaugeRange27.InRange = false;
            aGaugeRange27.Name = "GaugeRange3";
            aGaugeRange27.OuterRadius = 80;
            aGaugeRange27.StartValue = 0.8F;
            this.aGauge1.GaugeRanges.Add(aGaugeRange25);
            this.aGauge1.GaugeRanges.Add(aGaugeRange26);
            this.aGauge1.GaugeRanges.Add(aGaugeRange27);
            this.aGauge1.Location = new System.Drawing.Point(0, 0);
            this.aGauge1.MaxValue = 1F;
            this.aGauge1.MinValue = 0F;
            this.aGauge1.Name = "aGauge1";
            this.aGauge1.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray;
            this.aGauge1.NeedleColor2 = System.Drawing.Color.DimGray;
            this.aGauge1.NeedleRadius = 80;
            this.aGauge1.NeedleType = System.Windows.Forms.NeedleType.Advance;
            this.aGauge1.NeedleWidth = 2;
            this.aGauge1.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.aGauge1.ScaleLinesInterInnerRadius = 73;
            this.aGauge1.ScaleLinesInterOuterRadius = 80;
            this.aGauge1.ScaleLinesInterWidth = 1;
            this.aGauge1.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.aGauge1.ScaleLinesMajorInnerRadius = 70;
            this.aGauge1.ScaleLinesMajorOuterRadius = 80;
            this.aGauge1.ScaleLinesMajorStepValue = 1F;
            this.aGauge1.ScaleLinesMajorWidth = 2;
            this.aGauge1.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.aGauge1.ScaleLinesMinorInnerRadius = 75;
            this.aGauge1.ScaleLinesMinorOuterRadius = 80;
            this.aGauge1.ScaleLinesMinorTicks = 9;
            this.aGauge1.ScaleLinesMinorWidth = 1;
            this.aGauge1.ScaleNumbersColor = System.Drawing.Color.Black;
            this.aGauge1.ScaleNumbersFormat = null;
            this.aGauge1.ScaleNumbersRadius = 95;
            this.aGauge1.ScaleNumbersRotation = 0;
            this.aGauge1.ScaleNumbersStartScaleLine = 0;
            this.aGauge1.ScaleNumbersStepScaleLines = 1;
            this.aGauge1.Size = new System.Drawing.Size(221, 130);
            this.aGauge1.TabIndex = 0;
            this.aGauge1.Text = "aGauge1";
            this.aGauge1.Value = 0F;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.aGauge1);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(221, 163);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.aGauge2);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(224, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(221, 163);
            this.panel2.TabIndex = 3;
            // 
            // aGauge2
            // 
            this.aGauge2.BaseArcColor = System.Drawing.Color.Gray;
            this.aGauge2.BaseArcRadius = 80;
            this.aGauge2.BaseArcStart = 180;
            this.aGauge2.BaseArcSweep = 180;
            this.aGauge2.BaseArcWidth = 2;
            this.aGauge2.Center = new System.Drawing.Point(110, 110);
            this.aGauge2.Dock = System.Windows.Forms.DockStyle.Fill;
            aGaugeRange28.Color = System.Drawing.Color.Lime;
            aGaugeRange28.EndValue = 0.5F;
            aGaugeRange28.InnerRadius = 70;
            aGaugeRange28.InRange = false;
            aGaugeRange28.Name = "GaugeRange1";
            aGaugeRange28.OuterRadius = 80;
            aGaugeRange28.StartValue = 0F;
            aGaugeRange29.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            aGaugeRange29.EndValue = 0.8F;
            aGaugeRange29.InnerRadius = 70;
            aGaugeRange29.InRange = false;
            aGaugeRange29.Name = "GaugeRange2";
            aGaugeRange29.OuterRadius = 80;
            aGaugeRange29.StartValue = 0.5F;
            aGaugeRange30.Color = System.Drawing.Color.Red;
            aGaugeRange30.EndValue = 1F;
            aGaugeRange30.InnerRadius = 70;
            aGaugeRange30.InRange = false;
            aGaugeRange30.Name = "GaugeRange3";
            aGaugeRange30.OuterRadius = 80;
            aGaugeRange30.StartValue = 0.8F;
            this.aGauge2.GaugeRanges.Add(aGaugeRange28);
            this.aGauge2.GaugeRanges.Add(aGaugeRange29);
            this.aGauge2.GaugeRanges.Add(aGaugeRange30);
            this.aGauge2.Location = new System.Drawing.Point(0, 0);
            this.aGauge2.MaxValue = 1F;
            this.aGauge2.MinValue = 0F;
            this.aGauge2.Name = "aGauge2";
            this.aGauge2.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray;
            this.aGauge2.NeedleColor2 = System.Drawing.Color.DimGray;
            this.aGauge2.NeedleRadius = 80;
            this.aGauge2.NeedleType = System.Windows.Forms.NeedleType.Advance;
            this.aGauge2.NeedleWidth = 2;
            this.aGauge2.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.aGauge2.ScaleLinesInterInnerRadius = 73;
            this.aGauge2.ScaleLinesInterOuterRadius = 80;
            this.aGauge2.ScaleLinesInterWidth = 1;
            this.aGauge2.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.aGauge2.ScaleLinesMajorInnerRadius = 70;
            this.aGauge2.ScaleLinesMajorOuterRadius = 80;
            this.aGauge2.ScaleLinesMajorStepValue = 1F;
            this.aGauge2.ScaleLinesMajorWidth = 2;
            this.aGauge2.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.aGauge2.ScaleLinesMinorInnerRadius = 75;
            this.aGauge2.ScaleLinesMinorOuterRadius = 80;
            this.aGauge2.ScaleLinesMinorTicks = 9;
            this.aGauge2.ScaleLinesMinorWidth = 1;
            this.aGauge2.ScaleNumbersColor = System.Drawing.Color.Black;
            this.aGauge2.ScaleNumbersFormat = null;
            this.aGauge2.ScaleNumbersRadius = 95;
            this.aGauge2.ScaleNumbersRotation = 0;
            this.aGauge2.ScaleNumbersStartScaleLine = 0;
            this.aGauge2.ScaleNumbersStepScaleLines = 1;
            this.aGauge2.Size = new System.Drawing.Size(221, 130);
            this.aGauge2.TabIndex = 0;
            this.aGauge2.Text = "aGauge2";
            this.aGauge2.Value = 0F;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 130);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(221, 33);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 130);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(221, 33);
            this.panel4.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Left Channel";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(221, 33);
            this.label2.TabIndex = 1;
            this.label2.Text = "Right Channel";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Interval = 25;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // VUMeterDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 163);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "VUMeterDialog";
            this.Text = "VUMeterDialog";
            this.Load += new System.EventHandler(this.VUMeterDialog_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.AGauge aGauge1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.AGauge aGauge2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
    }
}