using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace options
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {
            //example useage...
            toggleInputDimVal();
            increaseDecimalPlacesByOne();
            decreaseDecimalPlacesByOne();
            toggleBackgroundAppearance();
        }

        public void toggleBackgroundAppearance()
        {
            Boolean result;
            ModelDoc2 swModel;
            swModel = ((ModelDoc2)(swApp.ActiveDoc));
            int bgd = swApp.GetUserPreferenceIntegerValue((int)
            swUserPreferenceIntegerValue_e.swColorsBackgroundAppearance);
            if (bgd == (int)swColorsBackgroundAppearance_e
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
        public void toggleInputDimVal()
        {
            Boolean OldSetting = false;
            OldSetting = swApp.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate);            
            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, !OldSetting);
            return;
        }

        public void increaseDecimalPlacesByOne()
        {
            ModelDoc2 swDoc = null;
            ModelDocExtension swDocExtension = null;
            swDoc = (ModelDoc2)swApp.ActiveDoc;
            swDocExtension = (ModelDocExtension)swDoc.Extension;
            int CurrentSetting = 0;
            int NewSetting = 0;
            CurrentSetting = (int)swDocExtension.GetUserPreferenceInteger
                ((int)swUserPreferenceIntegerValue_e.swUnitsLinearDecimalPlaces, 
                (int)swUserPreferenceOption_e.swDetailingDimension);
            NewSetting = CurrentSetting + 1;

            swDocExtension.SetUserPreferenceInteger
                ((int)swUserPreferenceIntegerValue_e.swUnitsLinearDecimalPlaces, 
                (int)swUserPreferenceOption_e.swDetailingDimension, NewSetting);

            return;
        }

        public void decreaseDecimalPlacesByOne()
        {
            ModelDoc2 swDoc = null;
            ModelDocExtension swDocExtension = null;
            swDoc = (ModelDoc2)swApp.ActiveDoc;
            swDocExtension = (ModelDocExtension)swDoc.Extension;
            int CurrentSetting = 0;
            int NewSetting = 0;
            CurrentSetting = (int)swDocExtension.GetUserPreferenceInteger
                ((int)swUserPreferenceIntegerValue_e.swUnitsLinearDecimalPlaces,
                (int)swUserPreferenceOption_e.swDetailingDimension);
            NewSetting = CurrentSetting - 1;

            swDocExtension.SetUserPreferenceInteger
                ((int)swUserPreferenceIntegerValue_e.swUnitsLinearDecimalPlaces,
                (int)swUserPreferenceOption_e.swDetailingDimension, NewSetting);

            return;
        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

