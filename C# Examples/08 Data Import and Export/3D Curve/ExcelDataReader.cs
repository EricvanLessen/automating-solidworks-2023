using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SolidWorks.Interop.swconst;

namespace _3D_Points
{
    public class ExcelDataReader
    {
        dynamic excel;
        public ExcelDataReader()
        {
            excel = Marshal.GetActiveObject("Excel.Application");
        }

        public List<Point> getExcelPoints(swLengthUnit_e units)
        {
            int i = 1;
            double multiplier = 1;
            List<Point> points = new List<Point>();
            if (units == swLengthUnit_e.swINCHES) { multiplier = Utilities.inchToMeters(1); }
            if (units == swLengthUnit_e.swMM) { multiplier = Utilities.mmToMeters(1); }
            while (excel.Cells(i, 1).Value != null)
            {
                Point p = new Point();
                p.X = excel.Cells(i, 1).Value * multiplier;
                p.Y = excel.Cells(i, 2).Value * multiplier;
                p.Z = excel.Cells(i, 3).Value * multiplier;
                points.Add(p);
                i++;
            }
            return points;
        }

    }
}
