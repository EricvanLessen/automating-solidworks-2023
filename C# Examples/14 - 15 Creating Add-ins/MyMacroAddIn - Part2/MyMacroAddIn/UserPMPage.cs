using System;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorks.Interop.swconst;

namespace MyMacroAddIn
{
    public class UserPMPage
    {
        //Local Objects
        public IPropertyManagerPage2 swPropertyPage = null;
        PMPHandler handler = null;
        ISldWorks iSwApp = null;
        SwAddin userAddin = null;

        #region Property Manager Page Controls
        //Groups
        IPropertyManagerPageGroup group2;

        //Controls
        public IPropertyManagerPageTextbox textbox1;
        IPropertyManagerPageSelectionbox selection1;
        IPropertyManagerPageButton button1;

        //Control IDs
        public const int group2ID = 1;

        public const int textbox1ID = 2;
        public const int selection1ID = 8;
        public const int buttonID1 = 13;

        #endregion

        public UserPMPage(SwAddin addin)
        {
            userAddin = addin;
            if (userAddin != null)
            {
                iSwApp = (ISldWorks)userAddin.SwApp;
                CreatePropertyManagerPage();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("SwAddin not set.");
            }
        }


        protected void CreatePropertyManagerPage()
        {
            int errors = -1;
            int options = (int)swPropertyManagerPageOptions_e.swPropertyManagerOptions_OkayButton |
                (int)swPropertyManagerPageOptions_e.swPropertyManagerOptions_CancelButton;

            handler = new PMPHandler(userAddin, this);
            swPropertyPage = (IPropertyManagerPage2)iSwApp.CreatePropertyManagerPage("Add Component", options, handler, ref errors);
            if (swPropertyPage != null && errors == (int)swPropertyManagerPageStatus_e.swPropertyManagerPage_Okay)
            {
                try
                {
                    AddControls();
                }
                catch (Exception e)
                {
                    iSwApp.SendMsgToUser2(e.Message, 0, 0);
                }
            }
        }


        //Controls are displayed on the page top to bottom in the order 
        //in which they are added to the object.
        protected void AddControls()
        {
            short controlType = -1;
            short align = -1;
            int options = -1;
            bool retval;

            //Add Message
            string message = "Select one or more circular edges where the component will be mated. "
                + "Browse to a fastener to insert, then click OK.";
;
            retval = swPropertyPage.SetMessage3(message,
                                            (int)swPropertyManagerPageMessageVisibility.swImportantMessageBox,
                                            (int)swPropertyManagerPageMessageExpanded.swMessageBoxExpand,
                                            "Message");

            //Add the groups
            options = (int)swAddGroupBoxOptions_e.swGroupBoxOptions_Expanded | 
                      (int)swAddGroupBoxOptions_e.swGroupBoxOptions_Visible;

            group2 = (IPropertyManagerPageGroup)swPropertyPage.AddGroupBox(group2ID, "Add Component", options);

            //Add the controls to group1

            //Add controls to group2
            //selection1
            controlType = (int)swPropertyManagerPageControlType_e.swControlType_Selectionbox;
            align = (int)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;
            options = (int)swAddControlOptions_e.swControlOptions_Enabled |
                      (int)swAddControlOptions_e.swControlOptions_Visible;

            selection1 = (IPropertyManagerPageSelectionbox)group2.AddControl(selection1ID, controlType, "Select Circular Edges", 
                align, options, "Select circular edges on flat faces where components will be added");
            if (selection1 != null)
            {
                int[] filter = { (int)swSelectType_e.swSelEDGES };
                selection1.Height = 50;
                selection1.SetSelectionFilters(filter);
            }

            // Button
            controlType = (int)swPropertyManagerPageControlType_e.swControlType_Button;
            align = (int)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;
            options = (int)swAddControlOptions_e.swControlOptions_Enabled |
                    (int)swAddControlOptions_e.swControlOptions_Visible;

            button1 = (IPropertyManagerPageButton)group2.AddControl2(buttonID1, controlType, "Browse...", 
                align, options, "Browse to a part");

            //label
            group2.AddControl(100, 1, "File path", 1, 3, "");

            //textbox1
            controlType = (int)swPropertyManagerPageControlType_e.swControlType_Textbox;
            align = (int)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;
            options = (int)swAddControlOptions_e.swControlOptions_Enabled |
                      (int)swAddControlOptions_e.swControlOptions_Visible;

            textbox1 = (IPropertyManagerPageTextbox)group2.AddControl(textbox1ID, controlType, "File path", 
                align, options, "File path");

        }

        public void Show()
        {
            if (swPropertyPage != null)
            {
                swPropertyPage.Show();
            }
        }
    }
}
