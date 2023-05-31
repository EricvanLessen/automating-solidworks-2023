Imports SolidWorks.Interop.sldworks
Public Class CustomProps
    Public Function PropExists(swDoc As ModelDoc2, propName As String, Optional config As String = "") As Boolean
        Dim cpm As CustomPropertyManager
        cpm = swDoc.Extension.CustomPropertyManager(config)
        Dim val As String
        val = cpm.Get(propName)
        If val = "" Then
            Return False
        End If
        Return True
    End Function
End Class
