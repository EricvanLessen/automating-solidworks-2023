using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace SavePDF
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {
            ModelDoc2 swDoc = null;
            int longstatus = 0;
            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            string FilePath = "";
            string NewFilePath = "";
            FilePath = swDoc.GetPathName();
            NewFilePath = System.IO.Path.ChangeExtension(FilePath, ".PDF");
            //
            // Save As
            longstatus = swDoc.SaveAs3(NewFilePath, 0, 0);
            MessageBox.Show("Saved " + NewFilePath, "Save message",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

