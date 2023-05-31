using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swdocumentmgr;

namespace SWDMProperties
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {
            MyDocMan MyDM = new MyDocMan();

            SwDMDocument dmDoc;
            string filePath;
            filePath = MyDM.BrowseForDoc();
            dmDoc = MyDM.OpenDoc(filePath, false);
            if (dmDoc == null){return;}
            //add or change Description
            MyDM.WriteProp(dmDoc, "Description", "MY DESCRIPTION");

            string props;
            props = MyDM.ReadAllProps(dmDoc);
            System.Diagnostics.Debug.Print(props);
            props = MyDM.ReadConfiProps(dmDoc);
            System.Diagnostics.Debug.Print(props);

            //save the document
            dmDoc.Save();
            //close the document
            dmDoc.CloseDoc();

            throw new ArgumentNullException("Look at property values in the Immediate Window");

            return;
        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

