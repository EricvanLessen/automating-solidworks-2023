Imports System.IO
Imports System.Windows.Forms

Public Class PropsDialog

  Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyButton.Click
    'set the property value
    PropVal(PropsListBox.SelectedIndex) = ValueTextBox.Text
    SetAllProps()

  End Sub

  Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
  Handles Cancel_Button.Click
    Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.Close()
  End Sub

  Private Sub PropsListBox_SelectedIndexChanged(
  sender As Object, e As EventArgs) _
   Handles PropsListBox.SelectedIndexChanged
    ValueTextBox.Text = PropVal(PropsListBox.SelectedIndex)
  End Sub

  Private Sub AddButton_Click(sender As Object,
  e As EventArgs) Handles AddButton.Click
    'get the property to add
    Dim NewName As String
    Dim NewVal As String
    NewName = InputBox("Enter new property name:")
    NewVal = InputBox("Enter property value for " & NewName & ":")

    'add it to the file and update the array
    AddProperty(NewName, NewVal)
    RefreshForm()
  End Sub

  Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
    'get the list's selected item to know what to delete
    Dim PropToDelete As String
    PropToDelete = PropsListBox.SelectedItem

    'verify deletion from the user
    Dim answer As MsgBoxResult = MsgBox("Delete " & PropToDelete & "?",
    MsgBoxStyle.YesNo + MsgBoxStyle.Question)
    If answer = MsgBoxResult.Yes Then
      'Delete the property from the file
      DeleteProperty(PropToDelete)
    End If
    RefreshForm()
  End Sub

  Sub RefreshForm()
    'reset list of properties displayed to the user
    PropsListBox.Items.Clear()
    PropsListBox.Items.AddRange(PropName)
    PropsListBox.SelectedIndex = PropName.Length - 1
    Me.Refresh()
  End Sub

  Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
    Dim FileName As String = "savedprops.txt"
    Try
      Dim sr As New StreamWriter(FileName)
      For i As Integer = 0 To PropName.Length - 1
        sr.WriteLine(PropName(i))
        sr.WriteLine(PropVal(i))
      Next
      sr.Close()
    Catch ex As Exception
      MsgBox(ex.Message, MsgBoxStyle.Exclamation)
      Exit Sub
    End Try

  End Sub

  Private Sub LoadButton_Click(sender As Object, e As EventArgs) Handles LoadButton.Click
    Dim FileName As String = "savedprops.txt"
    Dim sw As New StreamReader(FileName)
    Dim i As Integer = 0
    Do While Not sw.EndOfStream
      ReDim Preserve PropName(i)
      ReDim Preserve PropVal(i)
      PropName(i) = sw.ReadLine
      PropVal(i) = sw.ReadLine
      i = i + 1
    Loop

    'write the properties to the part
    SetAllProps()
    RefreshForm()

  End Sub
End Class
