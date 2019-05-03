using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tutorial11
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openWaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Wave File (*.wav)|*.wav";
            if (open.ShowDialog() != DialogResult.OK) return;

            customWaveViewer1.WaveStream = new NAudio.Wave.WaveFileReader(open.FileName);
            customWaveViewer1.FitToScreen();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void customWaveViewer1_Load(object sender, EventArgs e)
        {

        }

        private void openWave2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Wave File (*.wav)|*.wav";
            if (open.ShowDialog() != DialogResult.OK) return;

            customWaveViewer1.WaveStream = new NAudio.Wave.WaveFileReader(open.FileName);
            customWaveViewer1.FitToScreen();

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
