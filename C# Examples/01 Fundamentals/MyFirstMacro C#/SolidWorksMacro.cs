using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace MyFirstMacro_C_
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {

            string message;
            int swVersion;
            ModelDoc2 swDoc;
            string swTitle;
            swDoc = (ModelDoc2)swApp.ActiveDoc;
            if (swDoc == null)
            {
                return;
            }
            swTitle = swDoc.GetTitle();
            swVersion = swApp.DateCode();
            //message = "Hello SOLIDWORKS " + swVersion.ToString()
            //    + System.Environment.NewLine + "Document: " + swTitle
            //    + System.Environment.NewLine
            //    + "Would you like to close the active document?";
            //message = "Hello SOLIDWORKS " + swVersion.ToString() + "\nDocument: " + swTitle
            //    + "\nWould you like to close the active document?";
            message = string.Format("Hello SOLDIWORKS {0} \nDocument: {1}\nWould you like to close the active document?", swVersion.ToString(), swTitle);
            //show the message
            DialogResult diaRes;
            diaRes = MessageBox.Show(message, "Close document",
            MessageBoxButtons.YesNo);
            if (diaRes == DialogResult.Yes)
            {
                swApp.CloseDoc(swTitle);
            }
            return;

        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

