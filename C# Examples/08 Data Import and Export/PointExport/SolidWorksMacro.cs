using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;

namespace PointExport
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {
            //assume the desired sketch is currently in edit mode
            ModelDoc2 model = (ModelDoc2)swApp.ActiveDoc;
            SketchManager skMgr = model.SketchManager;
            Sketch ptSketch = skMgr.ActiveSketch;
            if (ptSketch == null)
            {
                MessageBox.Show("Please edit the desired sketch before running.",
                    "PointExport", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                //initialize a string for the output file
                string outputString;
                //it will be tab delimited
                string headerRow = "X\tY\tZ";
                outputString = headerRow;

                //gather all sketch points
                object[] points = (object[])ptSketch.GetSketchPoints2();
                foreach (SketchPoint skPoint in points)
                {
                    double x = skPoint.X;
                    double y = skPoint.Y;
                    double z = skPoint.Z;

                    //add the text to the output string, starting with a return character
                    outputString += string.Format("\n{0}\t{1}\t{2}", 
                        x.ToString(), y.ToString(),z.ToString());
                }

                //save the resulting string to a file
                string filePath = System.IO.Path.ChangeExtension(model.GetPathName(), ".txt");
                System.IO.File.WriteAllText(filePath, outputString);
                //open the file
                Process.Start(filePath);
                    
            }
            return;
        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

