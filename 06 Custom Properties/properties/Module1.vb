Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Windows.Forms

Module Module1
    Public PropName() As String
    Public PropVal() As String
    Dim PropMgr As CustomPropertyManager
    Public m_swApp As SldWorks
    Sub CreateTestProps()
        'Property names
        PropName = {"LastSavedBy", "CreatedOn", "Revision", "Material"}

        'Property values
        ReDim PropVal(3)
        PropVal(0) = "$PRP:""SW-Last Saved By"""  '"$PRP:" & Chr(34) & "SW-Last Saved By" & Chr(34)
        PropVal(1) = Date.Today
        PropVal(2) = "A"
        PropVal(3) = """SW-Material""" 'Chr(34) & "SW-Material" & Chr(34)
    End Sub

    Sub SetAllProps()
        'adds or sets all properties from PropName and PropVal arrays
        Dim Part As ModelDoc2 = m_swApp.ActiveDoc
        Dim PropMgr As CustomPropertyManager
        Dim value As Integer
        PropMgr = Part.Extension.CustomPropertyManager("")
        For m As Integer = 0 To PropName.Length - 1
            value = PropMgr.Add3(PropName(m), swCustomInfoType_e.swCustomInfoText,
        PropVal(m), swCustomPropertyAddOption_e.swCustomPropertyReplaceValue)
        Next m

    End Sub

    Sub ReadFileProps()
        Dim Part As ModelDoc2 = m_swApp.ActiveDoc
        'make sure that Part is not nothing
        If Part Is Nothing Then
            MsgBox("Please open a file first.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        'get the custom property manager        
        PropMgr = Part.Extension.CustomPropertyManager("")

        'resize the PropVal array
        Dim propCt As Integer = PropMgr.Count
        ReDim PropVal(propCt - 1)
        ReDim PropName(propCt - 1)
        'fill in the array of properties
        For k As Integer = 0 To propCt - 1
            PropName(k) = PropMgr.GetNames(k)
            PropMgr.Get6(PropName(k), False, PropVal(k), "", Nothing, False)
        Next

    End Sub
    Sub LoadForm(ByVal swapp As SldWorks)
        'set the local SolidWorks variable to the running SolidWorks instance
        m_swApp = swapp
        'read the properties from the file
        ReadFileProps()

        'initialize the new form
        Dim PropDia As New PropsDialog
        'if there are properties, fill in the controls
        If PropName.Length > 0 Then
            'load the listbox with the property names
            PropDia.PropsListBox.Items.AddRange(PropName)
            'set the first item in the list te be active
            PropDia.PropsListBox.SelectedItem = 0
            'show the first item's value in the text box
            PropDia.ValueTextBox.Text = PropVal(0)
        End If
        'show the form to the user
        Dim DiaRes As DialogResult
        DiaRes = PropDia.ShowDialog

    End Sub

    Sub AddProperty(ByVal Name As String, ByVal Value As String)
        PropMgr.Add3(Name, swCustomInfoType_e.swCustomInfoText, Value,
        swCustomPropertyAddOption_e.swCustomPropertyReplaceValue)

        're-read the properties arrays
        ReadFileProps()
    End Sub
    Sub DeleteProperty(ByVal Name As String)
        PropMgr.Delete2(Name)

        're-read the properties arrays
        ReadFileProps()
    End Sub


End Module
