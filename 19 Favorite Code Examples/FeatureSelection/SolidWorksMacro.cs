using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Diagnostics;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace FeatureSelection
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

            PartDoc Part = (PartDoc)Model;
            // get the Extension interface from the model
            ModelDocExtension Ext = Model.Extension;

            // get a feature in the FeatureManager
            // assuming a feature named "Extrude1"
            Feature feat = (Feature)Part.FeatureByName("Extrude1");
            if (feat != null)
            {
                // select it
                feat.Select2(false, -1);
                // get persistent ID
                int IDCount = Ext.GetPersistReferenceCount3(feat);
                object ID = null;
                ID = Ext.GetPersistReference3(feat);
                Debugger.Break(); // check selected feature and ID

                // get rid of the feature reference
                feat = null;
                // clear the selection
                Model.ClearSelection2(true);
                // select the feature by its ID
                int errors = 0;
                // get the feature by its persistent ID
                feat = (Feature)Ext.GetObjectByPersistReference3(ID, out errors);
                feat.Select2(false, -1);
                Debugger.Break(); // check selected feature
            }
        }


        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

