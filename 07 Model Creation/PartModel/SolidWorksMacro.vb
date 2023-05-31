Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System


Partial Class SolidWorksMacro
    Public Sub main()

        Dim swDoc As ModelDoc2 = Nothing
        Dim swPart As PartDoc = Nothing
        Dim swDrawing As DrawingDoc = Nothing
        Dim swAssembly As AssemblyDoc = Nothing
        Dim boolstatus As Boolean = False
        Dim longstatus As Integer = 0
        Dim longwarnings As Integer = 0
        '
        'New Document
        Dim swSheetWidth As Double
        swSheetWidth = 0
        Dim swSheetHeight As Double
        swSheetHeight = 0
        swDoc = swApp.NewPart 'CType(swApp.NewDocument("C:\REPLACE WITH A REAL PATH\Templates\Part.prtdot", 0, swSheetWidth, swSheetHeight), ModelDoc2)
        swDoc.ActiveView.enablegraphicsupdate = False
        swPart = swDoc

        'Store the user's setting
        Dim usersSetting As Boolean
        usersSetting = swApp.GetUserPreferenceToggle _
        (swUserPreferenceToggle_e.swInputDimValOnCreate)
        swApp.SetUserPreferenceToggle _
        (swUserPreferenceToggle_e.swInputDimValOnCreate, False)

        'create sketch
        Dim length As Double
        Dim xLoc As Double
        Dim yLoc As Double
        Dim userLength As String
        userLength = InputBox("Enter the line length in meters:")
        If Not IsNumeric(userLength) Then
            MsgBox("Please enter only values in meters.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        length = CType(userLength, Double)

        boolstatus = swDoc.Extension.SelectByID2("Front Plane", "PLANE", 0, 0, 0,
        False, 0, Nothing, 0)
        swDoc.SketchManager.InsertSketch(True)
        swDoc.ClearSelection2(True)
        Dim skSegment As SketchSegment = Nothing
        skSegment = swDoc.SketchManager.CreateLine(0, 0, 0, 0, length, 0)
        yLoc = length / 2
        xLoc = -(length / 2)
        Dim myDisplayDim As DisplayDimension = Nothing
        myDisplayDim = swDoc.AddDimension2(xLoc, yLoc, 0)
        Dim myDimension As Dimension = Nothing
        'myDimension = swDoc.Parameter("D1@Sketch1")
        'more generic technique that does not rely on dimension names
        'get the dimension directly from the created display dimension
        myDimension = myDisplayDim.GetDimension()
        myDimension.SystemValue = length
        skSegment = swDoc.SketchManager.CreateLine(0, 0, 0, length, 0, 0)
        xLoc = length / 2
        yLoc = -(length / 2)
        myDisplayDim = swDoc.AddDimension2(xLoc, yLoc, 0)
        'myDimension = swDoc.Parameter("D2@Sketch1")
        myDimension = myDisplayDim.GetDimension()
        myDimension.SystemValue = length

        'Restore the user's setting
        swApp.SetUserPreferenceToggle _
        (swUserPreferenceToggle_e.swInputDimValOnCreate, usersSetting)

        '
        'Named View
        swDoc.ShowNamedView2("*Trimetric", 8)

        Dim myFeature As Feature = Nothing
        myFeature = CType(swDoc.FeatureManager.FeatureExtrusionThin2(True,
        False, False, 0, 0, 0.1R, 0.01R, False, False, False, False,
        0.017453292519943334R, 0.017453292519943334R, False, False,
        False, False, True, 0.01R, 0.01R, 0.01R, 0, 0, False, 0.005R,
        True, True, 0, 0, False), Feature)
        swDoc.ISelectionManager.EnableContourSelection = False
        swDoc.ActiveView.EnableGraphicsUpdate = False
        swDoc.ViewZoomtofit2()

        swDoc.ClearSelection2(True)
        boolstatus = swDoc.Extension.SelectByRay(0.047617750872973374R, 0, 0.046577984656096305R, -0.40003602677931249R, -0.51503807491002407R, -0.75809429405028372R, 0.0014057224606580827R, 2, False, 0, 0)
        swDoc.SketchManager.InsertSketch(True)
        swDoc.ClearSelection2(True)
        Dim vSkLines As Array = Nothing
        vSkLines = CType(swDoc.SketchManager.CreateCornerRectangle(length, -0.0728368268352142R, 0, 0.0525843427129189R, -0.028526944014799938R, 0), Array)
        myFeature = CType(swDoc.FeatureManager.FeatureCut4(True, False, False, 1, 0, 0.01R, 0.01R, False, False, False, False, 0.017453292519943334R, 0.017453292519943334R, False, False, False, False, False, True, True, True, True, False, 0, 0, False, False), Feature)
        swDoc.ISelectionManager.EnableContourSelection = False
        boolstatus = swDoc.Extension.SelectByRay(0, 0.04986486960530101R, 0.049359275006281678R, -0.40003602677931249R, -0.51503807491002407R, -0.75809429405028372R, 0.0014057224606580827R, 2, False, 0, 0)
        '
        'Hole Wizard
        Dim swHoleFeature As Object = Nothing
        swHoleFeature = swDoc.FeatureManager.HoleWizard5(1, 1, 36, "M10", 1, 0.0105R, 0.01R, -1, 0.02R, 1.5707963267948966R, 0, 0, 0, 0, 0, 0, 0, -1, -1, -1, "", False, True, True, True, True, False)

        'create fillet
        swDoc.ClearSelection2(True)
        boolstatus = swDoc.Extension.SelectByRay(0.0005820422746865006R, -0.00045207896334886755R, 0.0420341416409542R, -0.40003602677931249R, -0.51503807491002407R, -0.75809429405028372R, 0.00088016786570743407R, 1, False, 1, 0)
        Dim swFeatData As SimpleFilletFeatureData2 = Nothing
        swFeatData = CType(swDoc.FeatureManager.CreateDefinition(swFeatureNameID_e.swFmFillet), SimpleFilletFeatureData2)
        '
        swFeatData.Initialize(swSimpleFilletType_e.swConstRadiusFillet)
        '
        'Dim swEdge As Object = Nothing
        'Dim edgesArray(0) As Object = Nothing
        'swEdge = CType(swDoc.ISelectionManager.GetSelectedObject6(1, 1),Object)
        'Dim edgesVar As [Variant]
        'swFeatData.Edges = edgesVar
        '
        'swFeatData.AsymmetricFillet = false
        swFeatData.DefaultRadius = 0.01R
        'swFeatData.ConicTypeForCrossSectionProfile = swFeatureFilletCircular
        'swFeatData.CurvatureContinuous = false
        'swFeatData.ConstantWidth = 0.01R
        'swFeatData.IsMultipleRadius = false
        'swFeatData.OverflowType = swFilletOverFlowType_Default
        '
        Dim swFeature As Feature = Nothing
        swFeature = CType(swDoc.FeatureManager.CreateFeature(swFeatData), Feature)
        swDoc.ActiveView.EnableGraphicsUpdate = True

    End Sub
    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks
End Class
