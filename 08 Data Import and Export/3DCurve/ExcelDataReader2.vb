Imports SolidWorks.Interop.swconst
Public Class ExcelDataReader2
    Dim Excel As Object
    Public Sub New()
        Try
            Excel = GetObject(, "Excel.Application")
        Catch ex As Exception
            MsgBox("Please open an Excel Workbook first.",
                   MsgBoxStyle.Critical)
        End Try
    End Sub

    Public Function getExcelPoints(units As swLengthUnit_e) As List(Of Point)
        Dim i As Integer = 1
        Dim points As New List(Of Point)
        Dim multiplier As Double = 1
        If units = swLengthUnit_e.swINCHES Then multiplier = inchToMeters(1)
        If units = swLengthUnit_e.swMM Then multiplier = mmToMeters(1)
        Do While Excel.Cells(i, 1).Value IsNot Nothing
            Dim p As New Point
            p.X = Excel.Cells(i, 1).value * multiplier
            p.Y = Excel.Cells(i, 2).value * multiplier
            p.Z = Excel.Cells(i, 3).value * multiplier
            points.Add(p)
            i += 1
        Loop
        Return points
    End Function
End Class
