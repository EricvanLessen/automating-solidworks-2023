using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace background
{
    public partial class SolidWorksMacro
    {
public void Main()
{
    Boolean result;
    ModelDoc2 swModel;
    swModel = ((ModelDoc2)(swApp.ActiveDoc));
    int bgd = swApp.GetUserPreferenceIntegerValue((int)
        swUserPreferenceIntegerValue_e.swColorsBackgroundAppearance);

    if(bgd == (int)swColorsBackgroundAppearance_e
                .swColorsBackgroundAppearance_DocumentScene)
    {
        result = swApp.SetUserPreferenceIntegerValue((int)
            swUserPreferenceIntegerValue_e.swColorsBackgroundAppearance, 
            (int)swColorsBackgroundAppearance_e
            .swColorsBackgroundAppearance_DocumentScene);
    }
    swModel.GraphicsRedraw2();
    return;
}

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

