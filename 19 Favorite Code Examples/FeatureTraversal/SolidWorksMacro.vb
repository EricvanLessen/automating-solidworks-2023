Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System

Partial Class SolidWorksMacro

  Public Sub main()
    Dim Model As ModelDoc2 = swApp.ActiveDoc
    'make sure the active document is a part
    If Model Is Nothing Then
      MsgBox("Please open a part first.", MsgBoxStyle.Exclamation)
      Exit Sub
    ElseIf Model.GetType <> swDocumentTypes_e.swDocPART Then
      MsgBox("For parts only.", MsgBoxStyle.Exclamation)
      Exit Sub
    End If
    'get the PartDoc interface from the model
    Dim Part As PartDoc = Model

    'get the first feature in the FeatureManager
    Dim feat As Feature = Part.FirstFeature()
    Dim featureName As String
    Dim featureTypeName As String
    Dim subFeat As Feature = Nothing
    Dim subFeatureName As String
    Dim subFeatureTypeName As String
    Dim message As String

    ' While we have a valid feature
    While Not feat Is Nothing
      ' Get the name of the feature
      featureName = feat.Name
      featureTypeName = feat.GetTypeName2
      message = "Feature: " & featureName & vbCrLf & _
      "FeatureType: " & featureTypeName & vbCrLf & _
      " SubFeatures:"

      'get the feature's sub features
      subFeat = feat.GetFirstSubFeature

      ' While we have a valid sub-feature
      While Not subFeat Is Nothing

        ' Get the name of the sub-feature
        ' and its type name
        subFeatureName = subFeat.Name
        subFeatureTypeName = subFeat.GetTypeName2
                message += vbCrLf & " " &
        subFeatureName & vbCrLf & " " &
        "Type: " & subFeatureTypeName

                subFeat = subFeat.GetNextSubFeature
        ' Continue until the last sub-feature is done
      End While

      ' Display the sub-features in the 
      System.Diagnostics.Debug.Print(message)
      ' Get the next feature
      feat = feat.GetNextFeature()
      ' Continue until the last feature is done
    End While

  End Sub

  ''' <summary>
  ''' The SldWorks swApp variable is pre-assigned for you.
  ''' </summary>
  Public swApp As SldWorks


End Class
