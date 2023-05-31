Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports SolidWorks.Interop.swpublished

Public Class UserPMPage
    Dim iSwApp As SldWorks
    Dim userAddin As SwAddin
    Dim handler As PMPageHandler
    Friend PropMgrPage As PropertyManagerPage2

#Region "Property Manager Page Controls"
    'Groups
    Dim group2 As PropertyManagerPageGroup

    'Controls
    Dim selection1 As PropertyManagerPageSelectionbox
    Dim button1 As PropertyManagerPageButton
    Friend text1 As PropertyManagerPageTextbox

    'Control IDs
    Dim group2ID As Integer = 1
    Dim selection1ID As Integer = 7
    Friend buttonID1 As Integer = 12
    Dim textID1 As Integer = 14

#End Region

    Sub Init(ByVal sw As SldWorks, ByVal addin As SwAddin)
        iSwApp = sw
        userAddin = addin
        CreatePage()
        AddControls()
    End Sub

    Sub Show()
        PropMgrPage.Show()
    End Sub

    Sub CreatePage()
        handler = New PMPageHandler()
        handler.Init(iSwApp, userAddin, Me)
        Dim options As Integer
        Dim errors As Integer
        options = swPropertyManagerPageOptions_e.swPropertyManagerOptions_OkayButton + swPropertyManagerPageOptions_e.swPropertyManagerOptions_CancelButton
        PropMgrPage = iSwApp.CreatePropertyManagerPage("Add Component", options, handler, errors)
    End Sub

    Sub AddControls()
        Dim options As Integer
        Dim leftAlign As Integer
        Dim controlType As Integer
        Dim retval As Boolean

        ' Add Message
        Dim message As String
        message = "Select one or more circular edges where the component will be mated. " _
      & "Browse to a fastener to insert, then click OK."
        retval = PropMgrPage.SetMessage3(message,
                                      swPropertyManagerPageMessageVisibility.swImportantMessageBox,
                                      swPropertyManagerPageMessageExpanded.swMessageBoxExpand,
                                      "Message")

        'Add Groups
        options = swAddGroupBoxOptions_e.swGroupBoxOptions_Checkbox + swAddGroupBoxOptions_e.swGroupBoxOptions_Visible
        group2 = PropMgrPage.AddGroupBox(group2ID, "Add Component", options)


        'Add Controls to Group2
        'Selection1
        controlType = swPropertyManagerPageControlType_e.swControlType_Selectionbox
        leftAlign = swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge
        options = swAddControlOptions_e.swControlOptions_Enabled + swAddControlOptions_e.swControlOptions_Visible
        selection1 = group2.AddControl(selection1ID, controlType, "Select Circular Edges", leftAlign, options,
                                       "Select circular edges on flat faces where components will be added")
        If Not selection1 Is Nothing Then
            Dim filter() As Integer = New Integer() {swSelectType_e.swSelEDGES}
            selection1.Height = 50
            selection1.SetSelectionFilters(filter)
        End If

        'Label
        group2.AddControl(100, 1, "File path", 1, 3, "")

        'Textbox1
        controlType = swPropertyManagerPageControlType_e.swControlType_Textbox
        leftAlign = swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge
        options = swAddControlOptions_e.swControlOptions_Enabled +
                    swAddControlOptions_e.swControlOptions_Visible
        text1 = group2.AddControl2(textID1, controlType, "File path",
                    leftAlign, options, "File path")

        'Button
        controlType = swPropertyManagerPageControlType_e.swControlType_Button
        leftAlign = swPropertyManagerPageControlLeftAlign_e.swControlAlign_DoubleIndent
        options = swAddControlOptions_e.swControlOptions_Enabled +
                    swAddControlOptions_e.swControlOptions_Visible
        button1 = group2.AddControl2(buttonID1, controlType, "Browse...",
                    leftAlign, options, "Browse to a part")

    End Sub

End Class

