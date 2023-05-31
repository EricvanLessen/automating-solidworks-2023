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
        swDoc = swApp.ActiveDoc

        '
        'New Document
        Dim swSheetWidth As Double
        swSheetWidth = 0.5588
        Dim swSheetHeight As Double
        swSheetHeight = 0.43179999999999996
        swDoc = swApp.NewDocument("C:\ProgramData\SolidWorks\SOLIDWORKS 2022\templates\Drawing.drwdot", 12, swSheetWidth, swSheetHeight)
        swDrawing = swDoc
        Dim swSheet As Object = Nothing
        swSheet = swDrawing.GetCurrentSheet()
        swSheet.SetProperties2(12, 12, 1, 1, False, swSheetWidth, swSheetHeight, True)
        swSheet.SetTemplateName("C:\ProgramData\SolidWorks\SOLIDWORKS 2023\lang\english\sheetformat\c - landscape." &
                "slddrt")
        swSheet.ReloadTemplate(True)
        swDrawing = swDoc
        boolstatus = swDrawing.Create3rdAngleViews("C:\...\PlanetGear.SLDPRT") 'enter a real file path
        Dim myView As View = Nothing
        swDrawing = CType(swDoc, DrawingDoc)
        myView = swDrawing.CreateDrawViewFromModelView3("C:\...\PlanetGear.SLDPRT",
                                                        "*Isometric", 0.4340922240373396R, 0.3055803757292882R, 0)  'enter a real file path
        boolstatus = swDrawing.ActivateView("Drawing View4")
        Dim vAnnotations As Array = Nothing

        vAnnotations = swDrawing.InsertModelAnnotations3(0, 32776, True, True, False, True)
        '
        'Zoom To Fit
        swDoc.ViewZoomtofit2()

        '
        'Save As
        longstatus = swDoc.SaveAs3("C:\...\PlanetGear.SLDDRW", 0, 0)  'enter a real file path
        'latest saveas method
        'swDoc.Extension.SaveAs3("C:\...\PlanetGear.SLDDRW", swSaveAsVersion_e.swSaveAsCurrentVersion, swSaveAsOptions_e.swSaveAsOptions_Silent, Nothing, Nothing, 0, 0)
    End Sub
    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks
End Class
