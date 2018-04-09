using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.Bll.Frame;
using Treasure.Model.General;

namespace Treasure.Main.Test
{
    public partial class Test : System.Web.UI.Page
    {
        //Dal.Rsb_Departments dal = new 考勤系统.Dal.Rsb_Departments();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack == false)
            //{
                Bind();
            //}
        }
        void Bind()
        {
            //System.Collections.Generic.List<Model.Rsb_Departments> list = dal.getList();
            //if (list.Count == 0)
            //{
            //    list.Add(new 考勤系统.Model.Rsb_Departments());
            //}


            string DeptID = "DeptID";
            string DeptName = "DeptName";
            string DeptNum = "DeptNum";
            string fatherID = "fatherID";

            DataTable dt = new DataTable();
            dt.Columns.Add(DeptID);
            dt.Columns.Add(DeptName);
            dt.Columns.Add(DeptNum);
            dt.Columns.Add(fatherID);

            DataRow row2 = dt.NewRow();
            row2[DeptID] = 2;
            row2[DeptName] = "DeptName_2";
            row2[DeptNum] = "DeptNum_2";
            row2[fatherID] = 1;
            dt.Rows.Add(row2);

            DataRow row1 = dt.NewRow();
            row1[DeptID] = 1;
            row1[DeptName] = "DeptName_1";
            row1[DeptNum] = "DeptNum_1";
            row1[fatherID] = 0;
            dt.Rows.Add(row1);

            ASPxTreeList1.DataSource = dt;
            ASPxTreeList1.DataBind();
        }

        protected void ASPxTreeList1_NodeDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            //int DeptID = Convert.ToInt32(e.Keys["DeptID"]);
            //dal.Delete(DeptID);
            ASPxTreeList1.CancelEdit();
            e.Cancel = true;
            Bind();
        }

        protected void ASPxTreeList1_NodeInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            //Model.Rsb_Departments model = new 考勤系统.Model.Rsb_Departments();
            //model.DeptName = e.NewValues["DeptName"].ToString();
            //model.DeptNum = e.NewValues["DeptNum"].ToString();
            //model.fatherID = Convert.ToInt32(e.NewValues["fatherID"]);
            //dal.Create(model);
            ASPxTreeList1.CancelEdit();
            e.Cancel = true;
            Bind();
        }

        protected void ASPxTreeList1_NodeUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //Model.Rsb_Departments model = new 考勤系统.Model.Rsb_Departments();
            //model.DeptName = e.NewValues["DeptName"].ToString();
            //model.DeptNum = e.NewValues["DeptNum"].ToString();
            //model.fatherID = Convert.ToInt32(e.NewValues["fatherID"]);
            //model.DeptID = Convert.ToInt32(e.Keys["DeptID"]);
            //dal.Update(model);
            ASPxTreeList1.CancelEdit();
            e.Cancel = true;
            Bind();
        }

        protected void ASPxTreeList1_NodeValidating(object sender, DevExpress.Web.ASPxTreeList.TreeListNodeValidationEventArgs e)
        {
            if (e.NewValues["DeptNum"] == null)
            {
                e.NodeError = "部门编号非空";
                return;
            }
            if (e.NewValues["DeptName"] == null)
            {
                e.NodeError = ("部门名称非空");
                return;
            }
            if (e.NewValues["fatherID"] == null)
            {
                e.NodeError = ("父id非空");
                return;
            }
            else
            {
                //if (!Common.Common.isMatchNum(e.NewValues["fatherID"].ToString()))
                //{
                //    e.NodeError = "要求为数字";
                //    return;
                //}
            }
            e.NewValues["fatherID"] = 33;


        }



        protected void ASPxTreeList1_InitNewNode(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            //Model.Rsb_Departments model = (Model.Rsb_Departments)ASPxTreeList1.FocusedNode.DataItem;
            //e.NewValues["fatherID"] = model.DeptID;

            string a = "";
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
            string a = "";

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