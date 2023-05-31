using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatchProcess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = textBoxSource.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBoxSource.Text = fbd.SelectedPath;
            }
        }

        private void buttonBrowse2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = textBoxOutput.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBoxOutput.Text = fbd.SelectedPath;
            }
        }
    }
}
