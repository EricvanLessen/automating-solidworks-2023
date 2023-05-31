Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System


Partial Class SolidWorksMacro
    Public Sub main()
    Dim swDoc As ModelDoc2
    Dim MyFeature As Feature
    Dim retval As Boolean

    swDoc = swApp.ActiveDoc

    'get the first feature
    MyFeature = swDoc.FirstFeature

    'loop through remaining features
    Dim radius As String
    Dim featureDef As Object
    radius = InputBox("Suppress all fillets < or = (in meters)")
    If Not IsNumeric(radius) Then
      MsgBox("Please enter only numeric values.", MsgBoxStyle.Exclamation)
      Exit Sub
    End If

    Do While MyFeature IsNot Nothing
      If MyFeature.GetTypeName2 = "Fillet" Then
        'MsgBox("Found: " & MyFeature.Name)
        featureDef = MyFeature.GetDefinition
        If featureDef.DefaultRadius <= CDbl(radius) Then
          MyFeature.Select2(False, 0)
          swDoc.EditSuppress2()
        End If

      End If

      MyFeature = MyFeature.GetNextFeature
    Loop


  End Sub
    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks
End Class
