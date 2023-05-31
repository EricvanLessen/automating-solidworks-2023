
using System.Diagnostics;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace AssemblyTraversal
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {
            ModelDoc2 swModel;
            Configuration swConf;
            Component2 swRootComp;
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swConf = (Configuration)swModel.GetActiveConfiguration();
            swRootComp = (Component2)swConf.GetRootComponent();
            Debug.Print("File = " + swModel.GetPathName());
            TraverseComponent(swRootComp, 1);
        }

        public void TraverseComponent(Component2 swComp, int nLevel)
        {
            object[] ChildComps;
            Component2 swChildComp;
            string sPadStr = "";                        
            for (int i = 0; i < nLevel; i++)
                sPadStr = sPadStr + "  ";
            ChildComps = (object[])swComp.GetChildren();

            for (int i = 0; i < ChildComps.Length; i++)
            {
                swChildComp = (Component2)ChildComps[i];
                TraverseComponent(swChildComp, nLevel + 1);
                Debug.Print(sPadStr + swChildComp.Name2 + " <" + swChildComp.ReferencedConfiguration + ">");
            }
        }


        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

