using System.Windows.Forms;
using SolidWorks.Interop.sldworks;

namespace BatchProcess
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {
            Form1 frm = new Form1();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                string filePath = frm.textBoxSource.Text;
                string outputPath = frm.textBoxOutput.Text;
                Batch.swApp = swApp;  //pass the SOLIDWORKS application to the static class
                Batch.Process(filePath, outputPath);
            }

            return;
        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

