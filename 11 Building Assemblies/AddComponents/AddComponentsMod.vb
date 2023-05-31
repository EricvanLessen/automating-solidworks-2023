Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Windows.Forms

Module AddComponentsMod

  Public m_swApp As SldWorks

  Public Sub AddCompAndMate(ByVal PartPath As String)
    Dim Model As ModelDoc2 = m_swApp.ActiveDoc
    Dim MyView As ModelView = Model.ActiveView
    If Model.GetType <> swDocumentTypes_e.swDocASSEMBLY Then
      MsgBox("For use with assemblies only.", _
        MsgBoxStyle.Exclamation)
      Exit Sub
    End If
    Dim MyAssy As AssemblyDoc = Model
    'continue adding processing code here

  End Sub

  Private Function OpenPartInvisible(ByVal PartPath As String) As PartDoc
    Dim errors As Integer
    Dim warnings As Integer
    'open the file invisibly
    m_swApp.DocumentVisible(False, swDocumentTypes_e.swDocPART)
    Dim Part As PartDoc = m_swApp.OpenDoc6 _
      (PartPath, swDocumentTypes_e.swDocPART, _
      swOpenDocOptions_e.swOpenDocOptions_Silent, "", errors, warnings)
    m_swApp.DocumentVisible(True, swDocumentTypes_e.swDocPART)
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
