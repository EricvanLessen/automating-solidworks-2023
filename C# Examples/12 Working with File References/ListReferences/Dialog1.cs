
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;

namespace ListReferences
{
    public partial class Dialog1 : Form
    {
        public Dialog1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 0)
            {
                string filename;
                filename = (string)dataGridView1.CurrentCell.Value;
                References.ShowModel(filename);
            }

        }

        private void SaveButton_Click(object sender, System.EventArgs e)
        {
            string DefaultFilePath = dataGridView1.Rows[0].Cells[0].Value + ".REFS.txt";            
            using (SaveFileDialog saveDia = new SaveFileDialog())
            {
                saveDia.FileName = Path.GetFileName(DefaultFilePath);
                saveDia.InitialDirectory = Path.GetDirectoryName(DefaultFilePath);
                saveDia.Filter = "Text file (*.txt)|*.txt";
                if (saveDia.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string output = References.FilesTableToString();
                        File.WriteAllText(saveDia.FileName, output);

                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message, "List References", 
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;                        
                    }
                    if (MessageBox.Show("Open file now?", "List References", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Interaction.Shell("notepad " + saveDia.FileName, AppWinStyle.NormalFocus);
                    }
                    
                }
                
            }

        }
    }
}
