Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System


Partial Class SolidWorksMacro
    Public Sub main()
        Dim message As String
        Dim swVersion As Integer
        Dim swDoc As ModelDoc2
        Dim swTitle As String
        swDoc = swApp.ActiveDoc
        If swDoc Is Nothing Then Exit Sub
        swTitle = swDoc.GetTitle

        swVersion = swApp.DateCode

        'message = "Hello SOLIDWORKS " & swVersion.ToString _
        '  & vbCrLf & "Document: " & swTitle _
        '  & vbCrLf & "Would you like to close the active document?"

        message = String.Format(
            "Hello SOLIDWORKS {0}{1}Active:{2}{1}Would you like to close the active document?",
            swVersion.ToString, vbCrLf, swTitle)
        'Show the message
        Dim response As MsgBoxResult

        response = MsgBox(message, MsgBoxStyle.YesNo)
        If response = MsgBoxResult.Yes Then
            swApp.CloseDoc(swTitle)
        End If

    End Sub
    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks
End Class
