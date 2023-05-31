Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System
Imports System.Windows.Forms

Partial Class SolidWorksMacro

    Public Sub main()
        Dim MyForm As New Form1
        Dim DiaRes As DialogResult = MyForm.ShowDialog
        If DiaRes = DialogResult.OK Then
            'process the folders
            Module1.Process(MyForm.FromFolderTextBox.Text, _
              MyForm.OutputTextBox.Text)
        End If
    End Sub

    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks


End Class
