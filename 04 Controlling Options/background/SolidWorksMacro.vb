Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Runtime.InteropServices
Imports System


Partial Class SolidWorksMacro
  Public Sub main()
    Dim result As Boolean
    Dim swModel As ModelDoc2 = Nothing
    swModel = swApp.ActiveDoc

    If swApp.GetUserPreferenceIntegerValue(
  swUserPreferenceIntegerValue_e.swColorsBackgroundAppearance) =
  swColorsBackgroundAppearance_e.swColorsBackgroundAppearance_DocumentScene _
  Then

      result = swApp.SetUserPreferenceIntegerValue(
    swUserPreferenceIntegerValue_e.swColorsBackgroundAppearance,
    swColorsBackgroundAppearance_e.swColorsBackgroundAppearance_Plain)

    Else
      result = swApp.SetUserPreferenceIntegerValue _
      (swUserPreferenceIntegerValue_e.swColorsBackgroundAppearance,
      swColorsBackgroundAppearance_e.
      swColorsBackgroundAppearance_DocumentScene)

    End If

    swModel.GrahicsRedraw2
  End Sub
  ''' <summary>
  ''' The SldWorks swApp variable is pre-assigned for you.
  ''' </summary>
  Public swApp As SldWorks
End Class
















