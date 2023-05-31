using System;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using Microsoft.VisualBasic;
using System.Linq;

namespace PartModel
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
            // 
            // New Document
            double swSheetWidth;
            swSheetWidth = 0;
            double swSheetHeight;
            swSheetHeight = 0;
            //swDoc = ((ModelDoc2)(swApp.NewDocument("C:\\...Path Required...\\Part.prtdot", 0, swSheetWidth, swSheetHeight)));
            swDoc = (ModelDoc2)swApp.NewPart();
            swPart = (PartDoc)swDoc;
            //disable graphics updating for this part
            ModelView mv = (ModelView)swDoc.ActiveView;
            mv.EnableGraphicsUpdate = false;

            //store the user's setting
            bool usersSetting = swApp.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate);
            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false);

            //create sketch     
            double length;
            double xLoc;
            double yLoc;
            string userLength;
            userLength = Interaction.InputBox("Enter the line length in meters:");
            if (!IsNumeric(userLength))
            {
                System.Windows.Forms.MessageBox.Show
                    ("Please enter only values in meters.", "PartModel", 
                    System.Windows.Forms.MessageBoxButtons.OK, 
                    System.Windows.Forms.MessageBoxIcon.Exclamation);
                return;
            }
            length = Convert.ToDouble(userLength);
            boolstatus = swDoc.Extension.SelectByID2("Front Plane", "PLANE", 0, 0, 0, false, 0, null, 0);
            swDoc.SketchManager.InsertSketch(true);
            swDoc.ClearSelection2(true);
            SketchSegment skSegment = null;
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateLine(0D, 0D, 0D, 0D, length, 0D)));
            DisplayDimension myDisplayDim = null;
            yLoc = length / 2;
            xLoc = -yLoc;
            myDisplayDim = ((DisplayDimension)(swDoc.AddDimension2(xLoc, yLoc, 0)));
            Dimension myDimension = null;
            //myDimension = ((Dimension)(swDoc.Parameter("D1@Sketch1")));
            //better technique that does not rely on dimension name
            //get the dimension from the DisplayDimension that was just created
            myDimension = (Dimension)myDisplayDim.GetDimension();
            myDimension.SystemValue = length;
            xLoc = length / 2;
            yLoc = -xLoc;
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateLine(0D, 0D, 0D, length, 0D, 0D)));
            myDisplayDim = ((DisplayDimension)(swDoc.AddDimension2(xLoc, yLoc, 0)));
            //myDimension = ((Dimension)(swDoc.Parameter("D2@Sketch1")));
            myDimension = (Dimension)myDisplayDim.GetDimension();
            myDimension.SystemValue = length;
            // 
            // Named View
            swDoc.ShowNamedView2("*Trimetric", 8);

            //create extrusion
            Feature myFeature = null;
            myFeature = ((Feature)(swDoc.FeatureManager.FeatureExtrusionThin2(true, false, false, 0, 0, length, length, false, false, false, false, 0.017453292519943334D, 0.017453292519943334D, false, false, false, false, true, 0.01D, 0.01D, 0.01D, 0, 0, false, 0.005D, true, true, 0, 0, false)));
            //swDoc.ISelectionManager.EnableContourSelection = false;

            //sketch rectangle
            boolstatus = swDoc.Extension.SelectByRay(0.052833363855540938D, 0, 0.040705551627127079D, -0.40003602677931249D, -0.51503807491002407D, -0.75809429405028372D, 0.00097004279600570629D, 2, false, 0, 0);
            swDoc.SketchManager.InsertSketch(true);
            Array vSkLines = null;
            vSkLines = ((Array)(swDoc.SketchManager.CreateCornerRectangle(length, -0.08474418720157928D, 0, 0.043267859458580915D, -0.022578298536018337D, 0)));
            myFeature = ((Feature)(swDoc.FeatureManager.FeatureCut4(true, false, false, 1, 0, 0.01D, 0.01D, false, false, false, false, 0.017453292519943334D, 0.017453292519943334D, false, false, false, false, false, true, true, true, true, false, 0, 0, false, false)));            

            //select face for hole wizard
            boolstatus = swDoc.Extension.SelectByRay(0, length /2, 0.0487889876318377D, -0.40003602677931249D, -0.51503807491002407D, -0.75809429405028372D, 0.00097004279600570629D, 2, false, 0, 0);
            // 
            // Hole Wizard
            object swHoleFeature = null;
            swHoleFeature = swDoc.FeatureManager.HoleWizard5(1, 1, 36, "M10", 
                1, 0.011D, 0.01D, -1, 0.02D, 1.5707963267948966D, 0, 1, 0, 0,
                0, 0, 0, -1, -1, -1, "", false, true, true, true, true, false);
            //extra code removed

            //add fillet
            swDoc.ClearSelection2(true);
            boolstatus = swDoc.Extension.SelectByRay(0.00038426606806751806D, -0.00029846389747945068D, 
                0.01833916875017394D, -0.40003602677931249D, -0.51503807491002407D, 
                -0.75809429405028372D, 0.00097004279600570629D, 1, false, 1, 0);
            SimpleFilletFeatureData2 swFeatData = null;
            swFeatData = ((SimpleFilletFeatureData2)(swDoc.FeatureManager.CreateDefinition((int)swFeatureNameID_e.swFmFillet)));
            // 
            swFeatData.Initialize((int)swSimpleFilletType_e.swConstRadiusFillet);
            // 
            //object swEdge = null;
            //object edgesArray[] = null;
            //swEdge = ((object)(swDoc.ISelectionManager.GetSelectedObject6(1, 1)));
            //Variant edgesVar;
            //swFeatData.Edges = edgesVar;
            //// 
            //swFeatData.AsymmetricFillet = false;
            //swFeatData.DefaultRadius = 0.01D;
            //swFeatData.ConicTypeForCrossSectionProfile = swFeatureFilletCircular;
            //swFeatData.CurvatureContinuous = false;
            //swFeatData.ConstantWidth = 0.01D;
            //swFeatData.IsMultipleRadius = false;
            //swFeatData.OverflowType = swFilletOverFlowType_Default;
            // 
            Feature swFeature = null;
            swFeature = ((Feature)(swDoc.FeatureManager.CreateFeature(swFeatData)));

            //reset the user's setting
            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, usersSetting);
            mv.EnableGraphicsUpdate = true;
            swDoc.GraphicsRedraw2();
            return;
        }

        //helper function to check string for numeric (double) value
        public bool IsNumeric(string value)
        {
            Double num = 0;
            bool isDouble = false;

            // Check for empty string.
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            isDouble = Double.TryParse(value, out num);

            return isDouble;
        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

