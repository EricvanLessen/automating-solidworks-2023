using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace materials
{
    public partial class SolidWorksMacro
    {
public void main()
{
    // Initialize the dialog
    var MyMaterials = new Dialog1();
    ComboBox MyCombo;
    MyCombo = MyMaterials.MaterialsCombo;

            // Set up materials list
    var MyProps = new string[] { "Alloy Steel", "6061 Alloy", "ABS PC"};
    MyCombo.Items.AddRange(MyProps);
    DialogResult Result;
    Result = MyMaterials.ShowDialog();
    if (Result == DialogResult.OK)
    {
        ModelDoc2 Model = (ModelDoc2)swApp.ActiveDoc;
        if (Model == null)
        {
            MessageBox.Show("You must first open a file.", 
                "Materials", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if ((int)Model.GetType() == (int)swDocumentTypes_e.swDocPART)
        {
            // Assign the material to the part
            PartDoc Part = (PartDoc)Model;
            // Part = swApp.ActiveDoc
            Part.SetMaterialPropertyName2("Default", "SOLIDWORKS Materials.sldmat", MyCombo.Text);
        }
        else if ((int)Model.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
        {
            AssemblyDoc Assy = (AssemblyDoc)Model;
            // set materials on selected components
            SelectionMgr SelMgr;
            SelMgr = (SelectionMgr)Model.SelectionManager;
            Component2 Comp;
            ModelDoc2 compModel;
            if (SelMgr.GetSelectedObjectCount2(-1) < 1)
            {
                MessageBox.Show("You must select at least one component.", "Materials", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            for (int i = 1, loopTo = SelMgr.GetSelectedObjectCount2(-1); i <= loopTo; i++)
            {
                Comp = (Component2)SelMgr.GetSelectedObjectsComponent4(i, -1);
                compModel = (ModelDoc2)Comp.GetModelDoc2();
                if (compModel.GetType() == (int)swDocumentTypes_e.swDocPART)
                {
                    PartDoc swPart = (PartDoc)compModel;
                    swPart.SetMaterialPropertyName2("Default", "SOLIDWORKS Materials.sldmat", MyCombo.Text);
                }
            }
        }
    }
}


        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

