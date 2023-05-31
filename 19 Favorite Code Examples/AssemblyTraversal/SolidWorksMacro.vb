Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System

Partial Class SolidWorksMacro

  Sub main()
    Dim swModel As ModelDoc2
    Dim swConf As Configuration
    Dim swRootComp As Component2

    swModel = swApp.ActiveDoc
    swConf = swModel.GetActiveConfiguration
    swRootComp = swConf.GetRootComponent

    Diagnostics.Debug.Print("File = " _
      & swModel.GetPathName)

    TraverseComponent(swRootComp, 1)
  End Sub

  Sub TraverseComponent(ByVal swComp As Component2, _
  ByVal nLevel As Long)

    Dim ChildComps() As Object
    Dim swChildComp As Component2
    Dim sPadStr As String = ""
    Dim i As Long

    For i = 0 To nLevel - 1
      sPadStr = sPadStr + "  "
    Next i

    ChildComps = swComp.GetChildren
    For i = 0 To ChildComps.Length - 1
      swChildComp = ChildComps(i)

      TraverseComponent(swChildComp, nLevel + 1)
            openSave(swChildComp)
      Diagnostics.Debug.Print(sPadStr & swChildComp.Name2 _
        & " <" & swChildComp.ReferencedConfiguration & ">")
    Next i
    End Sub

    Dim fileList As String = ""

    Sub openSave(ByVal comp As Component2)
        Dim model As ModelDoc2 = comp.GetModelDoc2
        Dim filePath As String = model.GetPathName
        If fileList.Contains(filePath) Then
            'skip it
        Else
            model = swApp.ActivateDoc2(filePath, True, Nothing)
            'model.Extension.InsertScene("C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS (4)\data\graphics\scenes\01 basic scenes\00 3 point faded.p2s")
            model.ForceRebuild3(False)
            model.ShowNamedView2("", 1)
            model.ShowNamedView2("", 7)
            model.ViewZoomtofit2()
            model.GraphicsRedraw2()
            Dim errors As Long, warnings As Long
            'model.Save3(1, errors, warnings)
            errors = model.SaveAsSilent(filePath, False)
            swApp.CloseDoc(filePath)
            fileList = fileList & filePath
        End If

    End Sub

  ''' <summary>
  ''' The SldWorks swApp variable is pre-assigned for you.
  ''' </summary>
  Public swApp As SldWorks


End Class
