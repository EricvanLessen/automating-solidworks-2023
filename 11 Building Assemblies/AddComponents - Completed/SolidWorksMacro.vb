Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System

Partial Class SolidWorksMacro

  Public Sub main()
    Dim MyDialog As New Dialog1
        AddComponents.m_swApp = swApp
        MyDialog.Show()
  End Sub

  ''' <summary>
  ''' The SldWorks swApp variable is pre-assigned for you.
  ''' </summary>
  Public swApp As SldWorks


End Class
