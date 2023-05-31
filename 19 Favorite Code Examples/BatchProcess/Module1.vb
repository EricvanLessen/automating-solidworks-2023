Imports System.IO
Imports System
Imports System.Windows.Forms
Imports SolidWorks.Interop.swconst
Imports SolidWorks.Interop.sldworks

Module Module1
    Dim LogFile As StreamWriter
Sub Process(ByVal FromFolder As String, _
ByVal ToFolder As String)

    'get the folders as DirectoryInfo interfaces
    Dim FromDir As New DirectoryInfo(FromFolder)
    Dim ToDir As New DirectoryInfo(ToFolder)

    'if the processing directory doesn't exist, 
    'tell the user and exit
    If Not FromDir.Exists Then
        MsgBox(FromDir.FullName & " does not exist.", _
          MsgBoxStyle.Exclamation)
        Exit Sub
    End If

    'if the output folder doesn't exist, create it
    If Not ToDir.Exists Then
        ToDir.Create()
    End If

    'initialize the log file
    LogFile = New StreamWriter(ToDir.FullName _
      & "\Process.log")

    'save as another file format
    'change the extension value to whatever you want to export
    Dim Extension As String = ".PDF"
    Dim FileTypeToProcess As Long = swDocumentTypes_e.swDocDRAWING
    SaveAs(FromDir, ToDir, Extension, FileTypeToProcess)

    ''save as DXF
    'Extension = ".DXF"
    'FileTypeToProcess = swDocumentTypes_e.swDocDRAWING
    'SaveAs(FromDir, ToDir, Extension, FileTypeToProcess)

    ''save parts as IGS
    'Extension = ".IGS"
    'FileTypeToProcess = swDocumentTypes_e.swDocPART
    'SaveAs(FromDir, ToDir, Extension, FileTypeToProcess)

    'add other processing as needed

    'finish up by closing out the log file
    LogFile.Flush()
    LogFile.Close()
End Sub

Sub SaveAs(ByVal dinf As DirectoryInfo, _
ByVal outDir As DirectoryInfo, _
ByVal Ext As String, ByVal TypeToProcess As Long)
    Dim longErrors As Long
    Dim longWarnings As Long

    Dim SWXWasRunning As Boolean

    'get the file extension based on file type
    Dim files() As FileInfo
    Dim filter As String = ""
    Select Case TypeToProcess
        Case swDocumentTypes_e.swDocASSEMBLY
            filter = ".SLDASM"
        Case swDocumentTypes_e.swDocPART
            filter = ".SLDPRT"
        Case swDocumentTypes_e.swDocDRAWING
            filter = ".SLDDRW"
    End Select

    'get all files in the directory
    'that match the filter
    files = dinf.GetFiles(filter)
    For Each f As FileInfo In files

        Dim swApp As SldWorks
        Dim Part As ModelDoc2

        'if solidworks is running, use the active session
        'if not, start a new hidden session
        Try
            swApp = GetObject(, "sldworks.application")
            SWXWasRunning = True
        Catch ex As Exception
            swApp = CreateObject("sldworks.application")
            SWXWasRunning = False
        End Try

        Try
            Part = swApp.OpenDoc6(f.FullName, TypeToProcess, _
              swOpenDocOptions_e.swOpenDocOptions_Silent _
              + swOpenDocOptions_e.swOpenDocOptions_ReadOnly, _
              "", longErrors, longWarnings)
            Dim newFileName As String
            newFileName = Path.ChangeExtension(f.FullName, Ext)
            Part.SaveAs2(newFileName, _
              swSaveAsVersion_e.swSaveAsCurrentVersion, _
              True, True)
            swApp.CloseDoc(Part.GetTitle)
            Part = Nothing
            If Not SWXWasRunning Then
                'exit SolidWorks if it was 
                'not previously running
                swApp.ExitApp()
                swApp = Nothing
            End If
        Catch ex As Exception
            WriteLogLine(Date.Now & vbTab & "ERROR" _
              & vbTab & f.Name & vbTab & ex.Message)
        End Try
    Next
End Sub

Sub WriteLogLine(ByVal LogString As String)
    LogFile.WriteLine(Now() & vbTab & LogString)
End Sub
End Module
