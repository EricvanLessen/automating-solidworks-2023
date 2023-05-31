Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System


Partial Class SolidWorksMacro
    Public Sub main()
        'get the active document
        Dim swDoc As ModelDoc2 = swApp.ActiveDoc
        If swDoc.GetType <> swDocumentTypes_e.swDocPART Then
            MsgBox("This macro is for parts only.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        'get the part interface
        Dim swPart As PartDoc = swDoc
        'set a new path for the flat pattern
        Dim flatPatternPath As String
        Dim ext As String = ".DXF"
        'could also use .DWG if prefered
        flatPatternPath = IO.Path.ChangeExtension(swDoc.GetPathName, ext)
        'export the flat pattern with bend lines
        Dim bendSetting As Long
        bendSetting = 2 ^ 0 + 2 ^ 2
        'bendSetting = 2 ^ 0  'use this setting for no bend lines
        swPart.ExportToDWG2(flatPatternPath, swDoc.GetTitle,
                            swExportToDWG_e.swExportToDWG_ExportSheetMetal,
                            True, Nothing, False, False, bendSetting, Nothing)
    End Sub
    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks
End Class
