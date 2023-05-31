using System;
using System.Windows.Forms;
using EPDM.Interop.epdm;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace PDMAddFiles
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {
            EdmVault5 vault = new EdmVault5();
            vault.LoginAuto("PDMVault", 0);

            IEdmFolder5 eFolder;
            eFolder = vault.BrowseForFolder(0, "Select the destination folder");

            string filePath;
            int fileID;
            IEdmFile5 eFile;
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult diaRes;
            diaRes = ofd.ShowDialog();
            if (diaRes == DialogResult.OK)
            {
                filePath = ofd.FileName;
                //try to add the file to the selected folder
                try
                {
                    fileID = eFolder.AddFile(0, filePath);
                    eFile = (IEdmFile5)vault.GetObject(EdmObjectType.EdmObject_File, fileID);
                }
                catch (Exception ex)
                {
                    //failed to add the file
                    MessageBox.Show(ex.Message);
                    return;
                }

                //update the Description with the file name
                try
                {
                    IEdmEnumeratorVariable8 eVar;
                    eVar = (IEdmEnumeratorVariable8)eFile.GetEnumeratorVariable();
                    string newDescription;
                    newDescription = System.IO.Path.GetFileNameWithoutExtension(eFile.Name);
                    eVar.SetVar("Description", "@", newDescription);
                    eVar.CloseFile(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + eFile.Name);
                    return;
                }

                //check in the file
                try
                {
                    eFile.UnlockFile(0, "API check in");
                    MessageBox.Show(eFile.Name + " added to the vault.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

