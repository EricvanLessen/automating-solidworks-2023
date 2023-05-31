Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swdocumentmgr


Partial Class SolidWorksMacro

  Public Sub main()
    Dim MyDM As New MyDocMan

    'open the document for read/write
    Dim dmDoc As SwDMDocument
    Dim FilePath As String
    FilePath = MyDM.BrowseForDoc()
    dmDoc = MyDM.OpenDoc(FilePath, False)
    If dmDoc Is Nothing Then Exit Sub

    Dim tableText As String
    tableText = MyDM.GetBOMTableText(dmDoc)

    'close the document
    dmDoc.CloseDoc()

    'Stop 'look at the tabletext in the watch text visualizer
    Dim bomFile As String = IO.Path.ChangeExtension( _
      dmDoc.FullName, ".BOM.txt")
    My.Computer.FileSystem.WriteAllText(bomFile, tableText, True)

  End Sub

    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks


End Class
