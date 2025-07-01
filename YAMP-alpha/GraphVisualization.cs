//NOTE: This is only a very, very, very minimalistic graph visualization. Don't use it in real world scenarios.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace YAMP_alpha
{
    public enum Channel
    {
        Left = 1,
        Right = 2,
        Both = 3
    }

    public class GraphVisualization
    {
        private readonly List<float> _left = new List<float>();
        private readonly List<float> _right = new List<float>();
        private readonly object _lockObj = new object();
        public Color RightChannelColor { get; set; } = Color.Red;
        public Color LeftChannelColor { get; set; } = Color.DeepSkyBlue;

        public Channel RenderChannel { get; set; } = Channel.Both;

        public bool HasSample => _left.Count > 0 && _right.Count > 0;

        public GraphVisualization()
        {

        }

        public GraphVisualization(bool DrawLeftSpectrum, bool DrawRightSpectrum)
        {
            DrawLeftChannel = DrawLeftSpectrum;
            DrawRightChannel = DrawRightChannel;
        }

        public void AddSamples(float left, float right)
        {
            lock (_lockObj)
            {
                _left.Add(left);
                _right.Add(right);
            }
        }

        public Image Draw(int width, int height)
        {
            var image = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(image))
            {
                Draw(g, width, height, 2);
            }
            return image;
        }

        public void Draw(Graphics graphics, int width, int height, int pixelsPerSample)
        {
            if (!HasSample)
            {
                return;
            }
            var samplesLeft = GetSamplesToDraw(_left, width / pixelsPerSample).ToArray();
            var samplesRight = GetSamplesToDraw(_right, width / pixelsPerSample).ToArray();

            switch (RenderChannel)
            {
                case Channel.Left:
                    graphics.DrawLines(new Pen(LeftChannelColor, 1), GetPoints(samplesLeft, pixelsPerSample, width, height).ToArray());
                    break;
                case Channel.Right:
                    graphics.DrawLines(new Pen(Color.FromArgb(150, RightChannelColor), 0.5f), GetPoints(samplesRight, pixelsPerSample, width, height).ToArray());
                    break;
                case Channel.Both:
                    graphics.DrawLines(new Pen(LeftChannelColor, 1), GetPoints(samplesLeft, pixelsPerSample, width, height).ToArray());
                    graphics.DrawLines(new Pen(Color.FromArgb(150, RightChannelColor), 0.5f), GetPoints(samplesRight, pixelsPerSample, width, height).ToArray());
                    break;
                default:
                    break;
            }
        }

        private IEnumerable<Point> GetPoints(float[] samples, int pixelsPerSample, int width, int height)
        {
            int halfY = height / pixelsPerSample;
            if (samples.Length >= 2)
            {
                for (int i = 0; i < samples.Length; i++)
                {
                    Point point = new Point
                    {
                        X = i * pixelsPerSample,
                        Y = halfY + (int)(samples[i] * halfY)
                    };
                    yield return point;
                }
            }
            else
            {
                yield return new Point(0, halfY);
                yield return new Point(width, halfY);
            }
        } 

        private IEnumerable<float> GetSamplesToDraw(List<float> inputSamples, int numberOfSamplesRequested)
        {
            float[] samples;
            lock (_lockObj)
            {
                samples = inputSamples.ToArray();
                inputSamples.Clear();
            }

            var resolution = samples.Length / numberOfSamplesRequested;
            int index = 0;
            float currentMax = 0;
            for (int i = 0; i < samples.Length; i++)
            {
                if (i > index * resolution)
                {
                    yield return currentMax;
                    currentMax = 0;
                    index++;
                }

                if (Math.Abs(currentMax) < Math.Abs(samples[i]))
                    currentMax = samples[i];
            }
        } 
    }
}
