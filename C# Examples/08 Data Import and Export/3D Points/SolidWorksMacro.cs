using SolidWorks.Interop.sldworks;
using System.Runtime.InteropServices;

namespace _3D_Points
{
    public partial class SolidWorksMacro
    {
public void Main()
{
    ModelDoc2 swDoc = null;
    swDoc = ((ModelDoc2)(swApp.ActiveDoc));
    swDoc.SketchManager.Insert3DSketch(true);
    swDoc.SketchManager.AddToDB = true;

    ExcelDataReader edr = new ExcelDataReader();
    foreach (Point p in edr.getExcelPoints(SolidWorks.Interop.swconst.swLengthUnit_e.swINCHES))
    {
        swDoc.SketchManager.CreatePoint(p.X, p.Y, p.Z);
    }

    swDoc.SketchManager.InsertSketch(true);
    swDoc.SketchManager.AddToDB = false;
    swDoc.ClearSelection2(true);
    swDoc.ViewZoomtofit2();
 
}        

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

