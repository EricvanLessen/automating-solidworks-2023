using SolidWorks.Interop.sldworks;

using SWDMProperties; //namespace from shared MyDocMan.cs

namespace SWDMReferences
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {
            MyDocMan MyDM = new MyDocMan();

            //browse for a file
            string filePath = MyDM.BrowseForDoc();
            MyDM.GetRefs(filePath, true);
            return;
        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

