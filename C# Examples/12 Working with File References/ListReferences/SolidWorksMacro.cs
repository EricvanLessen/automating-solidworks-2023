using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Data;


namespace ListReferences
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {
            References.m_swApp = swApp;
            using (Dialog1 dialog = new Dialog1())
            {
                DataTable refs = References.ReadRefs();
                dialog.dataGridView1.DataSource = refs;
                dialog.ShowDialog();
            }

            return;
        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

