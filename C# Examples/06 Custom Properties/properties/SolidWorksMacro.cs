using SolidWorks.Interop.sldworks;

namespace properties
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {

            
            Class1.LoadForm(swApp);

            return;
        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

