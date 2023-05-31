Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System
Imports EPDM.Interop.epdm

Partial Class SolidWorksMacro
    Public Sub main()
        'Create a file vault interface and log in to a vault
        Dim vault As IEdmVault5 = New EdmVault5
        vault.LoginAuto("PDMVault", 0)
        'Dim username As String = "Admin"
        'Dim password As String = "password"
        'Dim vaultname As String = "VaultName"
        'vault.Login(username, password, vaultname)

        'Get the vault's root folder interface 
        Dim message As String
        Dim file As IEdmFile5
        Dim folder As IEdmFolder5
        folder = vault.RootFolder

        'Get position of first file in the root folder 
        Dim pos As IEdmPos5
        pos = folder.GetFirstFilePosition
        If pos.IsNull Then
            message = "The root folder of your vault does not contain any files."
        Else
            message = "The root folder of your vault contains these files:" + vbLf
        End If

        'For all files in the root folder, append the name to the message
        While Not pos.IsNull
            file = folder.GetNextFile(pos)
            message = message + file.Name + vbLf
        End While

        'Show the names of all files in the root folder 
        MsgBox(message)

        'Let the user select one or more files that are
        'in the file vault to which we are logged in.
        Dim PathList As EdmStrLst5
        PathList = vault.BrowseForFile(0, , , , , , "Select a file to show its information")
        'Check if the user pressed Cancel
        If PathList Is Nothing Then
            'do nothing - user pressed cancel
        Else
            'Display a message box with 
            'the paths of all selected files.        
            message = "You selected the following files:" + vbLf
            pos = PathList.GetHeadPosition
            While Not pos.IsNull
                Dim filePath As String
                filePath = PathList.GetNext(pos)

                'connect to the file object
                Dim eFile As IEdmFile5
                eFile = vault.GetFileFromPath(filePath)
                message = filePath
                message = message & vbLf & "State: " & eFile.CurrentState.Name
                message = message & vbLf & "Is Checked Out: " _
            & eFile.IsLocked.ToString
                If eFile.IsLocked Then
                    message = message & vbLf & "Is Checked Out By: " _
                & eFile.LockedByUser.Name
                End If
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
