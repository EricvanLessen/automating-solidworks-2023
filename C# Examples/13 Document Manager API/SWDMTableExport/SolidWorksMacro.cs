using System;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swdocumentmgr;
using SWDMProperties;
using System.IO;

namespace SWDMTableExport
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {
            MyDocMan MyDM = new MyDocMan();

            //open the document for read/write
            SwDMDocument dmDoc;
            string filePath = MyDM.BrowseForDoc();
            dmDoc = MyDM.OpenDoc(filePath, false);
            if (dmDoc == null)
            {
                return;
            }

            string tableText = MyDM.GetBOMTableText((SwDMDocument10)dmDoc);

            //close the document
            dmDoc.CloseDoc();

            throw new ArgumentNullException("Look at tableText in the watch Text Visualizer");
            string bomFile = Path.ChangeExtension(
                dmDoc.FullName, ".BOM.txt");
            File.WriteAllText(bomFile, tableText);

            return;
        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

