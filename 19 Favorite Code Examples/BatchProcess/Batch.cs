using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchProcess
{
    using System;
    using System.IO;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;
    using System.Windows.Forms;
    using Microsoft.VisualBasic;

    internal static class Batch
    {
        internal static SldWorks swApp;
        private static StreamWriter LogFile;

        public static void Process(string FromFolder, string ToFolder)
        {

            // get the folders as DirectoryInfo interfaces
            DirectoryInfo FromDir = new DirectoryInfo(FromFolder);
            DirectoryInfo ToDir = new DirectoryInfo(ToFolder);

            // if the processing directory doesn't exist, 
            // tell the user and exit
            if (!FromDir.Exists)
            {
                MessageBox.Show(FromDir.FullName + " does not exist.", "Batch Process", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // if the output folder doesn't exist, create it
            if (!ToDir.Exists)
            {
                ToDir.Create();
            }

            // initialize the log file
            LogFile = new StreamWriter(ToDir.FullName + @"\Process.log");

            // save as another file format
            // change the extension value to whatever you want to export
            string Extension = ".PDF";
            int FileTypeToProcess = (int)swDocumentTypes_e.swDocDRAWING;
            SaveAs(FromDir, ToDir, Extension, FileTypeToProcess);

            // //save as DXF
            // Extension = ".DXF"
            // FileTypeToProcess = (int)swDocumentTypes_e.swDocDRAWING;
            // SaveAs(FromDir, ToDir, Extension, FileTypeToProcess);

            // //save parts as IGS
            // Extension = ".IGS";
            // FileTypeToProcess = (int)swDocumentTypes_e.swDocPART;
            // SaveAs(FromDir, ToDir, Extension, FileTypeToProcess);

            // add other processing as needed

            // finish up by closing out the log file
            LogFile.Flush();
            LogFile.Close();
        }

        public static void SaveAs(DirectoryInfo dinf, 
            DirectoryInfo outDir, string Ext, int TypeToProcess)
        {
            int longErrors = 0;
            int longWarnings = 0;

            // get the file extension based on file type
            FileInfo[] files;
            string filter = "";
            switch (TypeToProcess)
            {
                case (int)swDocumentTypes_e.swDocASSEMBLY:
                {
                        filter = ".SLDASM";
                        break;
                    }

                case (int)swDocumentTypes_e.swDocPART:
                {
                        filter = ".SLDPRT";
                        break;
                    }

                case (int)swDocumentTypes_e.swDocDRAWING:
                {
                        filter = ".SLDDRW";
                        break;
                    }
            }

            // get all files in the directory
            // that match the filter
            files = dinf.GetFiles(filter);
            foreach (FileInfo f in files)
            {
                ModelDoc2 Part;
                try
                {
                    Part = swApp.OpenDoc6(f.FullName, TypeToProcess, 
                        (int)swOpenDocOptions_e.swOpenDocOptions_Silent 
                        + (int)swOpenDocOptions_e.swOpenDocOptions_ReadOnly, 
                        "", ref longErrors, ref longWarnings);

                    string newFileName;
                    newFileName = Path.ChangeExtension(f.FullName, Ext);
                    Part.SaveAs2(newFileName, 
                        (int)swSaveAsVersion_e.swSaveAsCurrentVersion, true, true);

                    swApp.CloseDoc(Part.GetTitle());
                    Part = null;
                 }
                catch (Exception ex)
                {
                    WriteLogLine(DateTime.Now + Constants.vbTab 
                        + "ERROR" + Constants.vbTab 
                        + f.Name + Constants.vbTab + ex.Message);
                }
            }
        }

        public static void WriteLogLine(string LogString)
        {
            LogFile.WriteLine(DateAndTime.Now + Constants.vbTab + LogString);
        }
    }
}
