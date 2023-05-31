using System;
using System.Windows.Forms;

namespace AddComponents
{
    public partial class Dialog1 : Form
    {
        public Dialog1()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenDia = new OpenFileDialog();
            OpenDia.Filter = "SOLIDWORKS Part (*.sldprt)|*.sldprt";
            DialogResult diaRes = OpenDia.ShowDialog();
            if (diaRes == DialogResult.OK)
            {
                textBox1.Text = OpenDia.FileName;
            }
        }

        private void Dialog1_Load(object sender, EventArgs e)
        {

        }

        private void Dialog1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            AddComps.AddCompAndMate(textBox1.Text);
        }
    }
}
