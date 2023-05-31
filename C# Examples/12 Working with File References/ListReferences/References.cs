using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace ListReferences
{
    public static class References
    {
        public static SldWorks m_swApp;
        static DataTable filesTable = new DataTable();
        static string filePathColumnName = "File path";
        static string dirtyFlagColumnName = "Dirty flag";

        public static void ShowModel(string filename)
        {
            //'activate the currently selected item in the list
            ModelDoc2 swDoc;
            int errors = 0;
            swDoc = (ModelDoc2)m_swApp.ActivateDoc3(filename, true, 0, errors);
        }

public static DataTable ReadRefs()
{
    string[] fileRefs;
    string PathName;
    int UpdateStamp;
    ModelDoc2 swDoc;
    var RowData = new string[2];
    int i;
    swDoc = (ModelDoc2)m_swApp.ActiveDoc;
    PathName = swDoc.GetPathName();
    UpdateStamp = swDoc.GetUpdateStamp();

    // get an array of file references from the active document
    fileRefs = (string[])swDoc.Extension.GetDependencies(true, true, false, false, false);

    // set up the table columns    
    filesTable.Columns.Add(filePathColumnName);
    filesTable.Columns.Add(dirtyFlagColumnName);

    // add the top file name and UpdateStamp
    RowData[0] = PathName;
    RowData[1] = UpdateStamp.ToString();
    filesTable.Rows.Add(RowData);
    int loopTo = fileRefs.Length;
    for (i = 1; i <= loopTo; i += 2)
    {
        RowData[0] = fileRefs[i];
        // activate the document to get its modeldoc  
        swDoc = (ModelDoc2)m_swApp.GetOpenDocumentByName(fileRefs[i]);
        if (swDoc == null)
        {
            RowData[1] = "Not loaded";
        }
        else
        {
            UpdateStamp = swDoc.GetUpdateStamp();
            RowData[1] = UpdateStamp.ToString();
        }

        filesTable.Rows.Add(RowData);
    }

    return filesTable;
}

        public static string FilesTableToString()
        {
            string output = "";
            foreach (DataRow row in filesTable.Rows)
            {                
                output += (string)row[filePathColumnName] + System.Environment.NewLine;
            }
            return output;
        }        

    }
}
