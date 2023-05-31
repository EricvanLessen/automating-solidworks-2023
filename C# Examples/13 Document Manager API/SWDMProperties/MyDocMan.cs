using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorks.Interop.swdocumentmgr;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace SWDMProperties
{
class MyDocMan
{
    SwDMApplication swDMApp;
    public SwDMApplication DocManApp
    {   
        get {
            if (swDMApp == null)
            {
                SwDMClassFactory swDMClassFact = new SwDMClassFactory();
                swDMApp = (SwDMApplication)swDMClassFact.GetApplication("ENTER LICENSE KEY HERE");                    
            }
            return swDMApp; }
    }

        #region File Operations
        public string BrowseForDoc()
        {
            string filePath = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SOLIDWORKS Files (*.sld*)|*.sld*"
                + "|SOLIDWORKS Part (*.sldprt)|*.sldprt"
                + "|SOLIDWORKS Assembly (*.sldasm)|*.sldasm"
                + "|SOLIDWORKS Drawing (*.slddrw)|*.slddrw"
                + "|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
            }
            if (filePath == "")
            {
                return null;
            }
            return filePath;
        }

        public SwDMDocument OpenDoc(string filePath, bool isReadOnly)
        {
            string ext = Path.GetExtension(filePath).ToUpper();
            SwDmDocumentType docType;
            switch (ext)
            {
                case "SLDPRT":
                    docType = SwDmDocumentType.swDmDocumentPart;
                    break;
                case "SLDASM":
                    docType = SwDmDocumentType.swDmDocumentAssembly;
                    break;
                case "SLDDRW":
                    docType = SwDmDocumentType.swDmDocumentDrawing;
                    break;
                default:
                    docType = SwDmDocumentType.swDmDocumentUnknown; 
                    break;
            }

            SwDmDocumentOpenError openerrors = 0;
            SwDMDocument dmDoc = DocManApp.GetDocument(filePath,
                docType, isReadOnly, out openerrors);

            //error handling
            if (dmDoc == null)
            {
                string addToMessage;
                switch (openerrors)
                {
                    case SwDmDocumentOpenError.swDmDocumentOpenErrorFail:
                        addToMessage = "Failed to access the document.";
                        break;
                    case SwDmDocumentOpenError.swDmDocumentOpenErrorNonSW:
                        addToMessage = "Not a SOLIDWORKS file.";
                        break;
                    case SwDmDocumentOpenError.swDmDocumentOpenErrorFileNotFound:
                        addToMessage = "File not found.";
                        break;
                    case SwDmDocumentOpenError.swDmDocumentOpenErrorFileReadOnly:
                        addToMessage = "File is Read Only or is open in another application.";
                        break;
                    case SwDmDocumentOpenError.swDmDocumentOpenErrorNoLicense:
                        addToMessage = "Invalid Document Manager license.";
                        break;
                    case SwDmDocumentOpenError.swDmDocumentOpenErrorFutureVersion:
                        addToMessage = "Unsupported file version.";
                        break;
                    default:
                        addToMessage = "";
                        break;
                }
                string errMessage = "Error accessing " + filePath 
                    + System.Environment.NewLine + addToMessage;
                MessageBox.Show(errMessage, "SWDMProperties", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
            return dmDoc;
        }
        #endregion


        #region Properties
        public string ReadAllProps(SwDMDocument dmDoc)
        {
            //read all file custom properties
            string output;
            output = dmDoc.FullName + " Properties:";
            object[] propNames = dmDoc.GetCustomPropertyNames();
            if (propNames != null)
            {
                foreach (string propName in propNames)
                {
                    string propVal;
                    SwDmCustomInfoType myType;
                    propVal = dmDoc.GetCustomProperty(propName, out myType);
                    output += System.Environment.NewLine + propName + "\t" + propVal;
                }
            }
            return output;
        }

        public string ReadConfiProps(SwDMDocument dmDoc)
        {
            //read all configurations
            ISwDMConfigurationMgr configMan;
            configMan = dmDoc.ConfigurationManager;
            try
            {
                object[] configNames = configMan.GetConfigurationNames();
                string output = "Configurations:";
                foreach (string config in configNames)
                {
                    output += "\nConfiguration: " + config;
                    //get configuration specific properties
                    SwDMConfiguration dmConfig;
                    dmConfig = configMan.GetConfigurationByName(config);
                    object[] propNames = dmConfig.GetCustomPropertyNames();
                    if (propNames != null)
                    {
                        foreach (string propName in propNames)
                        {
                            string propVal;
                            SwDmCustomInfoType myType;
                            propVal = dmConfig.GetCustomProperty(propName, out myType);
                            output += System.Environment.NewLine + propName + "\t" + propVal;
                        }
                    }
                }            
                return output; 
            }
            catch (Exception)
            {
                //ignore the error for drawings
                return "";                
            }
            
        }

        public void WriteProp(SwDMDocument dmDoc,
            string pName, string pVal)
        {
            //write a property
            bool addRes;
            addRes = dmDoc.AddCustomProperty(pName,
                SwDmCustomInfoType.swDmCustomInfoText, pVal);
            if (!addRes)
            {
                dmDoc.SetCustomProperty(pName, pVal);
            }
        }
        #endregion

        #region Tables
        public string GetBOMTableText(SwDMDocument10 dmDoc)
        {
            string tableText = "";
            object[] tableNames = dmDoc.GetTableNames(SwDmTableType.swDmTableTypeBOM);
            foreach (string table in tableNames)
            {
                SwDMTable5 dmTable = (SwDMTable5)dmDoc.GetTable(table);
                Debug.Print(dmTable.Name);
                int rows;
                int columns;
                SwDmTableError errors;
                string[] ttext = dmTable.GetTableCellText(out errors, out rows, out columns);
                tableText = "Table: " + dmTable.Name + Environment.NewLine;
                //format the cell text array into a tab delimited text file
                int cells = 0;
                for (int i = 1; i <= rows; i++)
                {
                    string rowText = "";
                    for (int j = 1; j <= columns; j++)
                    {
                        rowText += ttext[cells] + "\t";
                        cells++;
                    }
                    tableText += rowText + Environment.NewLine;
                }
            }
            return tableText;
        }
        #endregion

        #region References
        //recursive routine to report all missing file references
        public void GetRefs(string filePath, bool replace = false)
        {
            Debug.Print("=== " + filePath + " REFERENCES ===");
            //get references
            SwDMSearchOption searchOpt;
            object brokenRefs;
            object isVirtual;
            object timeStamp;
            object refs;
            searchOpt = DocManApp.GetSearchOptionObject();
            searchOpt.SearchFilters = (int)SwDmSearchFilters.SwDmSearchExternalReference;
            SwDMDocument21 dmDoc = (SwDMDocument21)OpenDoc(filePath, false);
            if (dmDoc == null)
            {
                return;
            }
            refs = dmDoc.GetAllExternalReferences5(searchOpt, out brokenRefs,
                out isVirtual, out timeStamp, out refs);
            string myRef;
            string[] references = (string[])refs;
            for (int i = 0; i < references.Length; i++)
            {
                myRef = references[i];
                if (!(bool)isVirtual)
                {
                    if (!File.Exists(myRef))
                    {
                        Debug.Print("MISSING: " + myRef);
                        if (replace)
                        {
                            DialogResult msgRes;
                            msgRes = MessageBox.Show(text: "Missing: \n"
                                + myRef + "\nWould you like to replace it?",
                                caption: "SWDMReferences",
                                buttons: MessageBoxButtons.YesNo, 
                                icon: MessageBoxIcon.Question);
                            if (msgRes == DialogResult.Yes)
                            {
                                string newFilePath;
                                newFilePath = BrowseForDoc(Path.GetFileName(myRef));
                                if (newFilePath != "")
                                {
                                    dmDoc.ReplaceReference(myRef, newFilePath);
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Print(myRef);
                        string ext = Path.GetExtension(myRef);
                        if (ext.ToUpper() == ".SLDASM")
                        {
                            //recursively search the assembly
                            //for its references
                            GetRefs(myRef, replace);
                        }
                    }
                }
            }
            //save if replacing references
            if (replace){dmDoc.Save();}            
            dmDoc.CloseDoc();
        }

        public string BrowseForDoc(string fileName = "")
        {
            //open the document for read/write
            string filePath = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SOLIDWORKS Files (*.sldprt, "
              + "*.sldasm, *.slddrw)"
              + "|*.sldprt;*.sldasm;*.slddrw|Parts "
              + "(*.sldprt)|*.sldprt"
              + "|Assemblies (*.sldasm)|*.sldasm|Drawings "
              + "(*.slddrw)"
              + "|*.slddrw|All files (*.*)|*.*";
            ofd.FileName = fileName;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
            }
            if (filePath == "")
            {
                return null;
            }
            return filePath;
        }
        #endregion
    }
}
