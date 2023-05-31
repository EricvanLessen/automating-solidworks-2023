Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System


Partial Class SolidWorksMacro
    Public Sub main()
        Dim swDoc As ModelDoc2 = Nothing
        swDoc = CType(swApp.ActiveDoc, ModelDoc2)
        swDoc.InsertCurveFileBegin()

        Dim edr As New ExcelDataReader2
        Dim points As List(Of Point) = edr.getExcelPoints(swLengthUnit_e.swINCHES)
        For Each pt As Point In points
            swDoc.InsertCurveFilePoint(pt.X, pt.Y, pt.Z)
        Next
        swDoc.InsertCurveFileEnd()
        swDoc.ViewZoomtofit2()
    End Sub

    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks
End Class
