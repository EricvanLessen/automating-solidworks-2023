Imports System.Windows.Forms
Imports System.IO

Public Class Dialog1

  Private Sub OK_Button_Click(ByVal sender As System.Object, _
  ByVal e As System.EventArgs) Handles OK_Button.Click
    'process the data from the form with solidworks
    AddComponents.AddCompAndMate(FilePathTextBox.Text)

    Me.DialogResult = System.Windows.Forms.DialogResult.OK
    Me.Close()
  End Sub

  Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
    Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.Close()
  End Sub

  Private Sub BrowseButton_Click(ByVal sender As System.Object, _
  ByVal e As System.EventArgs) Handles BrowseButton.Click
    Dim OpenDia As New OpenFileDialog
    OpenDia.Filter = "SOLIDWORKS Part (*.sldprt)|*.sldprt"
    Dim diaRes As DialogResult = OpenDia.ShowDialog
    If diaRes = Windows.Forms.DialogResult.OK Then
      FilePathTextBox.Text = OpenDia.FileName
    End If
  End Sub

  Private Sub Dialog1_FormClosing(ByVal sender As Object, _
  ByVal e As System.Windows.Forms.FormClosingEventArgs) _
  Handles Me.FormClosing
    My.Settings.Save()
  End Sub

End Class
