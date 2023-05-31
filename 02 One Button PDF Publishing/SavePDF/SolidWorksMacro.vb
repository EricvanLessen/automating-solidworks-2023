Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System


Partial Class SolidWorksMacro
    Public Sub main()

        Dim swDoc As ModelDoc2 = Nothing
        Dim swPart As PartDoc = Nothing
        Dim swDrawing As DrawingDoc = Nothing
        Dim swAssembly As AssemblyDoc = Nothing
        Dim boolstatus As Boolean = False
        Dim longstatus As Integer = 0
        Dim longwarnings As Integer = 0
        swDoc = CType(swApp.ActiveDoc, ModelDoc2)
        swDoc.ClearSelection2(True)
        '
        'Save As
        longstatus = swDoc.SaveAs3("C:\Sample Files\Drawing1.pdf", 0, 2)


    End Sub
    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks
End Class
