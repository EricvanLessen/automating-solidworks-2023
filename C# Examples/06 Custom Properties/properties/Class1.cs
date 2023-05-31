using System;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Windows.Forms;

internal static partial class Class1
{
public static string[] PropName;
public static string[] PropVal;
private static CustomPropertyManager PropMgr;
public static SldWorks m_swApp;

    public static void CreateTestProps()
    {

        // Property names
        PropName = new string[] {"LastSavedBy", "CreatedOn",
        "Revision", "Material"};

        // Property values
        PropVal = new string[] { "$PRP:\"SW-Last Saved By\"", 
            DateTime.Today.ToString(), "A", "\"SW-Material\""};
    }

    public static void SetAllProps()
    {
        // adds or sets all properties from PropName and PropVal arrays
        ModelDoc2 Part = (ModelDoc2)m_swApp.ActiveDoc;
        CustomPropertyManager PropMgr;
        int value;
        PropMgr = Part.Extension.get_CustomPropertyManager("");
        for (int m = 0, loopTo = PropName.Length - 1; m <= loopTo; m++)
            value = PropMgr.Add3(PropName[m], (int)swCustomInfoType_e.swCustomInfoText, PropVal[m], 
                (int)swCustomPropertyAddOption_e.swCustomPropertyReplaceValue);
    }

public static void ReadFileProps()
{
    ModelDoc2 Part = (ModelDoc2) m_swApp.ActiveDoc;
    // make sure that Part is not nothing
    if (Part == null)
    {
        MessageBox.Show("Please open a file first.",  "Properties", 
        MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
        return;
    }

    // get the custom property manager        
    PropMgr = Part.Extension.get_CustomPropertyManager("");

    // resize the PropVal array
    int propCt = PropMgr.Count;
    PropVal = new string[propCt];
    PropName = new string[propCt];
    string[] propNames = (string[])PropMgr.GetNames();
    // fill in the array of properties
    for (int k = 0; k < propCt; k++)
    {
        PropName[k] = propNames[k];
        bool resolved;
        bool linked;
        string resolvedVal;
        PropMgr.Get6(PropName[k], false,out PropVal[k],out resolvedVal, out resolved,out linked);
    }

}

    public static void LoadForm(SldWorks swapp)
    {
        // set the local SolidWorks variable to the running SolidWorks instance
        m_swApp = swapp;
        // read the properties from the file
        ReadFileProps();

        // initialize the new form
        var PropDia = new properties.PropsDialog();
        // if there are properties, fill in the controls
        if (PropName.Length > 0)
        {
            // load the listbox with the property names
            PropDia.PropsListBox.Items.AddRange(PropName);
            // set the first item in the list te be active
            PropDia.PropsListBox.SelectedItem = 0;
            // show the first item's value in the text box
            PropDia.ValueTextBox.Text = PropVal[0];
        }
        // show the form to the user
        DialogResult DiaRes;
        DiaRes = PropDia.ShowDialog();
    }

    public static void AddProperty(string Name, string Value)
    {
        PropMgr.Add3(Name, (int)swCustomInfoType_e.swCustomInfoText, Value, 
            (int)swCustomPropertyAddOption_e.swCustomPropertyReplaceValue);

        // re-read the properties arrays
        ReadFileProps();
    }

    public static void DeleteProperty(string Name)
    {
        PropMgr.Delete2(Name);

        // re-read the properties arrays
        ReadFileProps();
    }
}