Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System


Partial Class SolidWorksMacro
  Public Sub main()

    Dim swDoc As ModelDoc2 = Nothing
        swDoc = swApp.ActiveDoc

        AddDatum(swDoc)
        AddFileNameNote(swDoc)
        InsertGeneralTable(swDoc)

        Dim BOM As BomTableAnnotation
    BOM = InsertBOMTable(swDoc)
    ReadBOMTable(BOM)

    Stop


  End Sub
  ''' <summary>
  ''' The SldWorks swApp variable is pre-assigned for you.
  ''' </summary>
  Public swApp As SldWorks
End Class
