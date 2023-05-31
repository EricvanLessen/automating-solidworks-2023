using System;
using System.Collections.Generic;
using System.Diagnostics;


using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using EPDM.Interop.epdm;


namespace PDMBOM
{
    public partial class SolidWorksMacro
    {
        public void Main()
        {

            EdmVault5 vault = new EdmVault5();
            vault.LoginAuto("PDMVault", 0);

            //Let the user select a SOLIDWORKS file
            EdmStrLst5 PathList;
            PathList = vault.BrowseForFile(0, bsCaption: "Select a file to show its information");

            //check if the user pressed cancel
            if (PathList == null)
            {
                //do nothing, user cancelled
            }
            else
            {
                IEdmPos5 pos;
                pos = PathList.GetHeadPosition();
                while (!pos.IsNull)
                {
                    string filePath;
                    filePath = PathList.GetNext(pos);
                    //connect to the file object
                    IEdmFile7 eFile;
                    IEdmFolder5 eFolder;
                    eFile = (IEdmFile7)vault.GetFileFromPath(filePath, out eFolder);

                    //get a specific BOM view
                    IEdmBomView3 bom;
                    bom = (IEdmBomView3)eFile.GetComputedBOM("Basic BOM",
                      0, "@", (int)EdmBomFlag.EdmBf_AsBuilt);

                    //get column headers
                    EdmBomColumn[] columns;
                    bom.GetColumns(out columns);
                    string header = "LEVEL\t";
                    foreach (EdmBomColumn column in columns)
                    {
                        header += column.mbsCaption + "\t";
                    }
                    Debug.Print(header);

                    //read each BOM row
                    object[] rows;
                    bom.GetRows(out rows);
                    foreach (IEdmBomCell row in rows)
                    {
                        if (row == null) { break; }
                        string rowString = row.GetTreeLevel().ToString() + "\t";
                        object varVal = "";
                        foreach (EdmBomColumn column in columns)
                        {
                            object compVal;
                            string config;
                            bool readOnly;
                            row.GetVar(column.mlVariableID, column.meType,
                              out varVal, out compVal, out config, out readOnly);
                            if (varVal == null) { varVal = ""; }
                            rowString += varVal + "\t";
                        }
                        //print the row
                        Debug.Print(rowString);
                    }

                }
            }
        }

        // The SldWorks swApp variable is pre-assigned for you.
        public SldWorks swApp;

    }
}

