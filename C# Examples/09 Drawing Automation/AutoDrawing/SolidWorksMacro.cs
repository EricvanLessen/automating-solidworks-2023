using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace AutoDrawing
{
    public partial class SolidWorksMacro
    {
public void main()
{
    //browse for a folder to process
    FolderBrowserDialog fbd = new FolderBrowserDialog;
    DialogResult myRes;
    fbd.Description = "Select a folder...";
    myRes = fbd.ShowDialog();
    string MyPath;
    if (myRes == DialogResult.OK)
    {
        MyPath = fbd.SelectedPath;
    }
    else
    {
        //no folder selected
        return;
    }
    //set the directory for parts
    DirectoryInfo MyDir = new DirectoryInfo(MyPath);
    if (MyDir.Exists)
    {
        //enter a valid template path
        string templatePath = "C:\\...\\Drawing.drwdot";
        FileInfo[] AllParts;
        AllParts = (FileInfo[])MyDir.GetFiles("*.sldprt");
        foreach (FileInfo MyFile in AllParts)
        {
            //Write the file path to the Immediate Window
            Debug.Print(MyFile.FullName);
            //open the model
            ModelDoc2 swDoc = openModel(MyFile.FullName);
            //create the drawing
            DrawingFromModel(templatePath, MyFile.FullName);
        }
    }            
}

private ModelDoc2 openModel(string modelPath)
{
    int docType;
    int errors = 0;
    int warnings = 0;
    switch (Path.GetExtension(modelPath).ToUpper())
    {
        case ".SLDPRT":
            docType = (int)swDocumentTypes_e.swDocPART;
            break;
        case ".SLDASM":
            docType = (int)swDocumentTypes_e.swDocASSEMBLY;
            break;
        case ".SLDDRW":
            docType = (int)swDocumentTypes_e.swDocDRAWING;
            break;
        default:
            docType = (int)swDocumentTypes_e.swDocNONE;
            break;
    }
    ModelDoc2 swDoc = swApp.OpenDoc6(modelPath, docType,
        (int)swOpenDocOptions_e.swOpenDocOptions_Silent, 
        "", errors, warnings);
    return swDoc;
}
public void DrawingFromModel(string drawingTemplate, string modelPath)
{
    int longstatus = 0;
    bool boolstatus;

    // 
    // New Document
    ModelDoc2 swDoc = null;
    swDoc = ((ModelDoc2)(swApp.NewDocument(drawingTemplate,0,0,0)));
    DrawingDoc swDrawing = null;
    swDrawing = (DrawingDoc)swDoc;
    boolstatus = swDrawing.Create3rdAngleViews(modelPath);
    SolidWorks.Interop.sldworks.View myView = null;
    myView = swDrawing.CreateDrawViewFromModelView3(modelPath, 
        "*Isometric", 0.451104, 0.2954, 0);            
    Array vAnnotations = null;            
    vAnnotations = ((Array)(swDrawing.InsertModelAnnotations3(0, 
        32776, true, true, false, true)));
    // 
    // Zoom To Fit
    swDoc.ViewZoomtofit2();
    // 
    // Save As
    string drawingPath = Path.ChangeExtension(modelPath, ".SLDDRW");
    longstatus = swDoc.SaveAs3(drawingPath, 0, 0);
    swApp.CloseDoc(drawingPath);
    swApp.CloseDoc(modelPath);
    return;
}

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

