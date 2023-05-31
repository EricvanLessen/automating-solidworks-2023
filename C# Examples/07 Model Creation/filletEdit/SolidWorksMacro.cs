using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace filletEdit
{
    public partial class SolidWorksMacro
    {
public void Main()
{
    ModelDoc2 swDoc;
    Feature MyFeature;
    bool retval;

    swDoc = (ModelDoc2)swApp.ActiveDoc;

    //get the first feature
    MyFeature = (Feature)swDoc.FirstFeature();

    //loop through remaining features
    string radius;
    object featureDef;
    radius = Interaction.InputBox("Suppress all fillets < or = (in meters)");
    if(!IsNumeric(radius)){ 
        MessageBox.Show("Please enter only numeric values.", "Feature Edit", 
            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
    }

    while (MyFeature != null) {
        if (MyFeature.GetTypeName2() == "Fillet")
        {
            //MessageBox.Show("Found: " + MyFeature.Name);
            featureDef = (SimpleFilletFeatureData2)MyFeature.GetDefinition();
            SimpleFilletFeatureData2 filletfeat = (SimpleFilletFeatureData2)featureDef;
    if (filletfeat.DefaultRadius <= Convert.ToDouble(radius))
            {
                MyFeature.Select2(false, 0);
                swDoc.EditSuppress2();
            }            
        }
        //MessageBox.Show("Feature: " + MyFeature.Name + System.Environment.NewLine
        //        + "Feature Type: " + MyFeature.GetTypeName2());
        MyFeature = (Feature)MyFeature.GetNextFeature();
    }

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

