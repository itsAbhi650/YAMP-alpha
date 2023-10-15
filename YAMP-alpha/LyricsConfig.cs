using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class LyricsConfig : Form
    {
        FontStyle LyricsFontStyle = new FontStyle();
        public Font NewLyricsFont = DefaultFont;
        public Brush NewLyricsBrush = SystemBrushes.ActiveBorder;
        public Brush NewLyricHighlightBrush = SystemBrushes.ActiveBorder;
        public Brush NewLyricsBorderBrush = SystemBrushes.ActiveBorder;
        //public static bool EnableLyricsLineGradient;
        public Size LyricSize;
        private static Rectangle LyricRect;

        public LyricsConfig()
        {
            InitializeComponent();
            ComboBoxFonts.DataSource = FontFamily.Families.Select(x => x.Name).ToList();
            ComboBoxFonts.SelectedItem = LyricsHelper.LyricsFont.Name;
            LyricsFontStyle = LyricsHelper.LyricsFont.Style;
            cbBold.Tag = FontStyle.Bold;
            cbItalic.Tag = FontStyle.Italic;
            cbUnderline.Tag = FontStyle.Underline;
            cbStrike.Tag = FontStyle.Strikeout;
            cbBold.Checked = LyricsFontStyle.HasFlag(FontStyle.Bold);
            cbUnderline.Checked = LyricsFontStyle.HasFlag(FontStyle.Underline);
            cbItalic.Checked = LyricsFontStyle.HasFlag(FontStyle.Italic);
            cbStrike.Checked = LyricsFontStyle.HasFlag(FontStyle.Strikeout);
            cbStrike.CheckedChanged += FontEmphasis_CheckedChanged;
            cbUnderline.CheckedChanged += FontEmphasis_CheckedChanged;
            cbItalic.CheckedChanged += FontEmphasis_CheckedChanged;
            cbBold.CheckedChanged += FontEmphasis_CheckedChanged;
            cbEnableLyricsBackColor.Checked = LyricsHelper.EnableLyricsHighlight;
            pnlFirstBackColor.BackColor = LyricsHelper.LyricHighlightPrimaryColor;
            pnlSecondBackColor.BackColor = LyricsHelper.LyricHighlightSecondryColor;
            pnlLyricBrdFirstColor.BackColor = LyricsHelper.LyricBorderPrimaryColor;
            pnlLyricBrdSecndColor.BackColor = LyricsHelper.LyricBorderSecondryColor;
            cbLyricsBorderGradient.Checked = LyricsHelper.EnableLyricsBorderGradient;
            cbLyricsBackGradient.Checked = LyricsHelper.EnableLyricsHighlightGradient;
            cmbBxBackColorDirection.SelectedIndex = ((int)LyricsHelper.LyricsHighlightGradientMode) - 1;
            string[] GradientDirections = Enum.GetNames(typeof(System.Drawing.Drawing2D.LinearGradientMode));
            cmbBxBorderColorDirection.DataSource = GradientDirections;
            cmbBxBackColorDirection.DataSource = GradientDirections.ToArray(); //To create a clone copy of original array. This will avoid all dropdown changing together.
            cmbBxForeColorDirection.DataSource = GradientDirections.ToArray(); //To create a clone copy of original array. This will avoid all dropdown changing together.
            //ComboBoxFonts.SelectedItem = NewLyricsFont.Name;
            cbEnableLyricsBorder.Checked = LyricsHelper.EnableLyricsBorder;
            FontSizeUpDown.Value = (decimal)LyricsHelper.LyricsFont.Size;
            NewLyricsBrush = LyricsHelper.LyricsWriterBrush;
            ComboBoxFonts.SelectedItem = NewLyricsFont.Name;
            if (NewLyricsBrush is SolidBrush solidcolorbrush)
            {
                primaryColorPanel.BackColor = solidcolorbrush.Color;
                cbGradientEnable.Checked = false;
            }
            else if (NewLyricsBrush is System.Drawing.Drawing2D.LinearGradientBrush GradientBrush)
            {
                primaryColorPanel.BackColor = GradientBrush.LinearColors[0];
                secondryColorPanel.BackColor = GradientBrush.LinearColors[1];
                cbGradientEnable.Checked = true;
            }
        }

        private void FontEmphasis_CheckedChanged(object sender, EventArgs e)
        {
            enumCheckedChanged(sender as CheckBox, true);
        }

        private void enumCheckedChanged(CheckBox pBox, bool pbAddFirst)
        {
            if (!(pBox.Tag is FontStyle)) return;

            FontStyle flag = (FontStyle)pBox.Tag;

            if (pBox.Checked && pbAddFirst)
            {
                LyricsFontStyle |= flag;
            }
            else if (pBox.Checked && !pbAddFirst)
            {
                LyricsFontStyle |= flag;
            }
            else
            {
                LyricsFontStyle ^= flag;
            }
            NewLyricsFont = new Font(ComboBoxFonts.SelectedItem.ToString(), (float)FontSizeUpDown.Value, LyricsFontStyle);
            pictureBox1.Refresh();
        }

        private void ComboBoxFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            NewLyricsFont = new Font(ComboBoxFonts.SelectedItem.ToString(), (float)FontSizeUpDown.Value, LyricsFontStyle);
            LyricRect = LyricsHelper.UpdateLyricRect(testTextBox.Text, pictureBox1.DisplayRectangle, NewLyricsFont);
            pictureBox1.Refresh();
        }

        private void FontSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            NewLyricsFont = new Font(ComboBoxFonts.SelectedItem.ToString(), (float)FontSizeUpDown.Value, LyricsFontStyle);
            LyricRect = LyricsHelper.UpdateLyricRect(testTextBox.Text, pictureBox1.DisplayRectangle, NewLyricsFont);
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(ComboBoxFonts.SelectedItem)))
            {
                MessageBox.Show("Font not selected!");
                ComboBoxFonts.Focus();
                return;
            }

            if (cbEnableLyricsBackColor.Checked)
            {
                e.Graphics.FillRectangle(NewLyricHighlightBrush, LyricRect);
            }
            e.Graphics.DrawString(testTextBox.Text, NewLyricsFont, NewLyricsBrush, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            if (cbEnableLyricsBorder.Checked)
            {
                e.Graphics.DrawRectangle(new Pen(NewLyricsBorderBrush), LyricRect);
            }
        }

        private void cbGradientEnable_CheckedChanged(object sender, EventArgs e)
        {
            label9.Visible = label8.Visible = cmbBxForeColorDirection.Visible = secondryColorPanel.Visible = cbGradientEnable.Checked;
            if (cbGradientEnable.Checked)
            {
                label6.Text = "Color1:";
                label8.Text = "Color2:";
            }
            else
            {
                label6.Text = "ForeColor:";
            }
            UpdateLyricsBrush(ref NewLyricsBrush, LyricsHelper.LyricPrimaryForeColor, LyricsHelper.LyricSecondryForeColor, LyricRect, cbGradientEnable.Checked, (System.Drawing.Drawing2D.LinearGradientMode)cmbBxForeColorDirection.SelectedIndex);
            pictureBox1.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LyricRect = LyricsHelper.UpdateLyricRect(testTextBox.Text, pictureBox1.DisplayRectangle, NewLyricsFont);
            UpdateLyricsBrush(ref NewLyricsBrush, LyricsHelper.LyricPrimaryForeColor, LyricsHelper.LyricSecondryForeColor, LyricRect, cbGradientEnable.Checked, (System.Drawing.Drawing2D.LinearGradientMode)cmbBxForeColorDirection.SelectedIndex);
            UpdateLyricsBorderBrushArea(pictureBox1.DisplayRectangle);
            pictureBox1.Refresh();
        }

        private void cmbbxDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbGradientEnable.Checked)
            {
                NewLyricsBrush = new System.Drawing.Drawing2D.LinearGradientBrush(LyricRect, LyricsHelper.LyricPrimaryForeColor, LyricsHelper.LyricSecondryForeColor, (System.Drawing.Drawing2D.LinearGradientMode)cmbBxForeColorDirection.SelectedIndex);
                pictureBox1.Refresh();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            LyricsHelper.LyricsFont = NewLyricsFont;

            LyricsHelper.LyricsWriterBrush = NewLyricsBrush;
            LyricsHelper.LyricsHighlightBrush = NewLyricHighlightBrush;
            LyricsHelper.LyricsBorderBrush = NewLyricsBorderBrush;

            LyricsHelper.EnableLyricsBorder = cbEnableLyricsBorder.Checked;
            LyricsHelper.EnableLyricsHighlight = cbEnableLyricsBackColor.Checked;
            LyricsHelper.EnableLyricsForeColorGradient = cbGradientEnable.Checked;
            LyricsHelper.EnableLyricsBorderGradient = cbLyricsBorderGradient.Checked;
            LyricsHelper.EnableLyricsHighlightGradient = cbLyricsBackGradient.Checked;

            LyricsHelper.LyricsHighlightGradientMode = (System.Drawing.Drawing2D.LinearGradientMode)Enum.Parse(typeof(System.Drawing.Drawing2D.LinearGradientMode), cmbBxBackColorDirection.SelectedItem.ToString());
            LyricsHelper.LyricsTextGradientMode = (System.Drawing.Drawing2D.LinearGradientMode)Enum.Parse(typeof(System.Drawing.Drawing2D.LinearGradientMode), cmbBxForeColorDirection.SelectedItem.ToString());
            LyricsHelper.LyricsBorderGradientMode = (System.Drawing.Drawing2D.LinearGradientMode)Enum.Parse(typeof(System.Drawing.Drawing2D.LinearGradientMode), cmbBxBorderColorDirection.SelectedItem.ToString());

            DialogResult = DialogResult.OK;
        }

        private void LyricsConfig_Load(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void panel6_DoubleClick(object sender, EventArgs e)
        {
            using (ColorDialog clrDiag = new ColorDialog())
            {
                if (clrDiag.ShowDialog() == DialogResult.OK)
                {
                    ((Panel)sender).BackColor = clrDiag.Color;
                    UpdateLyricsBorderBrushArea(pictureBox1.DisplayRectangle);
                    LyricsHelper.UpdateLyricsHighlightBrush(ref NewLyricHighlightBrush, pictureBox1.DisplayRectangle, cbLyricsBackGradient.Checked);
                    pictureBox1.Refresh();
                }
            }
        }

        private void cbBackGradient_CheckedChanged(object sender, EventArgs e)
        {
            pnlSecondBackColor.Visible = label11.Visible = label14.Visible = cmbBxBackColorDirection.Visible = cbLyricsBackGradient.Checked;
            LyricsHelper.UpdateLyricsHighlightBrush(ref NewLyricHighlightBrush, pictureBox1.DisplayRectangle, cbLyricsBackGradient.Checked);
            pictureBox1.Refresh();
        }

        private void UpdateLyricsBorderBrushArea(Rectangle Canvas)
        {
            if (LyricsHelper.EnableLyricsBorderGradient)
            {
                Rectangle HalfCentered = new Rectangle((int)(Canvas.Width * 0.25F), (int)(Canvas.Height * 0.25F), Canvas.Width / 2, Canvas.Height / 2);
                NewLyricsBorderBrush = new System.Drawing.Drawing2D.LinearGradientBrush(Canvas, LyricsHelper.LyricBorderPrimaryColor, LyricsHelper.LyricBorderSecondryColor, LyricsHelper.LyricsBorderGradientMode);
            }
            else
            {
                NewLyricsBorderBrush = new SolidBrush(pnlLyricBrdFirstColor.BackColor);
            }
        }

        private void UpdateLyricsBrush(ref Brush brush, Color Color1, Color Color2, Rectangle DrawAreaRect, bool EnableGradient, System.Drawing.Drawing2D.LinearGradientMode GradientMode)
        {
            if (EnableGradient && LyricRect.Width != 0 && LyricRect.Height != 0)
            {
                brush = new System.Drawing.Drawing2D.LinearGradientBrush(DrawAreaRect, Color1, Color2, GradientMode);
            }
            else
            {
                brush = new SolidBrush(Color1);
            }
        }

        private void chkEnableLyricsBackColor_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void cmbBxBackColorDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            LyricsHelper.UpdateLyricsHighlightBrush(ref NewLyricHighlightBrush, pictureBox1.DisplayRectangle, cbLyricsBackGradient.Checked);
            pictureBox1.Refresh();
        }

        private void cbEnableLyricsBorder_CheckedChanged(object sender, EventArgs e)
        {
            if (LyricRect == null || LyricRect == Rectangle.Empty) LyricRect = LyricsHelper.UpdateLyricRect(testTextBox.Text, pictureBox1.DisplayRectangle, NewLyricsFont);
            pictureBox1.Refresh();
        }

        private void cbLyricsBorderColor_CheckedChanged(object sender, EventArgs e)
        {
            label24.Visible = label21.Visible = cmbBxBorderColorDirection.Visible = pnlLyricBrdSecndColor.Visible = cbLyricsBorderGradient.Checked;
            UpdateLyricsBorderBrushArea(pictureBox1.DisplayRectangle);
            pictureBox1.Refresh();
        }

        private void cmbBxBorderColorDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            LyricRect = LyricsHelper.UpdateLyricRect(testTextBox.Text, pictureBox1.DisplayRectangle, NewLyricsFont);
            UpdateLyricsBorderBrushArea(pictureBox1.DisplayRectangle);
            pictureBox1.Refresh();
        }

        private void LyricsColorPanels_DoubleClick(object sender, EventArgs e)
        {
            using (ColorDialog clrDiag = new ColorDialog())
            {
                if (clrDiag.ShowDialog() == DialogResult.OK)
                {
                    Panel clrPnl = (Panel)sender;
                    string tagPnl = clrPnl.Tag.ToString();
                    clrPnl.BackColor = clrDiag.Color;
                    if (tagPnl.Contains("B"))
                    {
                        LyricsHelper.LyricBorderPrimaryColor = pnlLyricBrdFirstColor.BackColor;
                        LyricsHelper.LyricBorderSecondryColor = pnlLyricBrdSecndColor.BackColor;
                        UpdateLyricsBrush(ref NewLyricsBorderBrush, LyricsHelper.LyricBorderPrimaryColor, LyricsHelper.LyricBorderSecondryColor, pictureBox1.DisplayRectangle, cbLyricsBorderGradient.Checked, (System.Drawing.Drawing2D.LinearGradientMode)cmbBxBorderColorDirection.SelectedIndex);
                    }
                    else if (tagPnl.Contains("H"))
                    {
                        LyricsHelper.LyricHighlightPrimaryColor = pnlFirstBackColor.BackColor;
                        LyricsHelper.LyricHighlightSecondryColor = pnlSecondBackColor.BackColor;
                        UpdateLyricsBrush(ref NewLyricHighlightBrush, LyricsHelper.LyricHighlightPrimaryColor, LyricsHelper.LyricHighlightSecondryColor, LyricRect, cbLyricsBackGradient.Checked, (System.Drawing.Drawing2D.LinearGradientMode)cmbBxBackColorDirection.SelectedIndex);
                    }
                    else if (tagPnl.Contains("F"))
                    {
                        LyricsHelper.LyricPrimaryForeColor = primaryColorPanel.BackColor;
                        LyricsHelper.LyricSecondryForeColor = secondryColorPanel.BackColor;
                        UpdateLyricsBrush(ref NewLyricsBrush, LyricsHelper.LyricPrimaryForeColor, LyricsHelper.LyricSecondryForeColor, LyricRect, cbGradientEnable.Checked, GradientMode: (System.Drawing.Drawing2D.LinearGradientMode)cmbBxForeColorDirection.SelectedIndex);
                    }
                    pictureBox1.Refresh();
                }
            }
        }
    }
}
