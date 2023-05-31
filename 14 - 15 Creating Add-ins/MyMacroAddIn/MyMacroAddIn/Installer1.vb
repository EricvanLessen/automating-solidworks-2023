Imports System.ComponentModel
Imports System.Configuration.Install
Imports System.Runtime.InteropServices

Public Class Installer1

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add initialization code after the call to InitializeComponent

    End Sub

    Private Sub Installer1_AfterInstall(sender As Object, e As InstallEventArgs) Handles Me.AfterInstall
        Dim regsrv As New RegistrationServices
        regsrv.RegisterAssembly(MyBase.GetType().Assembly, AssemblyRegistrationFlags.SetCodeBase)

    End Sub

    Private Sub Installer1_BeforeUninstall(sender As Object, e As InstallEventArgs) Handles Me.BeforeUninstall
        Dim regsrv As New RegistrationServices
        regsrv.UnregisterAssembly(MyBase.GetType().Assembly)
    End Sub
End Class
