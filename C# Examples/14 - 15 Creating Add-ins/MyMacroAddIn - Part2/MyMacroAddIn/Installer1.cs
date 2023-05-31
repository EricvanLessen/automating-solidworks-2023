using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MyMacroAddIn
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        public Installer1()
        {
            InitializeComponent();
            BeforeUninstall += new InstallEventHandler(Installer1_BeforeUninstall);
            AfterInstall += new InstallEventHandler(Installer1_AfterInstall);
        }

        public void Installer1_AfterInstall(object sender, InstallEventArgs e)
        {
            RegistrationServices regsrv = new RegistrationServices();
            regsrv.RegisterAssembly(asm, AssemblyRegistrationFlags.SetCodeBase);
        }

        private void Installer1_BeforeUninstall(object sender, InstallEventArgs e)
        {
            RegistrationServices regsrv = new RegistrationServices();
            regsrv.UnregisterAssembly(asm);
        }

    }


}
