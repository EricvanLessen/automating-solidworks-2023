Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System
Imports EPDM.Interop.epdm

Partial Class SolidWorksMacro
    Public Sub main()
        Dim vault As New EdmVault5
        vault.LoginAuto("PDMVault", 0)

        Dim eFolder As IEdmFolder5
        eFolder = vault.BrowseForFolder(0, "Select the destination folder")
        Dim filePath As String
        Dim fileID As Integer
        Dim eFile As IEdmFile5
        Dim OpenDia As New Windows.Forms.OpenFileDialog
        Dim diaRes As Windows.Forms.DialogResult
        diaRes = OpenDia.ShowDialog
        If diaRes = Windows.Forms.DialogResult.OK Then
            filePath = OpenDia.FileName
            'try to add the file to the selected folder
            Try
                fileID = eFolder.AddFile(0, filePath)
                eFile = vault.GetObject(EdmObjectType.EdmObject_File, fileID)
            Catch ex As Exception
                'failed to add the file
                MsgBox(ex.Message)
                Exit Sub
            End Try

            'update the Description with the file name
            Try
                Dim eVar As IEdmEnumeratorVariable8
                eVar = eFile.GetEnumeratorVariable
                Dim newDescription As String
                newDescription = IO.Path.GetFileNameWithoutExtension(eFile.Name)
                eVar.SetVar("Description", "@", newDescription)
                eVar.CloseFile(True)
            Catch ex As Exception
                MsgBox(ex.Message & vbLf & eFile.Name)
                Exit Sub
            End Try

            'try to check in the file
            Try
                eFile.UnlockFile(0, "Automatic check in")
                MsgBox(eFile.Name & " added to the vault.")
            Catch ex As Exception
                MsgBox(ex.Message & vbLf & eFile.Name)
                Exit Sub
            End Try
        End If

    End Sub
    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks
End Class
