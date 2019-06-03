using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using NAudio.Wave;

namespace Tutorial11
{
    /// <summary>
    /// Control for viewing waveforms
    /// </summary>
    public class CustomWaveViewer : System.Windows.Forms.UserControl
    {
        public Color PenColor { get; set; }
        public float PenWidth { get; set; }
        public byte[] bytes { get; set; }
        public byte[] [] waveDataTest { get; set; }
        public int tepmInt { get; set; }

        public void FitToScreen()
        {
            if (waveStream == null) return;

            int samples = (int)(waveStream.Length / bytesPerSample);
            startPosition = 0;
            SamplesPerPixel = samples / this.Width;
        }

        public void Zoom(int leftSample, int rightSample)
        {
            startPosition = leftSample * bytesPerSample;
            SamplesPerPixel = (rightSample - leftSample) / this.Width;
        }

        private Point mousePos, startPos;
        private bool mouseDrag = false;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                startPos = e.Location;
                mousePos = new Point(-1, -1);
                mouseDrag = true;
                DrawVerticalLine(e.X);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mouseDrag)
            {
                DrawVerticalLine(e.X);
                if (mousePos.X != -1) DrawVerticalLine(mousePos.X);
                mousePos = e.Location;
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (mouseDrag && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mouseDrag = false;
                DrawVerticalLine(startPos.X);

                if (mousePos.X == -1) return;
                DrawVerticalLine(mousePos.X);

                int leftSample = (int)(StartPosition / bytesPerSample + samplesPerPixel * Math.Min(startPos.X, mousePos.X));
                int rightSample = (int)(StartPosition / bytesPerSample + samplesPerPixel * Math.Max(startPos.X, mousePos.X));
                Zoom(leftSample, rightSample);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle) FitToScreen();

            base.OnMouseUp(e);
        }

        private void DrawVerticalLine(int x)
        {
            ControlPaint.DrawReversibleLine(PointToScreen(new Point(x, 0)), PointToScreen(new Point(x, Height)), Color.Green);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            FitToScreen();
        }

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private WaveStream waveStream;
        private int samplesPerPixel = 128;
        private long startPosition;
        private int bytesPerSample;
        /// <summary>
        /// Creates a new WaveViewer control
        /// </summary>
        public CustomWaveViewer()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            this.DoubleBuffered = true;

            this.PenColor = Color.DodgerBlue;
            this.PenWidth = 1;
        }

        /// <summary>
        /// sets the associated wavestream
        /// </summary>
        public WaveStream WaveStream
        {
            get
            {
                return waveStream;
            }
            set
            {
                waveStream = value;
                if (waveStream != null)
                {
                    bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8) * waveStream.WaveFormat.Channels;
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// The zoom level, in samples per pixel
        /// </summary>
        public int SamplesPerPixel
        {
            get
            {
                return samplesPerPixel;
            }
            set
            {
                samplesPerPixel = Math.Max(1, value);
                this.Invalidate();
            }
        }

        /// <summary>
        /// Start position (currently in bytes)
        /// </summary>
        public long StartPosition
        {
            get
            {
                return startPosition;
            }
            set
            {
                startPosition = value;
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// <see cref="Control.OnPaint"/>
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (waveStream != null)
            {
                waveStream.Position = 0;
                int bytesRead;
                byte[] waveData = new byte[4944];

                if (waveDataTest == null)
                {
                    waveDataTest = new byte[waveData.Length] [];

                }

                waveStream.Position = startPosition + (e.ClipRectangle.Left * bytesPerSample * samplesPerPixel);
                int qwe = 0;
                using (Pen linePen = new Pen(PenColor, PenWidth))
                {
                    for (float x = e.ClipRectangle.X; x < e.ClipRectangle.Right; x += 1)
                    {
                        short low = 0;
                        short high = 0;
               

                        if (waveDataTest[qwe] == null)
                        {
                            tepmInt = 0;
                        }

                        bytesRead = waveStream.Read(waveData, 0, samplesPerPixel * bytesPerSample);
                       

                        if (bytesRead == 0)
                            break;
                        for (int n = 0; n < bytesRead; n += 2)
                        {
                            short sample = 0;
                            sample = BitConverter.ToInt16(waveData, n);
                          
                            if (sample < low) low = sample;
                            if (sample > high) high = sample;
                        }

                        float lowPercent = ((((float) low) - short.MinValue) / ushort.MaxValue);
                        float highPercent = ((((float) high) - short.MinValue) / ushort.MaxValue);

                        string xx = waveData[qwe].ToString();
                        string yy = "";
                        if (bytes != null)
                        {
                             yy = bytes[qwe].ToString();
                        }
                        if (tepmInt == 0)
                        {
                            waveDataTest[qwe] = new byte[waveData.Length];
                            for (int i = 0; i < waveData.Length; i++)
                            {
                                waveDataTest[qwe] [i] = waveData[i];
                            }
                        }

                        if (tepmInt >0 && waveDataTest[qwe] != null)
                        {
                           // byte[] temp = waveDataTest[qwe];
                            for (int i = 0; i < waveDataTest[qwe].Length; i++)
                            {
                                if (waveDataTest[qwe][i] != waveData[i])
                                {
                                    e.Graphics.DrawLine(new Pen(Color.Red, 0), x, 100 * lowPercent, x,
                                        100 * highPercent);
                                }
                                else
                                {
                                    e.Graphics.DrawLine(new Pen(Color.BlueViolet, 0), x, 100 * lowPercent, x, 100 * highPercent);
                                }
                            }
                        }
                        else
                        {
                            e.Graphics.DrawLine(new Pen(Color.Blue,0), x, 100 * lowPercent, x, 100 * highPercent);
                        }
                        qwe++;
                    }
                }
                tepmInt++;
                bytes = waveData;
              

                for (int i = 0; i < waveData.Length; i++)
                {
                    if (bytes[i] != waveData[i])
                    {
                        Console.WriteLine(i);
                    }
                }
            }

            base.OnPaint(e);
        }


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CustomWaveViewer
            // 
            this.Name = "CustomWaveViewer";
            this.ResumeLayout(false);

        }
        #endregion
    }
}