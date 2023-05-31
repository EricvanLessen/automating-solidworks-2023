using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Points
{
    static class Utilities
    {
        public static double inchToMeters(double InchVal)
        {
            return InchVal * 0.0254;
        }

        public static double mmToMeters(double mmVal)
        {
            return mmVal / 1000;
        }
    }
}
