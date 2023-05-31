using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using EPDM.Interop.epdm;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace PDMConnection
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {
            // Create a file vault interface and log in to a vault
            IEdmVault5 vault = new EdmVault5();
            vault.LoginAuto("PDMVault", 0);
            //string username = "Admin";
            //string password = "admin";
            //string vaultname = "PDMVault";
            //vault.Login(username, password, vaultname);

            // Get the vault's root folder interface 
            string message;
            IEdmFile5 file;
            IEdmFolder5 folder;
            folder = vault.RootFolder;

            // Get position of first file in the root folder 
            IEdmPos5 pos;
            pos = folder.GetFirstFilePosition();
            if (pos.IsNull)
            {
                message = "The root folder of your vault does not contain any files.";
            }
            else
            {
                message = "The root folder of your vault contains these files:\n";
            }

            // For all files in the root folder, append the name to the message
            while (!pos.IsNull)
            {
                file = folder.GetNextFile(pos);
                message = message + file.Name + "\n";
            }

            // Show the names of all files in the root folder 
            MessageBox.Show(message);

            //Let the user select one or more files that are
            //n the file vault to which we are logged in.
            EdmStrLst5 PathList;
            PathList = vault.BrowseForFile(hParentWnd: 0, bsCaption: "Select a file to show its information");
            //Check if the user pressed Cancel
            if (PathList == null)
            {
                //do nothing - user pressed cancel

            }
            else
            {
                //Display a message with the paths of all selected files
                message = "You selected the following files:\n";
                pos = PathList.GetHeadPosition();
                while (!pos.IsNull)
                {
                    string filePath;
                    filePath = PathList.GetNext(pos);
                    //connect to the file
                    IEdmFile5 eFile;
                    IEdmFolder5 eFolder;
                    eFile = vault.GetFileFromPath(filePath, out eFolder);
                    message += filePath;
                    message += "\nState: " + eFile.CurrentState.Name;
                    message += "\nIs Checked Out: " + eFile.IsLocked.ToString();
                    if (eFile.IsLocked)
                    {
                        message += "\nIs Checked Out By: " + eFile.LockedByUser.Name;
                    }
                    //show information about the selected file
                    MessageBox.Show(message);
                }
            }

        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

