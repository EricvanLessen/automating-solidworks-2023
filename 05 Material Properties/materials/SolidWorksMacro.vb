Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System
Imports System.Globalization
Imports System.Windows.Forms

Partial Class SolidWorksMacro
    Public Sub main()
        'Initialize the dialog
        Dim MyMaterials As New Dialog1
        Dim MyCombo As Windows.Forms.ComboBox
        MyCombo = MyMaterials.MaterialsCombo

        'set up materials list
        Dim MyProps() As String = {"Alloy Steel", "6061 Alloy", "ABS PC"}
        MyCombo.Items.AddRange(MyProps)

        Dim Result As DialogResult
        Result = MyMaterials.ShowDialog()

        If Result = DialogResult.OK Then
            Dim Model As ModelDoc2 = swApp.ActiveDoc
            If Model Is Nothing Then
                MsgBox("You must first open a file.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            If Model.GetType = swDocumentTypes_e.swDocPART Then
                'Assign the material to the part
                Dim Part As PartDoc = Model
                'Part = swApp.ActiveDoc
                Part.SetMaterialPropertyName2("Default",
                   "SOLIDWORKS Materials.sldmat", MyCombo.Text)
            ElseIf Model.GetType = swDocumentTypes_e.swDocASSEMBLY Then
                Dim Assy As AssemblyDoc = Model
                'set materials on selected components
                Dim SelMgr As SelectionMgr
                SelMgr = Model.SelectionManager
                Dim Comp As Component2
                Dim compModel As ModelDoc2
                If SelMgr.GetSelectedObjectCount2(-1) < 1 Then
                    MsgBox("You must select at least one component.", MsgBoxStyle.Exclamation)
                    Exit Sub
                End If

                For i As Integer = 1 To SelMgr.GetSelectedObjectCount2(-1)
                    Comp = SelMgr.GetSelectedObjectsComponent4(i, -1)
                    compModel = Comp.GetModelDoc2
                    If compModel.GetType = swDocumentTypes_e.swDocPART Then
                        compModel.SetMaterialPropertyName2("Default", "SOLIDWORKS Materials.sldmat", MyCombo.Text)
                    End If
                Next

            End If
        End If




    End Sub
    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks
End Class
