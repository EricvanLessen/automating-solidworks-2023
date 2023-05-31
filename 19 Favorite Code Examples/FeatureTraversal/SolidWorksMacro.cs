using System.Diagnostics;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace FeatureTraversal
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {
            ModelDoc2 Model = (ModelDoc2)swApp.ActiveDoc;
            // make sure the active document is a part
            if (Model == null)
            {
                swApp.SendMsgToUser2("Please open a part first.", 
                    (int)swMessageBoxIcon_e.swMbWarning, 
                    (int)swMessageBoxBtn_e.swMbOk);
                return;
            }
            else if (Model.GetType() != (int)swDocumentTypes_e.swDocPART)
            {
                swApp.SendMsgToUser2("For parts only.",
                    (int)swMessageBoxIcon_e.swMbWarning, 
                    (int)swMessageBoxBtn_e.swMbOk);
                return;
            }
            // get the PartDoc interface from the model
            PartDoc Part = (PartDoc)Model;

            // get the first feature in the FeatureManager
            Feature feat = (Feature)Part.FirstFeature();
            string featureName;
            string featureTypeName;
            Feature subFeat = null;
            string subFeatureName;
            string subFeatureTypeName;
            string message;

            // While we have a valid feature
            while (feat != null)
            {
                // Get the name of the feature
                featureName = feat.Name;
                featureTypeName = feat.GetTypeName2();
                message = "Feature: " + featureName + "\nFeatureType: " 
                    + featureTypeName + "\n SubFeatures:";


                // get the feature's sub features
                subFeat = (Feature)feat.GetFirstSubFeature();

                // While we have a valid sub-feature
                while (subFeat is object)
                {

                    // Get the name of the sub-feature
                    // and its type name
                    subFeatureName = subFeat.Name;
                    subFeatureTypeName = subFeat.GetTypeName2();
                    message += "\n " + subFeatureName + "\n " 
                        + "Type: " + subFeatureTypeName;

                    subFeat = (Feature)subFeat.GetNextSubFeature();
                    // Continue until the last sub-feature is done
                }

                // Display the sub-features in the 
                Debug.Print(message);
                // Get the next feature
                feat = (Feature)feat.GetNextFeature();
                // Continue until the last feature is done
            }
        }


        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

