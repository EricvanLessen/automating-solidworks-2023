using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace FeatureEdit
{
    public partial class SolidWorksMacro
    {
public void Main()
{
    PartDoc Part;
    Feature MyFeature;
    ExtrudeFeatureData2 featureDef;
    bool retval;
    string message = "";

    Part = (PartDoc)swApp.ActiveDoc;

    MyFeature = (Feature)Part.FeatureByName("Cut-Extrude1");
    featureDef = (ExtrudeFeatureData2)MyFeature.GetDefinition();

    //get some settings from the feature
    if (!featureDef.GetDraftWhileExtruding(true))
    {
        message = "The selected feature has no draft.\n";
    }

    switch (featureDef.GetEndCondition(true))
    {
        case (int)swEndConditions_e.swEndCondBlind:
            message += "Blind";
            break;
        case (int)swEndConditions_e.swEndCondThroughAll:
            message += "Through All";
            break;
        case (int)swEndConditions_e.swEndCondThroughAllBoth:
            message += "Through All Both";
            break;
        case (int)swEndConditions_e.swEndCondUpToSurface:
            message += "Up To Surface";
            break;
        case (int)swEndConditions_e.swEndCondMidPlane:
            message += "Mid Plane";
            break;
        case (int)swEndConditions_e.swEndCondOffsetFromSurface:
            message += "Offset From Surface";
            break;
        case (int)swEndConditions_e.swEndCondThroughNext:
            message += "Through Next";
            break;
        case (int)swEndConditions_e.swEndCondUpToBody:
            message += "Up To Body";
            break;
        default:
            break;
    }

    MessageBox.Show(message + " end condition.");

    //rollback to edit the feature
    retval = featureDef.AccessSelections(Part, null);
    //modify some feature values 
    featureDef.SetEndCondition(true,
        (int)swEndConditions_e.swEndCondThroughAll);
    featureDef.SetDraftWhileExtruding(true, true);
    featureDef.SetDraftAngle(true, 2 * Math.PI / 180);

    //complete the edit operation
    retval = MyFeature.ModifyDefinition(featureDef, Part, null);

    //in case the modification failed
    if(!retval){
        featureDef.ReleaseSelectionAccess();
    }       

    return;
}

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

