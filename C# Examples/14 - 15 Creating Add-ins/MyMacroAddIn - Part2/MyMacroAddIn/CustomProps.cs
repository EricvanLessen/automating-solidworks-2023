using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolidWorks.Interop.sldworks;

namespace MyMacroAddIn
{
    class CustomProps
    {
        public bool PropExists(ModelDoc2 swDoc,
            string propName, string config = "")
        {
            CustomPropertyManager cpm;
            cpm = swDoc.Extension.CustomPropertyManager[config];
            string val = cpm.Get(propName);
            if (val == "")
            {
                return false;
            }
            return true;
        }
    }
}
