using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace ExportFlatPattern
{
    public partial class SolidWorksMacro
    {
public void Main()
{
    ModelDoc2 swDoc = null;
    PartDoc swPart = null;

    //get the active document
    swDoc = (ModelDoc2)swApp.ActiveDoc;
    //exit if not a part
    if (swDoc.GetType() != (int)swDocumentTypes_e.swDocPART)
    {
        swApp.SendMsgToUser2("This macro is for parts only.", 
            (int)swMessageBoxIcon_e.swMbWarning, (int)swMessageBoxBtn_e.swMbOk);
        return;
    }
    //get the part interface
    swPart = (PartDoc)swDoc;
    //set a new path for the flat pattern
    string FlatPatternPath;
    string ext = ".DXF";
    //could also use .DWG if prefered
    FlatPatternPath = System.IO.Path.ChangeExtension(swDoc.GetPathName(), ext);
    //export the flat pattern with bend lines
    int bendSetting = 2 ^ 0 + 2 ^ 2;
    //int bendSetting = 2 ^ 0 //use this setting for no bend lines
    swPart.ExportToDWG2(FlatPatternPath, swDoc.GetTitle(), 
        (int)swExportToDWG_e.swExportToDWG_ExportSheetMetal,
        true, null, false, false, bendSetting, null);
    return;
}

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

