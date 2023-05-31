Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System


Partial Class SolidWorksMacro
  Public Sub main()
    Dim Part As PartDoc
    Dim MyFeature As Feature
    Dim featureDef As ExtrudeFeatureData2
    Dim retval As Boolean
    Dim message As String

    Part = swApp.ActiveDoc

    MyFeature = Part.FeatureByName("Cut-Extrude1")
    featureDef = MyFeature.GetDefinition

    'get some settings from the feature
    If featureDef.GetDraftWhileExtruding(True) = False Then
      message = "The selected feature has no draft." & vbCr
    End If

    Select Case featureDef.GetEndCondition(True)
      Case swEndConditions_e.swEndCondBlind
                message += "Blind"
            Case swEndConditions_e.swEndCondThroughAll
                message += "Through All"
            Case swEndConditions_e.swEndCondThroughAllBoth
                message += "Through All Both"
            Case swEndConditions_e.swEndCondUpToSurface
                message += "Up To Surface"
            Case swEndConditions_e.swEndCondMidPlane
                message += "Mid Plane"
            Case swEndConditions_e.swEndCondOffsetFromSurface
                message += "Offset From Surface"
            Case swEndConditions_e.swEndCondThroughNext
                message += "Up To Next"
            Case swEndConditions_e.swEndCondUpToBody
        message = message & "Up To Body"
    End Select

    MsgBox(message & " end condition.", MsgBoxStyle.Information)

    'rollback to edit the feature
    retval = featureDef.AccessSelections(Part, Nothing)

    'modify some feature values
    featureDef.SetEndCondition(True,
  swEndConditions_e.swEndCondThroughAll)
    featureDef.SetDraftWhileExtruding(True, True)
    featureDef.SetDraftAngle(True, 2 * Math.PI / 180)

    'complete the edit operation
    retval = MyFeature.ModifyDefinition(featureDef, Part, Nothing)

    'in case the modification failed
    If retval = False Then
      featureDef.ReleaseSelectionAccess()
    End If


  End Sub
  ''' <summary>
  ''' The SldWorks swApp variable is pre-assigned for you.
  ''' </summary>
  Public swApp As SldWorks
End Class
