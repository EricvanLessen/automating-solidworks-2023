Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Data

Module ReferencesMod
  Public m_swApp As SldWorks
  Dim filesTable As New DataTable
  Dim filePathColumnName As String = "File path"
  Dim dirtyFlagColumnName As String = "Dirty flag"

  Sub ShowModel(ByVal filename As String)
    'activate the currently selected item in the list
    Dim swDoc As ModelDoc2
    swDoc = m_swApp.ActivateDoc3(filename, True, 0, Nothing)
  End Sub

  Function ReadRefs() As DataTable
    Dim fileRefs() As String
    Dim PathName As String
    Dim UpdateStamp As Long
    Dim swDoc As ModelDoc2
    Dim RowData(1) As String
    Dim i As Integer

    swDoc = m_swApp.ActiveDoc
    PathName = swDoc.GetPathName
    UpdateStamp = swDoc.GetUpdateStamp

    'get an array of file references from the active document
    fileRefs = swDoc.Extension.GetDependencies(True, True,
       False, False, False)

    'set up the table columns    
    filesTable.Columns.Add(filePathColumnName)
    filesTable.Columns.Add(dirtyFlagColumnName)

    'add the top file name and UpdateStamp
    RowData(0) = PathName
    RowData(1) = UpdateStamp.ToString
    filesTable.Rows.Add(RowData)

    For i = 1 To UBound(fileRefs) Step 2
      RowData(0) = fileRefs(i)
      'activate the document to get its modeldoc  
      swDoc = m_swApp.GetOpenDocumentByName(fileRefs(i))
      If swDoc Is Nothing Then
        RowData(1) = "Not loaded"
      Else
        UpdateStamp = swDoc.GetUpdateStamp
        RowData(1) = UpdateStamp.ToString
      End If

      filesTable.Rows.Add(RowData)
    Next

    Return filesTable

  End Function


  Function FilesTableToString() As String
    Dim output As String = ""
    For Each row As DataRow In filesTable.Rows
            output += row.Item(filePathColumnName) & vbCrLf

        Next
    Return output
  End Function

End Module
