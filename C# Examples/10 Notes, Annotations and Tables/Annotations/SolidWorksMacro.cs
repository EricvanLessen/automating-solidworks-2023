using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace Annotations
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {
            
            ModelDoc2 swDoc = null;
            PartDoc swPart = null;
            DrawingDoc swDrawing = null;
            AssemblyDoc swAssembly = null;
            bool boolstatus = false;
            int longstatus = 0;
            int longwarnings = 0;
            swDoc = ((ModelDoc2)(swApp.ActiveDoc));


            //myAnnotations.AddDatum(swDoc);
            //myAnnotations.AddFileNameNote(swDoc);
            //myAnnotations.InsertGeneralTable(swDoc);

            BomTableAnnotation BOM;
            BOM = myAnnotations.InsertBOMTable(swDoc);
            myAnnotations.ReadBOMTable(BOM);

            throw new ArgumentException("Check the Immediate window for BOM data.");
            
            return;
        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

