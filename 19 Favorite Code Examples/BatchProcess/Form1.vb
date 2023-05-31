Imports System.Windows.Forms

Public Class Form1

Private Sub FromFolderButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FromFolderButton.Click
    Dim fb As New FolderBrowserDialog
    fb.Description = "Select the folder to process:"
    Dim res As DialogResult = fb.ShowDialog
    If res = Windows.Forms.DialogResult.OK Then
        FromFolderTextBox.Text = fb.SelectedPath
    End If
End Sub

Private Sub OutputFolderButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutputFolderButton.Click
    Dim fb As New FolderBrowserDialog
    fb.Description = "Select the output folder:"
    Dim res As DialogResult = fb.ShowDialog
    If res = Windows.Forms.DialogResult.OK Then
        OutputTextBox.Text = fb.SelectedPath
    End If
End Sub

Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OK_Button.Click
    Me.DialogResult = System.Windows.Forms.DialogResult.OK
    Me.Close()
End Sub

Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
    Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.Close()
End Sub

Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    'save settings
    My.Settings.Save()
End Sub
End Class