Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Windows.Forms

Module AddComponentsMod

  Public m_swApp As SldWorks

  Public Sub AddCompAndMate(ByVal PartPath As String)
    Dim Model As ModelDoc2 = m_swApp.ActiveDoc
    Dim MyView As ModelView = Model.ActiveView
    Dim errors As Long
    If Model.GetType <> swDocumentTypes_e.swDocASSEMBLY Then
      MsgBox("For use with assemblies only.", _
        MsgBoxStyle.Exclamation)
      Exit Sub
    End If
    Dim MyAssy As AssemblyDoc = Model
    'continue adding processing code here
    'get the current selection
    'user should have selected the edge of a hole
    'or an entire face for parts to be inserted on
    Dim Edges As New Collection
    Dim selMgr As SelectionMgr = Model.SelectionManager
    Dim SelectedObject As Object = Nothing
    Dim SelCount As Integer = selMgr.GetSelectedObjectCount2(-1)
    If SelCount > 0 Then
      'make sure they have selected a face
      SelectedObject = selMgr.GetSelectedObject6(1, -1)
      Dim SelType As Integer = selMgr.GetSelectedObjectType3(1, -1)
      If SelType = swSelectType_e.swSelFACES And SelCount = 1 Then
        'get all circular edges on the face
        Edges = GetAllFaceCircularEdges(SelectedObject)

      ElseIf SelType = swSelectType_e.swSelEDGES Then
        'get all circular edges selected
        Edges = GetSelectedCircularEdges(selMgr)

      End If
    Else
      MsgBox("Please select a face or hole edges.", MsgBoxStyle.Exclamation)
      Exit Sub
    End If

    'open the part now
    Dim Part As PartDoc = OpenPartInvisible(PartPath)
    If Part Is Nothing Or Edges Is Nothing Then
      CleanUp(MyView, Model)
      Exit Sub
    End If


    'get the component that was selected
    Dim SelectedComp As Component2 = Nothing
    SelectedComp = selMgr.GetSelectedObjectsComponent2(1)

    'turn off assembly graphics update
    'add it to each circular edge
    MyView.EnableGraphicsUpdate = False
    For Each CircEdge As Edge In Edges
      'get the center of the circular edge
      Dim Center() As Double
      Center = GetCircleCenter(CircEdge.GetCurve, SelectedComp)
      'insert the part at the circular edge's center
      Dim MyComp As Component2 = MyAssy.AddComponent4(PartPath, _
        "", Center(0), Center(1), Center(2))

      'get the two faces from the edge
      'set the first face to the cylinder
      'the second to the flat face
      Dim MyFaces() As Face2 = GetEdgeFaces(CircEdge)
      'Add Mates
      'add a coincident mate bewteen the flat face
      'and the Front Plane of the added component
      Dim MyPlane As Feature = MyComp.FeatureByName("Front Plane")
      Dim MyMate As Mate2
      If Not MyPlane Is Nothing Then
        MyPlane.Select2(False, -1)
        MyFaces(1).Select(True)

        MyMate = MyAssy.AddMate5 _
          (swMateType_e.swMateCOINCIDENT, _
          swMateAlign_e.swMateAlignALIGNED, False, _
          0, 0, 0, 0, 0, 0, 0, 0, False, False, 0, errors)
      End If

      'mate the cylinder concentric to "Axis1"
      Dim MyAxis As Feature = MyComp.FeatureByName("Axis1")
      If Not MyAxis Is Nothing Then
        MyAxis.Select2(False, -1)
        'select the cylindrical face
        MyFaces(0).Select(True)

        MyMate = MyAssy.AddMate5(swMateType_e.swMateCONCENTRIC, _
          swMateAlign_e.swMateAlignCLOSEST, False, _
          0, 0, 0, 0, 1, 0, 0, 0, False, False, 0, errors)
      End If
    Next

    CleanUp(MyView, Model)
  End Sub

  Private Sub CleanUp(ByVal MyView As ModelView, ByVal Model As ModelDoc2)
    'turn part visibility back on
    m_swApp.DocumentVisible(True, swDocumentTypes_e.swDocPART)
    'turn graphics updating back on
    MyView.EnableGraphicsUpdate = True
    Model.ClearSelection2(True)
  End Sub

  Private Function OpenPartInvisible(ByVal PartPath As String) As PartDoc
    Dim errors As Integer
    Dim warnings As Integer
    'open the file invisibly
    m_swApp.DocumentVisible(False, swDocumentTypes_e.swDocPART)
    Dim Part As PartDoc = m_swApp.OpenDoc6 _
      (PartPath, swDocumentTypes_e.swDocPART, _
      swOpenDocOptions_e.swOpenDocOptions_Silent, "", errors, warnings)
    If Part Is Nothing Then
      MsgBox("Unable to open " & PartPath, MsgBoxStyle.Exclamation)
      Return Nothing
    End If
    Return Part
  End Function

  Private Function GetSelectedCircularEdges(ByVal selMgr As SelectionMgr) As Collection
    Dim SelectedObject As Object
    Dim Edges As New Collection
    For i As Integer = 1 To selMgr.GetSelectedObjectCount2(-1)
      SelectedObject = selMgr.GetSelectedObject6(i, -1)
      Dim SelType As Integer = selMgr.GetSelectedObjectType3(i, -1)
      'make sure each selection is an edge
      If SelType = swSelectType_e.swSelEDGES Then
        Dim MyEdge As Edge = SelectedObject
        If IsFullCircle(MyEdge) Then
          Edges.Add(MyEdge)
        End If
      End If
    Next
    Return Edges
  End Function

  Private Function GetAllFaceCircularEdges(ByVal SelectedFace As Face2) As Collection
    Dim Edges As New Collection
    Dim FaceEdges() As Object = SelectedFace.GetEdges
    For Each MyEdge As Edge In FaceEdges
      If IsFullCircle(MyEdge) Then
        Edges.Add(MyEdge)
      End If
    Next
    Return Edges
  End Function

  Private Function IsFullCircle(ByVal EdgeToCheck As Edge) As Boolean
    Dim MyCurve As Curve = EdgeToCheck.GetCurve
    If MyCurve.IsCircle Then
      'you have a circular edge
      'is it a complete circle?
      If EdgeToCheck.GetStartVertex() Is Nothing Then
        'full circle
        IsFullCircle = True
        Exit Function
      End If
    End If
    IsFullCircle = False
  End Function

  'function to get and return the two adjacent faces of the edge
  Private Function GetEdgeFaces(ByVal MyEdge As Edge) As Face2()
    Dim tmpFaces(1) As Face2
    Dim tmpFace0 As Face2 = MyEdge.GetTwoAdjacentFaces2(0)
    Dim tmpSurf0 As Surface = tmpFace0.GetSurface
    'check if the surface is a cylinder
    If tmpSurf0.IsCylinder Then
      tmpFaces(0) = MyEdge.GetTwoAdjacentFaces2(0)
      tmpFaces(1) = MyEdge.GetTwoAdjacentFaces2(1)
    Else
      tmpFaces(0) = MyEdge.GetTwoAdjacentFaces2(1)
      tmpFaces(1) = MyEdge.GetTwoAdjacentFaces2(0)
    End If
    'the zero element should be a cylinder
    GetEdgeFaces = tmpFaces
  End Function

  'return an array of doubles for the x, y, z circle center
  'relative to the assembly
  Private Function GetCircleCenter(ByVal MyCurve As Curve, _
  ByVal Comp As Component2) As Double()
    Dim MyCenter(2) As Double
    Dim returnValues As Object = MyCurve.CircleParams
    MyCenter(0) = returnValues(0)
    MyCenter(1) = returnValues(1)
    MyCenter(2) = returnValues(2)

    Dim MathUtil As MathUtility = m_swApp.GetMathUtility
    Dim mPoint As MathPoint = Nothing
    mPoint = MathUtil.CreatePoint(MyCenter)

    Dim CompTransform As MathTransform = Comp.Transform2
    mPoint = mPoint.MultiplyTransform(CompTransform)
    'return the x,y,z location in assembly space
    GetCircleCenter = mPoint.ArrayData
  End Function
End Module
