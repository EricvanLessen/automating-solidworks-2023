Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System
Imports EPDM.Interop.epdm

Partial Class SolidWorksMacro
    Public Sub main()
        'Create a file vault interface and log in to a vault
        Dim vault As EdmVault5 = New EdmVault5
        Dim vaultName As String = "PDMVault"
        vault.LoginAuto(vaultName, 0)

        'Let the user select one or more files that are
        'in the file vault to which we are logged in  
        Dim PathList As EdmStrLst5
        PathList = vault.BrowseForFile(0, , , , , ,
    "Select a file to show its information")

        'Check if the user pressed Cancel
        If PathList Is Nothing Then
            'do nothing - user pressed cancel
        Else
            Dim message As String
            Dim pos As IEdmPos5
            'Display a message box with 
            'the paths of all selected files.        
            message = "You selected the following files:" + vbLf
            pos = PathList.GetHeadPosition
            While Not pos.IsNull
                Dim filePath As String
                filePath = PathList.GetNext(pos)

                'connect to the file object
                Dim eFile As IEdmFile5
                Dim parentFolder As IEdmFolder5
                eFile = vault.GetFileFromPath(filePath, parentFolder)
                message = filePath
                message = message & vbLf & "State: " & eFile.CurrentState.Name
                message = message & vbLf & "Is Checked Out: " _
          & eFile.IsLocked.ToString
                If eFile.IsLocked Then
                    message = message & vbLf & "Is Checked Out By: " _
          & eFile.LockedByUser.Name
                End If

                'get the file's EnumeratorVariable interface
                'for working with its card variables
                Dim eVar As IEdmEnumeratorVariable10
                eVar = eFile.GetEnumeratorVariable

                'get the description
                Dim varValue As String
                eVar.GetVar("Description", "@", varValue)
                message = message & vbLf & "Description: " & varValue

                'try to check out the file
                Try
                    eFile.LockFile(parentFolder.ID, 0)
                Catch ex As Exception
                    MsgBox(ex.Message & vbLf & eFile.Name)
                    Exit Sub
                End Try

                'reconnect to the file's EnumeratorVariable
                'after check out
                eVar = eFile.GetEnumeratorVariable

                'edit the file here
                Try
                    eVar.SetVar("Description", "@", "NEW DESCRIPTION")
                    'close the file and save (flush) any updates
                    eVar.CloseFile(True)
                Catch ex As Exception
                    MsgBox(ex.Message & vbLf & eFile.Name)
                    Exit Sub
                End Try

                'try to check in the file
                Try
                    eFile.UnlockFile(0, "API check in")
                Catch ex As Exception
                    MsgBox(ex.Message & vbLf & eFile.Name)
                    Exit Sub
                End Try


                'show information about the selected file
                MsgBox(message)
            End While

        End If


    End Sub
    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks
End Class
