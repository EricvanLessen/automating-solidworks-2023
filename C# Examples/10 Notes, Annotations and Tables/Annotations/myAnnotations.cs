using System;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Windows.Forms;

namespace Annotations
{
    internal static class myAnnotations
    {
public static void AddFileNameNote(ModelDoc2 swDoc)
{
    bool boolstatus = false;
    int longstatus = 0;
    // 
    Note myNote = null;
    Annotation myAnnotation = null;
    TextFormat myTextFormat = null;
    DrawingDoc myDrawing = (DrawingDoc)swDoc;
    Sheet mySheet;
    double[] props;
    double width;
    double height;
    mySheet = (Sheet)myDrawing.GetCurrentSheet();
    props = (double[])mySheet.GetProperties2();
    width = props[5];  //sheet width
    height = props[6]; //sheet height
    myDrawing.EditSheet();
    myNote = ((Note)(swDoc.InsertNote("$PRPSHEET:\"SW-File Name\"")));

    if ((myNote != null))
    {
        myNote.LockPosition = false;
        myNote.Angle = Math.PI / 2;
        boolstatus = myNote.SetBalloon(0, 0);
        myAnnotation = ((Annotation)(myNote.GetAnnotation()));
        if ((myAnnotation != null))
        {
            longstatus = myAnnotation.SetLeader3((
                (int)(swLeaderStyle_e.swNO_LEADER)), 0, true, false, false, false);
            boolstatus = myAnnotation.SetPosition(width - 0.01, 0.015, 0);
            //boolstatus = myAnnotation.SetTextFormat(0, true, myTextFormat);
        }
    }
    swDoc.ClearSelection2(true);
    swDoc.WindowRedraw();
}

        public static void AddDatum(ModelDoc2 swDoc  )
        {
            //an edge must be pre-selected 
            SelectionMgr selMgr = (SelectionMgr)swDoc.SelectionManager;
            if ((int)selMgr.GetSelectedObjectCount() == 0) {
                return;
            }

            //no datum will be created otherwise
            DatumTag myDatumTag;
            myDatumTag = (DatumTag)swDoc.InsertDatumTag2();

        }

        public static void InsertGeneralTable(ModelDoc2 swDoc)
        {
            DrawingDoc swDrawing = (DrawingDoc)swDoc;
            TableAnnotation myTable = null;
            //table definition values
            //location in meters
            double tableLocX = 0.15;
            double tableLocY = 0.05;
            //if no template is used, specify the number 
            //of columns and rows
            int columns = 2;
            int rows = 2;
            bool Anchor = false;
            //place at X,Y location
            //optionally set the full path to a table template
            string tableTemplate = "";

            //insert the table
            myTable = swDrawing.InsertTableAnnotation2
              (Anchor, tableLocX, tableLocY,
              (int)swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_TopRight,           
              tableTemplate, rows, columns);

            //add table text
            myTable.set_Text2(0, 0, true, "Row 1 Column 1");
            myTable.set_Text2(0, 1, true, "Row 1 Column 2");
            myTable.set_Text2(1, 0, true, "Row 2 Column 1");
            myTable.set_Text2(1, 1, true, "$PRP:\"SW-Sheet Name(Sheet Name)\"");

            //read some table text
            MessageBox.Show(myTable.get_Text2(1, 1, true) + System.Environment.NewLine
              + myTable.get_DisplayedText2(1, 1, true));
            
        }

        public static BomTableAnnotation InsertBOMTable(ModelDoc2 swDoc  )
        {
            DrawingDoc swDrawing = null;
            swDrawing = (DrawingDoc) swDoc;

            //get the first view from the drawing
            SolidWorks.Interop.sldworks.View swActiveView = null;
            SolidWorks.Interop.sldworks.View swSheetView = null;
            swSheetView =  (SolidWorks.Interop.sldworks.View)swDrawing.GetFirstView(); //sheet
            swActiveView = (SolidWorks.Interop.sldworks.View)swSheetView.GetNextView(); //first view
            //BOM Table definition variables
            BomTableAnnotation swBOMTable = null;

            string config =
              (string)swActiveView.ReferencedConfiguration;
            string template = "C:\\Program Files\\" 
              + "SOLIDWORKS Corp\\SOLIDWORKS\\lang\\english\\bom-standard.sldbomtbt";

            //insert the table into the drawing 
            //based on the active view
            swBOMTable = swActiveView.InsertBomTable4(false, 0.2, 0.3,
              (int)swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_TopRight,
              (int)swBomType_e.swBomType_Indented, config, template,
              false, (int)swNumberingType_e.swNumberingType_Detailed, false);
            return swBOMTable;            
        }

         public static void ReadBOMTable(BomTableAnnotation swBOMTable  )
        {
            //read the cells from the table
            TableAnnotation genTable = (TableAnnotation)swBOMTable;
            int columns = genTable.ColumnCount;
            int rows = genTable.RowCount;

            for (int i = 0; i < rows; i++)
                {
                for (int j = 0; j < columns; j++)
                    {
                        System.Diagnostics.Debug.Write(genTable.get_DisplayedText2(i, j, true) + "\t");
                    }
                    System.Diagnostics.Debug.WriteLine("");
                }
                    
        } 

        public static BomTableAnnotation GetBOMTalbe(ModelDoc2 swDoc)
        {
            Feature swFeat;
            BomFeature swBOMFeat = null;

            //traverse all features
            //looking for a BOMFeature
            swFeat = (Feature)swDoc.FirstFeature();
            while (swFeat != null)
            {
                if (swFeat.GetTypeName() == "BomFeat")
                {
                    swBOMFeat = (BomFeature)swFeat.GetSpecificFeature2();
                        break;
                }
                swFeat = (Feature)swFeat.GetNextFeature();
            }

            BomTableAnnotation myBOMTable;
            TableAnnotation genTable;
            if (swBOMFeat == null)
            {
                MessageBox.Show("No BOM table found.");
                return null;
            }
            else
            {
                object[] tables = (object[])swBOMFeat.GetTableAnnotations();
                if (swBOMFeat.GetTableAnnotationCount() == 1)
                {                    
                    myBOMTable = (BomTableAnnotation)tables[0];
                    genTable = (TableAnnotation)myBOMTable;
                    System.Diagnostics.Debug.Print("First cell = " + (string)genTable.get_DisplayedText2(0, 0, true));
                }
                else
                {
                    MessageBox.Show("BOM is split. Only the first section will be returned.");
                    return (BomTableAnnotation)tables[0];
                }
            }
            return (BomTableAnnotation)myBOMTable;
        }


    }
}
