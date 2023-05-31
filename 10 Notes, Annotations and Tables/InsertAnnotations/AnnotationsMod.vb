Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System


Module AnnotationsMod

    Function GetBOMTable(swDoc As ModelDoc2) As BomTableAnnotation
        Dim swFeat As Feature
        Dim swBOMFeat As BomFeature = Nothing

        'traverse all features
        'looking for a BomFeature 
        swFeat = swDoc.FirstFeature
        Do While Not swFeat Is Nothing
            If swFeat.GetTypeName = "BomFeat" Then
                swBOMFeat = swFeat.GetSpecificFeature2
                Exit Do
            End If
            swFeat = swFeat.GetNextFeature
        Loop

        Dim myBOMTable As BomTableAnnotation
        Dim genTable As TableAnnotation
        If swBOMFeat Is Nothing Then
            MsgBox("No BOM table found.")
            Return Nothing
        Else
            If swBOMFeat.GetTableAnnotationCount = 1 Then
                myBOMTable = swBOMFeat.GetTableAnnotations(0)
                genTable = myBOMTable
                Debug.Print("First cell = " & genTable.DisplayedText2(0, 0, True))
            Else
                MsgBox("BOM is split. Only the first section will be returned.")
                Return swBOMFeat.GetTableAnnotations(0)
            End If
        End If

        Return myBOMTable
    End Function

    Sub ReadBOMTable(swBOMTable As BomTableAnnotation)
        'read the cells from the table
        Dim genTable As TableAnnotation = swBOMTable
        Dim columns As Long = genTable.ColumnCount
        Dim rows As Long = genTable.RowCount
        For i As Integer = 0 To rows - 1
            For j As Integer = 0 To columns - 1
                Debug.Write(genTable.DisplayedText2(i, j, True) & vbTab)
            Next
            Debug.WriteLine("")
        Next

    End Sub

    Function InsertBOMTable(swDoc As ModelDoc2) As BomTableAnnotation
        Dim swDrawing As DrawingDoc = Nothing
        swDrawing = swDoc

        'get the first view from the drawing
        Dim swActiveView As View = Nothing

        Dim swSheetView As View = Nothing
        swSheetView = swDrawing.GetFirstView 'sheet
        swActiveView = swSheetView.GetNextView 'first view

        'BOM Table definition variables
        Dim swBOMTable As BomTableAnnotation = Nothing
        Dim config As String =
        swActiveView.ReferencedConfiguration()
        Dim template As String = "C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\lang\english\bom-standard.sldbomtbt"

        'insert the table into the drawing 
        'based on the active view
        swBOMTable = swActiveView.InsertBomTable4(False, 0.2, 0.3,
        swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_TopRight,
        swBomType_e.swBomType_Indented, config, template,
        False, swNumberingType_e.swNumberingType_Detailed, False)
        Return swBOMTable

    End Function


    Sub InsertGeneralTable(swDoc As ModelDoc2)
        Dim swDrawing As DrawingDoc = swDoc
        Dim myTable As TableAnnotation = Nothing
        'table definition values
        'location in meters
        Dim tableLocX As Double = 0.15
        Dim tableLocY As Double = 0.05
        'if no template is used, specify the number 
        'of columns and rows
        Dim columns As Long = 2
        Dim rows As Long = 2
        Dim Anchor As Boolean = False
        'place at X,Y location
        'optionally set the full path to a table template
        Dim tableTemplate As String = ""

        'insert the table
        myTable = swDrawing.InsertTableAnnotation2 _
        (Anchor, tableLocX, tableLocY,
        swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_TopRight,
        tableTemplate, rows, columns)

        'add table text
        myTable.Text2(0, 0, True) = "Row 1 Column 1"
        myTable.Text2(0, 1, True) = "Row 1 Column 2"
        myTable.Text2(1, 0, True) = "Row 2 Column 1"
        myTable.Text2(1, 1, True) = "$PRP:""SW-Sheet Name(Sheet Name)"""

        'read some table text
        MsgBox(myTable.Text2(1, 1, True) & vbCrLf _
        & myTable.DisplayedText2(1, 1, True))

    End Sub
    Sub AddFileNameNote(swDoc As ModelDoc2)
        Dim boolstatus As Boolean = False
        Dim longstatus As Integer = 0

        Dim myNote As Note = Nothing
        Dim myAnnotation As Annotation = Nothing
        Dim myTextFormat As TextFormat = Nothing
        Dim myDrawing As DrawingDoc = swDoc
        Dim mySheet As Sheet
        Dim width As Double
        Dim height As Double
        mySheet = myDrawing.GetCurrentSheet
        width = mySheet.GetProperties2(5)  'sheet width
        height = mySheet.GetProperties2(6) 'sheet height
        myDrawing.EditSheet()
        myNote = swDoc.InsertNote("$PRPSHEET:""SW-File Name""")

        If (Not (myNote) Is Nothing) Then
            myNote.LockPosition = False
            myNote.Angle = Math.PI / 2
            boolstatus = myNote.SetBalloon(0, 0)
            myAnnotation = myNote.GetAnnotation()
            If (Not (myAnnotation) Is Nothing) Then
                longstatus =
        myAnnotation.SetLeader3(swLeaderStyle_e.swNO_LEADER,
        0, True, False, False, False)
                boolstatus = myAnnotation.SetPosition2(width - 0.01, 0.015, 0)
                'boolstatus = myAnnotation.SetTextFormat(0, True, myTextFormat)
            End If
        End If

        swDoc.ClearSelection2(True)
        swDoc.WindowRedraw()
    End Sub

    Sub AddDatum(swDoc As ModelDoc2)
        'an edge must be pre-selected 
        Dim selMgr As SelectionMgr = swDoc.SelectionManager
        If selMgr.GetSelectedObjectCount = 0 Then Exit Sub

        'no datum will be created otherwise
        Dim myDatumTag As DatumTag
        myDatumTag = swDoc.InsertDatumTag2
    End Sub


End Module
