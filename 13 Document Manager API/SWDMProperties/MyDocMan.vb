Imports SolidWorks.Interop.swdocumentmgr
Imports System.Diagnostics

Public Class MyDocMan
  Dim swDMApp As SwDMApplication4

  Public ReadOnly Property DocManApp() As SwDMApplication4
    Get
      If swDMApp Is Nothing Then
        Dim swDMClassFact As SwDMClassFactory
        swDMClassFact =
          CreateObject("SwDocumentMgr.SwDMClassFactory")
        swDMApp = swDMClassFact.GetApplication("ENTER YOUR DOCUMENT MANAGER KEY HERE")
      End If
      Return swDMApp
    End Get
  End Property

  Friend Function BrowseForDoc(Optional ByVal FileName As String = "") As String
    'open the document for read/write
    Dim FilePath As String = ""
    Dim ofd As New Windows.Forms.OpenFileDialog
    ofd.Filter = "SOLIDWORKS Files (*.sldprt, " _
    & "*.sldasm, *.slddrw)" _
    & "|*.sldprt;*.sldasm;*.slddrw|Parts " _
    & "(*.sldprt)|*.sldprt" _
    & "|Assemblies (*.sldasm)|*.sldasm|Drawings " _
    & "(*.slddrw)" _
    & "|*.slddrw|All files (*.*)|*.*"
    ofd.FileName = FileName
    If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
      FilePath = ofd.FileName
    End If
    If FilePath = "" Then Return Nothing

    Return FilePath
  End Function

  Friend Function OpenDoc(FilePath As String,
   isReadOnly As Boolean) As SwDMDocument
    Dim ext As String = System.IO.Path.GetExtension(FilePath).ToUpper
    Dim docType As Integer
    Select Case ext
      Case "SLDPRT"
        docType = SwDmDocumentType.swDmDocumentPart
      Case "SLDASM"
        docType = SwDmDocumentType.swDmDocumentAssembly
      Case "SLDDRW"
        docType = SwDmDocumentType.swDmDocumentDrawing
      Case Else
        docType = SwDmDocumentType.swDmDocumentUnknown
    End Select

    Dim openerrors As Integer
    Dim dmDoc As SwDMDocument
    dmDoc = DocManApp.GetDocument(FilePath,
      docType, isReadOnly, openerrors)

    'error handling
    If dmDoc Is Nothing Then
      Dim errMessage As String = "Error accessing " & FilePath
      Select Case openerrors
        Case SwDmDocumentOpenError.swDmDocumentOpenErrorNonSW
                    errMessage += vbCrLf & "Non SOLIDWORKS file."
                Case SwDmDocumentOpenError.swDmDocumentOpenErrorNoLicense
                    errMessage += vbCrLf & "Invalid Document " _
            & "Manager license."
                Case SwDmDocumentOpenError.swDmDocumentOpenErrorFutureVersion
                    errMessage += vbCrLf & "Unsupported file version."
                Case SwDmDocumentOpenError.swDmDocumentOpenErrorFileNotFound
                    errMessage += vbCrLf & "File not found."
                Case SwDmDocumentOpenError.swDmDocumentOpenErrorFail
                    errMessage += vbCrLf & "Failed to access the document."
                Case SwDmDocumentOpenError.swDmDocumentOpenErrorFileReadOnly
                    errMessage += vbCrLf & "File is Read Only or is " _
            & "open in another application."
            End Select
      MsgBox(errMessage, MsgBoxStyle.Exclamation)
      Return Nothing
    Else
      Return dmDoc
    End If
  End Function

  Friend Function ReadAllProps(ByVal dmDoc As SwDMDocument) As String
    'read all file custom properties
    Dim output As String
    output = dmDoc.FullName & " Properties:"
    Dim propNames As Object = dmDoc.GetCustomPropertyNames()
    If Not propNames Is Nothing Then
      For Each propName As String In propNames
        Dim propVal As String
        propVal = dmDoc.GetCustomProperty(propName, Nothing)
        output = output & vbCrLf & propName & vbTab & propVal
      Next
    End If
    Return output
  End Function

  Friend Function ReadConfigProps(ByVal dmDoc As SwDMDocument) As String
    'read all configurations
    Dim configMan As SwDMConfigurationMgr
    configMan = dmDoc.ConfigurationManager()
    Dim configNames As Object
    Try
      configNames = configMan.GetConfigurationNames
      Dim output As String = "Configurations:"
      For Each config As String In configNames
        output = output & vbCrLf & "Configuration: " & config
        'Get configuration specific properties
        Dim dmConfig As SwDMConfiguration
        dmConfig = configMan.GetConfigurationByName(config)
        Dim propNames As Object = dmConfig.GetCustomPropertyNames()
        If Not propNames Is Nothing Then
          For Each propName As String In propNames
            Dim propVal As String
            propVal = dmConfig.GetCustomProperty(propName, Nothing)
            output = output & vbCrLf & propName & vbTab & propVal
          Next
        End If

      Next
      Return output
    Catch ex As Exception
      'ignore the error for drawings
      Return ""
    End Try

  End Function

  Friend Sub WriteProp(ByVal dmDoc As SwDMDocument,
  ByVal pName As String, ByVal pVal As String)
    'write a property
    Dim addRes As Boolean
    addRes = dmDoc.AddCustomProperty(pName,
      SwDmCustomInfoType.swDmCustomInfoText, pVal)
    If Not addRes Then
      dmDoc.SetCustomProperty(pName, pVal)
    End If
  End Sub

  Friend Function GetBOMTableText(dmDoc As SwDMDocument10) As String
    Dim tableText As String
    Dim tableNames As Object
    tableNames = dmDoc.GetTableNames(SwDmTableType.swDmTableTypeBOM)
    For Each table As String In tableNames
      Dim dmTable As SwDMTable5
      dmTable = dmDoc.GetTable(table)
      Debug.Print(dmTable.Name)
      Dim rows As Integer
      Dim columns As Integer
      Dim ttext As Object
      ttext = dmTable.GetTableCellText(Nothing, rows, columns)

      tableText = "Table: " & dmTable.Name & vbCrLf
      'format the cell text array into a tab 
      'delimited text file
      Dim cells As Integer = 0
      For i As Integer = 1 To rows
        Dim rowText As String = ""
        For j As Integer = 1 To columns
          rowText = rowText & ttext(cells) & vbTab
          cells += 1
        Next
        tableText = tableText & rowText & vbCrLf
      Next
    Next
    Return tableText
  End Function














  Friend Function GetBOMTableTextFULL(dmDoc As SwDMDocument10) As String
    Dim tableText As String
    Dim tableNames As Object
    tableNames = dmDoc.GetTableNames(SwDmTableType.swDmTableTypeBOM)
    For Each table As String In tableNames
      Dim dmTable As SwDMTable5
      dmTable = dmDoc.GetTable(table)
      Debug.Print(dmTable.Name)
      Dim rows As Integer
      Dim columns As Integer
      Dim ttext As Object
      ttext = dmTable.GetTableCellText(Nothing, rows, columns)
      tableText = "Table: " & dmTable.Name & vbCrLf
      'format the cell text array into a tab 
      'delimited text file
      Dim cells As Integer = 0
      For i As Integer = 1 To rows
        Dim rowText As String = ""
        For j As Integer = 1 To columns
          rowText = rowText & ttext(cells) & vbTab
          cells += 1
        Next
        tableText = tableText & rowText & vbCrLf
      Next
    Next
    Return tableText
  End Function

  'Recursive routine to report all missing file references
  Public Sub GetRefs(FilePath As String,
Optional replace As Boolean = False)
    Debug.Print("=== " & FilePath & " REFERENCES ===")
    'get references
    Dim searchOpt As SwDMSearchOption
    Dim brokenRefs As Object = Nothing
    Dim isVirtual As Object = Nothing
    Dim timeStamp As Object = Nothing
    Dim refs As Object
    searchOpt = DocManApp.GetSearchOptionObject()

    searchOpt.SearchFilters =
    SwDmSearchFilters.SwDmSearchExternalReference

    Dim dmDoc As SwDMDocument21
    dmDoc = OpenDoc(FilePath, False)
    If dmDoc Is Nothing Then Exit Sub

    refs = dmDoc.GetAllExternalReferences5(
    searchOpt, brokenRefs, isVirtual, timeStamp, Nothing)

    Dim ref As String
    For i As Integer = 0 To refs.Length - 1
      ref = refs(i)
      If Not isVirtual(i) Then
        If Not IO.File.Exists(ref) Then
          Debug.Print("MISSING: " & ref)
          If replace Then
            Dim msgRes As MsgBoxResult
            msgRes = MsgBox("Missing: " & vbCrLf & ref & vbCrLf _
            & "Would you like to replace it?",
            MsgBoxStyle.YesNo + MsgBoxStyle.Question)
            If msgRes = MsgBoxResult.Yes Then
              Dim newFilePath As String
              newFilePath = BrowseForDoc(IO.Path.GetFileName(ref))
              If newFilePath <> "" Then
                dmDoc.ReplaceReference(ref, newFilePath)
              End If
            End If
          End If

        Else
          Debug.Print(ref)
          If IO.Path.GetExtension(ref).ToUpper = ".SLDASM" Then
            'recursively search the assembly 
            'for its references
            GetRefs(ref, replace)
          End If
        End If
      End If
    Next
    'save if replacing references
    If replace Then dmDoc.Save()
    dmDoc.CloseDoc()
  End Sub


End Class

