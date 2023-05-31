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
    Dim Part As PartDoc = Model
    'get the Extension interface from the model
    Dim Ext As ModelDocExtension = Model.Extension

    'get the first feature in the FeatureManager
    Dim feat As Feature = Part.FeatureByName("Extrude1")
    If Not feat Is Nothing Then
      'select it
      feat.Select2(False, -1)
      'get persistent ID
      Dim IDCount As Integer = Ext.GetPersistReferenceCount3(feat)
      Dim ID(IDCount) As Byte
      ID = Ext.GetPersistReference3(feat)
      Stop 'check selected feature and ID

      'get rid of the feature reference
      feat = Nothing
      'clear the selection
      Model.ClearSelection2(True)
      'select the feature by its ID
      Dim errors As Integer
      'get the feature by its persistent ID
      feat = Ext.GetObjectByPersistReference3(ID, errors)
      feat.Select2(False, -1)
      Stop 'check selected feature
    End If
  End Sub


  ''' <summary>
  ''' The SldWorks swApp variable is pre-assigned for you.
  ''' </summary>
  Public swApp As SldWorks


End Class
