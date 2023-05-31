Friend Module ExcelDataReader
    Function getExcelPoints() As List(Of Double())
        Dim Excel As Object = GetObject(, "Excel.Application")
        Dim i As Integer = 1
        Dim xPt As Double = 0
        Dim yPt As Double = 0
        Dim zPt As Double = 0
        Dim points As New List(Of Double())
        Do While Excel.Cells(i, 1).Value IsNot Nothing
            xPt = Excel.Cells(i, 1).value
            yPt = Excel.Cells(i, 2).value
            zPt = Excel.Cells(i, 3).value
            Dim p As Double() = {xPt, yPt, zPt}
            points.Add(p)
            i += 1
        Loop
        Return points
    End Function
End Module
