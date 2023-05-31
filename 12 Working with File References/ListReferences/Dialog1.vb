Imports System.Windows.Forms
Imports System.IO

Public Class Dialog1

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


  Private Sub FilesGridView_CellContentDoubleClick _
    (ByVal sender As Object, _
    ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
    Handles FilesGridView.CellContentDoubleClick
    Dim filename As String
    If FilesGridView.CurrentCell.ColumnIndex = 0 Then
      filename = FilesGridView.CurrentCell.Value
      ReferencesMod.ShowModel(filename)
    End If
  End Sub

  Private Sub SaveButton_Click(ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles SaveButton.Click

    Dim DefaultFilePath As String = FilesGridView.Rows(0).Cells(0).Value & ".REFS.txt"
    Dim saveDia As New SaveFileDialog
    saveDia.FileName = Path.GetFileName(DefaultFilePath)
    saveDia.InitialDirectory = Path.GetDirectoryName(DefaultFilePath)
    saveDia.Filter = "Text file (*.txt)|*.txt"
    If saveDia.ShowDialog = Windows.Forms.DialogResult.OK Then
      Try
        Dim output As String = ReferencesMod.FilesTableToString()
        My.Computer.FileSystem.WriteAllText(saveDia.FileName, output, False)
      Catch ex As Exception
        MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        Exit Sub
      End Try

      If MsgBox("Open file now?", MsgBoxStyle.YesNo _
        + MsgBoxStyle.Question) = MsgBoxResult.Yes Then
        Shell("notepad " & saveDia.FileName, AppWinStyle.NormalFocus)
      End If
    End If
  End Sub
End Class
