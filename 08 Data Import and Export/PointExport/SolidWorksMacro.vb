Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System


Partial Class SolidWorksMacro
    Public Sub main()

        'assume the desired sketch is currently in edit mode
        Dim model As ModelDoc2 = swApp.ActiveDoc
        Dim skMgr As SketchManager = model.SketchManager
        Dim ptSketch As Sketch = skMgr.ActiveSketch
        If ptSketch Is Nothing Then
            MsgBox("Please edit the desired sketch before running.",
        MsgBoxStyle.Exclamation)
            Exit Sub
        Else
            'initialize a string for the output file
            Dim outputString As String
            'it will be tab-delimited
            Dim headerRow As String = "X" & vbTab & "Y" & vbTab & "Z"
            outputString = headerRow

            'gather all sketch points
            Dim points As Object
            points = ptSketch.GetSketchPoints2
            For Each skPoint As SketchPoint In points
                Dim x As Double = skPoint.X
                Dim y As Double = skPoint.Y
                Dim z As Double = skPoint.Z

                'add the text to the output string
                outputString += vbCrLf & x.ToString & vbTab _
          & y.ToString & vbTab & z.ToString
            Next

            'save the resulting data to a file
            Dim filePath As String =
        IO.Path.ChangeExtension(model.GetPathName, ".txt")
            My.Computer.FileSystem.WriteAllText(filePath, outputString, False)
            'open the text file
            Process.Start(filePath)
        End If

    End Sub
    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks
End Class
