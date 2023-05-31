using System;
using System.Windows.Forms;
using EPDM.Interop.epdm;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace PDMVariables
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {
            // Create a file vault interface and log in to a vault
            IEdmVault5 vault = new EdmVault5();
            vault.LoginAuto("PDMVault", 0);            

            //Let the user select one or more files that are
            //in the file vault to which we are logged in  
            EdmStrLst5 PathList;
            PathList = vault.BrowseForFile(0, bsCaption: "Select a file to show its information");

            //Check if the user pressed Cancel
            if (PathList == null)
            {
                //do nothing - user pressed cancel
            }
            else
            {
                string message;
                IEdmPos5 pos;
                //Display a message box with 
                //the paths of all selected files.   
                message = "You selected the following files:\n";
                pos = PathList.GetHeadPosition();
                while (!pos.IsNull)
                {
                    string filePath;
                    filePath = PathList.GetNext(pos);
                    //connect to the file object
                    IEdmFile5 eFile;
                    IEdmFolder5 eFolder;
                    eFile = vault.GetFileFromPath(filePath, out eFolder);
                    message += filePath;
                    message += "\nState: " + eFile.CurrentState.Name;
                    message += "\nIs Checked Out: " + eFile.IsLocked.ToString();

                    //get the file's EnumeratorVariable interface
                    //for working with its card variables
                    IEdmEnumeratorVariable10 eVar;
                    eVar = (IEdmEnumeratorVariable10)eFile.GetEnumeratorVariable();

                    //get the description
                    object varValue;
                    eVar.GetVar("Description", "@", out varValue);
                    message += "\nDescription: " + varValue.ToString();

                    //try to check out the file
                    try
                    {
                        eFile.LockFile(eFolder.ID, 0);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(eFile.Name);
                        return;
                    }

                    //edit the file here
                    //reconnec to the file's EnumeratorVariable after check out
                    eVar = (IEdmEnumeratorVariable10)eFile.GetEnumeratorVariable();
                    try
                    {
                        eVar.SetVar("Description", "@", "NEW DESCRIPTION");
                        //close the file and save (flush) any updates
                        eVar.CloseFile(true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n" + eFile.Name);
                        return;
                    }

                    try
                    {
                        eFile.UnlockFile(0, "API check in");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n" + eFile.Name);
                        return;
                    }

                    //show information about the file
                    MessageBox.Show(message);
                }
            }
        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

