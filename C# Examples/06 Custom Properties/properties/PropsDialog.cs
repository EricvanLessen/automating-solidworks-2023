using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;

namespace properties
{
    public partial class PropsDialog : Form
    {
        public PropsDialog()
        {
            InitializeComponent();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            //set the property value

            Class1.PropVal[PropsListBox.SelectedIndex] = ValueTextBox.Text;
            Class1.SetAllProps();

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void PropsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValueTextBox.Text = Class1.PropVal[PropsListBox.SelectedIndex];

        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string NewName;
            string NewVal;
            NewName = Interaction.InputBox("Enter new property name:");
            NewVal = Interaction.InputBox("Enter property value for" + NewName + ":");

            //add it to the file and update the array
            Class1.AddProperty(NewName, NewVal);
            RefreshForm();
        }

        private void RefreshForm()
        {
            //reset list of properties displayed to the user
            PropsListBox.Items.Clear();
            PropsListBox.Items.AddRange(Class1.PropName);
            PropsListBox.SelectedIndex = Class1.PropName.Length - 1;
            this.Refresh();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            //get the list's selected item to know what to delete
            string PropToDelete = (string)PropsListBox.SelectedItem;

            //verify deletion from the user
            DialogResult answer = MessageBox.Show("Delete " + PropToDelete + "?",
                "Properties", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.Yes)
            {
                Class1.DeleteProperty(PropToDelete);
            }
            RefreshForm();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string FileName = "savedprops.txt";
            try
            {
                StreamWriter sw = new StreamWriter(FileName);
                for (int i = 0; i <= Class1.PropName.Length - 1; i++)
                {
                    sw.WriteLine(Class1.PropName[i]);
                    sw.WriteLine(Class1.PropVal[i]);
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Properties", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {

            string FileName = "savedprops.txt";
            StreamReader sr = new StreamReader(FileName);
            int i = 0;
            while (!sr.EndOfStream)
            {
                Array.Resize(ref Class1.PropName, i);
                Array.Resize(ref Class1.PropVal, i);
                Class1.PropName[i] = sr.ReadLine();
                Class1.PropVal[i] = sr.ReadLine();
                i++;
            }

            //write the properties to the part
            Class1.SetAllProps();
            RefreshForm();
        }
    }
}
