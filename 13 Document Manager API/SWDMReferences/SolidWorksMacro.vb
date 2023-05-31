Imports SolidWorks.Interop.sldworks

Partial Class SolidWorksMacro

    Public Sub main()
      Dim MyDM As New MyDocMan

      'Browse for a file
      Dim FilePath As String
      FilePath = MyDM.BrowseForDoc()

      MyDM.GetRefs(FilePath, True)

    End Sub

    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks


End Class
