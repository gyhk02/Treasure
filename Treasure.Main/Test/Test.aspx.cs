using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.BLL.Frame;
using Treasure.Model.General;

namespace Treasure.Main.Test
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        void Bind()
        {
            DataTable dtMenuItem = new SYS_MENU_ITEM_BLL().GetMenuItemList();
            ASPxTreeList1.DataSource = dtMenuItem;
            ASPxTreeList1.DataBind();
        }

        protected void ASPxTreeList1_NodeDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
        
        }

        protected void ASPxTreeList1_NodeInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
        
        }

        protected void ASPxTreeList1_NodeUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
         
        }

        protected void ASPxTreeList1_NodeValidating(object sender, DevExpress.Web.ASPxTreeList.TreeListNodeValidationEventArgs e)
        {
            
        }



        protected void ASPxTreeList1_InitNewNode(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {

        }

        protected void ASPxTreeList1_HtmlRowPrepared(object sender, DevExpress.Web.ASPxTreeList.TreeListHtmlRowEventArgs e)
        {
            if (e.RowKind == DevExpress.Web.ASPxTreeList.TreeListRowKind.EditForm)
            {
                Session["status"] = "edit";
            }

        }


        protected void ASPxTreeList1_CellEditorInitialize(object sender, DevExpress.Web.ASPxTreeList.TreeListColumnEditorEventArgs e)
        {
            //if (e.Column.FieldName == "fatherID")
            //{
            //    //if (Session["status"] != null && Session["status"].ToString() == "edit")
            //    //{

            //        Model.Rsb_Departments model = (Model.Rsb_Departments)ASPxTreeList1.FindNodeByKeyValue(e.NodeKey).DataItem;
            //        ASPxTextBox cboYear = e.Editor as DevExpress.Web.ASPxEditors.ASPxTextBox;
            //        cboYear.Text = model.DeptID.ToString();
            //    //}
            //}

        }

        protected void ASPxTreeList1_FocusedNodeChanged(object sender, EventArgs e)
        {
            string a = "";
        }
    }
}