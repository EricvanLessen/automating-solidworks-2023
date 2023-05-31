Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System
Imports EPDM.Interop.epdm

Partial Class SolidWorksMacro
    Public Sub main()
        'Create a file vault interface 
        'and log in to a vault. 
        Dim vault As EdmVault5 = New EdmVault5
        Dim vaultName As String = "PDMVault"
        vault.LoginAuto(vaultName, 0)

        'Let the user select a SOLIDWORKS file    
        Dim PathList As EdmStrLst5
        PathList = vault.BrowseForFile(0, ,
"SOLIDWORKS Files (*.sld*)|*.sld*||", , , ,
"Select a file to show its information")

        'Check if the user pressed Cancel
        If PathList Is Nothing Then
            'do nothing - user pressed cancel
        Else
            Dim pos As IEdmPos5
            pos = PathList.GetHeadPosition
            While Not pos.IsNull
                Dim filePath As String
                filePath = PathList.GetNext(pos)

                'connect to the file object
                Dim eFile As IEdmFile7
                Dim parentFolder As IEdmFolder5
                eFile = vault.GetFileFromPath(filePath, parentFolder)

                'get a specific BOM view
                Dim bom As IEdmBomView3
                bom = eFile.GetComputedBOM("Basic BOM", 0, "@", EdmBomFlag.EdmBf_AsBuilt)

                ReadComputedBOM(bom)

                If Not bom Is Nothing Then
                    bom.SaveToCSV("C:\temp\" & eFile.Name & ".csv", True)
                End If

                Dim namedBOMs() As EdmBomInfo
                eFile.GetDerivedBOMs(namedBOMs)

                ReadNamedBOMs(namedBOMs, vault)

            End While
        End If

    End Sub

    Private Sub ReadNamedBOMs(namedBOMs() As EdmBomInfo, vault As EdmVault5)
        Dim bom As IEdmBomView3
        If namedBOMs.Length > 0 Then
            'found at least one Named BOM
            For Each bomInf As EdmBomInfo In namedBOMs
                Dim nbom As EdmBom = vault.GetObject(
                  EdmObjectType.EdmObject_BOM, bomInf.mlBomID)
                If nbom.CurrentState.Name <> "" Then
                    'found a current Named BOM
                    bom = nbom.GetView
                    Debug.Print("Named BOM: " & nbom.Name)

                    'get column headers
                    Dim columns() As EdmBomColumn
                    bom.GetColumns(columns)
                    Dim header As String = "LEVEL" & vbTab
                    For Each column As EdmBomColumn In columns
                        header = header & column.mbsCaption & vbTab
                    Next
                    Debug.Print(header)

                    'read each BOM row
                    Dim rows() As Object
                    bom.GetRows(rows)
                    Dim row As IEdmBomCell
                    For Each row In rows
                        If IsNothing(row) Then Exit For
                        Dim rowString As String = row.GetTreeLevel.ToString & vbTab
                        Dim varVal As String = ""
                        For Each column As EdmBomColumn In columns
                            row.GetVar(column.mlVariableID, column.meType, varVal, Nothing, Nothing, Nothing)
                            If IsNothing(varVal) Then varVal = ""
                            rowString = rowString & varVal & vbTab
                        Next
                        'print the row
                        Debug.WriteLine(rowString)
                    Next
                End If
            Next
        End If
    End Sub

    Private Sub ReadComputedBOM(bom As IEdmBomView3)

        'get column headers
        Dim columns() As EdmBomColumn
        bom.GetColumns(columns)
        Dim header As String = "LEVEL" & vbTab
        For Each column As EdmBomColumn In columns
            header = header & column.mbsCaption & vbTab
        Next
        Debug.Print(header)

        'read each BOM row
        Dim rows() As Object
        bom.GetRows(rows)
        Dim row As IEdmBomCell
        For Each row In rows
            If IsNothing(row) Then Exit For
            Dim rowString As String = row.GetTreeLevel.ToString & vbTab
            Dim varVal As String = ""
            For Each column As EdmBomColumn In columns
                row.GetVar(column.mlVariableID, column.meType, varVal, Nothing, Nothing, Nothing)
                If IsNothing(varVal) Then varVal = ""
                rowString = rowString & varVal & vbTab
            Next
            'print the row
            Debug.Print(rowString)
        Next
    End Sub
    ''' <summary>
    ''' The SldWorks swApp variable is pre-assigned for you.
    ''' </summary>
    Public swApp As SldWorks
End Class
