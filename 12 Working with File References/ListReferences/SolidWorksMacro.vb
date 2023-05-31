Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System
Imports System.Data

Partial Class SolidWorksMacro

  Public Sub main()

    Dim refDataTable As DataTable
    ReferencesMod.m_swApp = swApp
    refDataTable = ReferencesMod.ReadRefs()
    Dim refDia As New Dialog1
    refDia.FilesGridView.DataSource = refDataTable
    refDia.ShowDialog()

  End Sub

  ''' <summary>
  ''' The SldWorks swApp variable is pre-assigned for you.
  ''' </summary>
  Public swApp As SldWorks


End Class
