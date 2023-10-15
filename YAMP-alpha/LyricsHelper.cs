using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMP_alpha
{
    public static class LyricsHelper
    {
        public static bool EnableLyricsBorder { get; set; }
        public static bool EnableLyricsForeColorGradient { get; set; }
        public static bool EnableLyricsHighlight { get; internal set; }
        public static bool EnableLyricsHighlightGradient { get; internal set; }
        public static bool EnableLyricsBorderGradient { get; internal set; }

        public static Brush LyricsWriterBrush = new SolidBrush(Color.White);
        public static Brush LyricsHighlightBrush = new SolidBrush(Color.White);
        public static Brush LyricsBorderBrush = new SolidBrush(Color.White);
        public static Color LyricPrimaryForeColor = Color.White;
        public static Color LyricSecondryForeColor = Color.White;
        public static Color LyricBorderPrimaryColor = Color.White;
        public static Color LyricBorderSecondryColor = Color.White;
        public static Color LyricHighlightPrimaryColor = Color.White;
        public static Color LyricHighlightSecondryColor = Color.White;
        public static System.Drawing.Drawing2D.LinearGradientMode LyricsTextGradientMode;
        public static System.Drawing.Drawing2D.LinearGradientMode LyricsBorderGradientMode;
        public static System.Drawing.Drawing2D.LinearGradientMode LyricsHighlightGradientMode;
        public static FontStyle LyricsFontStyle = FontStyle.Regular;
        public static Font LyricsFont = new Font("Arial", 14, LyricsFontStyle);
        private const long TICKS_PER_MILLISECOND = 10_000;
        private const long TICKS_PER_SECOND = TICKS_PER_MILLISECOND * 1000;
        private const long TICKS_PER_MINUTE = TICKS_PER_SECOND * 60;

        public static Rectangle GetTextRectangle(Rectangle Canvas, string Text, Font font)
        {
            Rectangle rect = Rectangle.Empty;
            if (Text.Length > 0)
            {
                Size sz = System.Windows.Forms.TextRenderer.MeasureText(Text, font);
                sz.Width = (int)(sz.Width * 1.25F);
                rect = new Rectangle(new Point((Canvas.Width - sz.Width) / 2, (Canvas.Height - sz.Height) / 2), sz);
            }
            return rect;
        }

        public static void UpdateLyricsWriterBrush(Rectangle LyricRect)
        {
            if (EnableLyricsForeColorGradient && LyricRect.Width != 0 && LyricRect.Height != 0)
            {
                LyricsWriterBrush = new System.Drawing.Drawing2D.LinearGradientBrush(LyricRect, LyricPrimaryForeColor, LyricSecondryForeColor, LyricsTextGradientMode);
            }
            else
            {
                LyricsWriterBrush = new SolidBrush(LyricPrimaryForeColor);
            }
        }

        public static void UpdateLyricsBorderBrush(Rectangle Canvas)
        {
            if (EnableLyricsBorderGradient)
            {
                LyricsBorderBrush = new System.Drawing.Drawing2D.LinearGradientBrush(Canvas, LyricBorderPrimaryColor, LyricBorderSecondryColor, LyricsBorderGradientMode);
            }
            else
            {
                LyricsBorderBrush = new SolidBrush(LyricBorderPrimaryColor);
            }
        }

        public static void UpdateLyricsHighlightBrush(ref Brush brush, Rectangle Canvas, bool EnableGradient = false)
        {
            if (EnableGradient)
            {
                brush = new System.Drawing.Drawing2D.LinearGradientBrush(Canvas, LyricHighlightPrimaryColor, LyricHighlightSecondryColor, LyricsHighlightGradientMode);
            }
            else
            {
                brush = new SolidBrush(LyricHighlightPrimaryColor);
            }
        }

        public static Rectangle UpdateLyricRect(string Text, Rectangle Canvas, Font font)
        {
            Size LyricSize = System.Windows.Forms.TextRenderer.MeasureText(Text, font);
            Rectangle LyricRect = new Rectangle(new Point((Canvas.Width - LyricSize.Width) / 2, (Canvas.Height - LyricSize.Height) / 2), LyricSize);
            return LyricRect;
        }

        public static bool TryParseLrcString(string value, int start, int end, out TimeSpan result)
        {
            var m = 0;
            var s = 0;
            var t = 0;

            var i = start;
            for (; i < end; i++)
            {
                var v = value[i] - '0';
                if (v >= 0 && v <= 9)
                    m = m * 10 + v;
                else if (value[i] == ':')
                {
                    i++;
                    break;
                }
                else if (char.IsWhiteSpace(value, i))
                {
                    continue;
                }
                else
                {
                    goto ERROR;
                }
            }

            for (; i < end; i++)
            {
                var v = value[i] - '0';
                if (v >= 0 && v <= 9)
                    s = s * 10 + v;
                else if (value[i] == '.')
                {
                    i++;
                    break;
                }
                else if (char.IsWhiteSpace(value, i))
                {
                    continue;
                }
                else
                {
                    goto ERROR;
                }
            }

            var weight = (int)(TICKS_PER_SECOND / 10);
            for (; i < end; i++)
            {
                var v = value[i] - '0';
                if (v >= 0 && v <= 9)
                {
                    t += weight * v;
                    weight /= 10;
                }
                else if (char.IsWhiteSpace(value, i))
                {
                    continue;
                }
                else
                {
                    goto ERROR;
                }
            }

            result = new TimeSpan(t + TICKS_PER_SECOND * s + TICKS_PER_MINUTE * m);
            return true;

        ERROR:
            result = default;
            return false;
        }

    }
}
