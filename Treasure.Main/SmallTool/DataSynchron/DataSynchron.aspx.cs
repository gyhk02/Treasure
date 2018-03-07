using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using Treasure.BLL.General;
using Treasure.BLL.SmallTool.DataSynchron;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Extend;
using Treasure.Utility.Helpers;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.SmallTool.DataSynchron
{
    public partial class DataSynchron : System.Web.UI.Page
    {

        #region 自定义变量

        DataSynchronBLL bll = new DataSynchronBLL();
        DataBaseBll bllDataBase = new DataBaseBll();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定数据库
                DataTable dtDatabase = bllDataBase.GetDatabaseLinks();

                DropDownListExtend.BindToShowNo(ddlSourceDb, dtDatabase, false);
                DropDownListExtend.BindToShowNo(ddlTargetDb, dtDatabase, false);

                Session["dtDataDb"] = dtDatabase;

                ddlSourceDb.SelectedValue = "1";
                ddlTargetDb.SelectedValue = "3";

                ddlSourceDb_SelectedIndexChanged(sender, e);
                ddlTargetDb_SelectedIndexChanged(sender, e);

                //默认同步类型
                rblSynchronType.Text = "完全同步";

                grvTableList.KeyFieldName = DataSynchronVO.TableName;
            }
        }

        #endregion

        #region 按钮

        #region 选择源数据库
        /// <summary>
        /// 选择源数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSourceDb_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = TypeConversion.ToInt(ddlSourceDb.SelectedValue);

            DataTable dtDatabase = Session["dtDataDb"] as DataTable;
            List<DataRow> lstRow = dtDatabase.AsEnumerable().Where(p => p.Field<int>(GeneralVO.id) == id).ToList();
            if (lstRow.Count == 1)
            {
                DataRow row = lstRow[0];
                lblSourceVersion.Text = row[DataSynchronVO.Version].ToString();
                txtSourceIp.Text = row[DataSynchronVO.Ip].ToString();
                txtSourceLoginName.Text = row[DataSynchronVO.LoginName].ToString();
                txtSourcePwd.Text = row[DataSynchronVO.Pwd].ToString();
                txtSourceDbName.Text = row[DataSynchronVO.DbName].ToString();
            }
            else
            {
                lblSourceVersion.Text = "";
                txtSourceIp.Text = "";
                txtSourceLoginName.Text = "";
                txtSourcePwd.Text = "";
                txtSourceDbName.Text = "";
            }
        }
        #endregion

        #region 选择目标数据库
        /// <summary>
        /// 选择目标数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTargetDb_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = TypeConversion.ToInt(ddlTargetDb.SelectedValue);
            DataTable dtDatabase = Session["dtDataDb"] as DataTable;
            List<DataRow> lstRow = dtDatabase.AsEnumerable().Where(p => p.Field<int>(GeneralVO.id) == id).ToList();
            if (lstRow.Count == 1)
            {
                DataRow row = lstRow[0];
                lblTargetVersion.Text = row[DataSynchronVO.Version].ToString();
                txtTargetIp.Text = row[DataSynchronVO.Ip].ToString();
                txtTargetLoginName.Text = row[DataSynchronVO.LoginName].ToString();
                txtTargetPwd.Text = row[DataSynchronVO.Pwd].ToString();
                txtTargetDbName.Text = row[DataSynchronVO.DbName].ToString();
            }
            else
            {
                lblTargetVersion.Text = "";
                txtTargetIp.Text = "";
                txtTargetLoginName.Text = "";
                txtTargetPwd.Text = "";
                txtTargetDbName.Text = "";
            }
        }
        #endregion

        #region 连接数据库
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConnection_Click(object sender, EventArgs e)
        {

            lblMessage.Text = DateTime.Now.ToString(ConstantVO.DATETIME_Y_M_D_H_M_S) + ConstantVO.ENTER_BR;

            #region 检查源数据库连接是否正确

            string strIp = txtSourceIp.Text.Trim();
            string strLoginName = txtSourceLoginName.Text.Trim();
            string strPwd = txtSourcePwd.Text.Trim();
            string strDbName = txtSourceDbName.Text.Trim();

            string strSouceConnection = "Data Source=" + strIp + ";Initial Catalog=" + strDbName
                + ";User ID=" + strLoginName + ";Password=" + strPwd + ";Persist Security Info=True;";

            if (bllDataBase.JudgeConneStr(strSouceConnection) == true)
            {
                hdnSourceConnection.Value = strSouceConnection;
                lblMessage.Text = lblMessage.Text + "源据库连接成功。" + ConstantVO.ENTER_BR;
            }
            else
            {
                lblMessage.Text = lblMessage.Text + "源据库连接异常：" + ConstantVO.ENTER_BR;
                return;
            }

            #endregion

            #region 检查目标数据库连接是否正确

            string targetIp = txtTargetIp.Text.Trim();
            string targetLoginName = txtTargetLoginName.Text.Trim();
            string targetPwd = txtTargetPwd.Text.Trim();
            string targetDbName = txtTargetDbName.Text.Trim();

            string strTargetConnection = "Data Source=" + targetIp + ";Initial Catalog=" + targetDbName
                + ";User ID=" + targetLoginName + ";Password=" + targetPwd + ";Persist Security Info=True;";

            if (bllDataBase.JudgeConneStr(strTargetConnection) == true)
            {
                hdnTargetConnection.Value = strTargetConnection;
                lblMessage.Text = lblMessage.Text + "目标据库连接成功。" + ConstantVO.ENTER_BR;
            }
            else
            {
                lblMessage.Text = lblMessage.Text + "目标据库连接异常：" + ConstantVO.ENTER_BR;
                return;
            }

            #endregion

            //表
            DataTable dtTable = bllDataBase.GetTableList(strSouceConnection);
            grvTableList.DataSource = dtTable;
            grvTableList.DataBind();

            //存储过程
            DataTable dtProcedure = bll.GetProcedureOrFunctionList(strSouceConnection, 1);
            grvProcedureList.DataSource = dtProcedure;
            grvProcedureList.DataBind();

            //函数
            DataTable dtFunction = bll.GetProcedureOrFunctionList(strSouceConnection, 2);
            grvFunctionList.DataSource = dtFunction;
            grvFunctionList.DataBind();
        }
        #endregion

        #region 同步
        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSynchron_Click(object sender, EventArgs e)
        {
            ClientScriptManager clientScript = Page.ClientScript;

            if (TypeConversion.ToInt(hdnIsSubmit.Value) == 0)
            {
                return;
            }

            //获取同步类型
            string synchronType = rblSynchronType.SelectedValue;
            if (string.IsNullOrEmpty(synchronType) == true)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('请先选择同步类型');</script>");
                return;
            }

            //获取要同步的表
            List<string> lstTableList = grvTableList.GetSelectedFieldValues(new string[] { DataSynchronVO.TableName }).ConvertAll<string>(c => string.Format("{0}", c));

            //获取要同步的存储过程
            List<string> lstProcedureList = grvProcedureList.GetSelectedFieldValues(new string[] { GeneralVO.name }).ConvertAll<string>(c => string.Format("{0}", c));

            //获取要同步的函数
            List<string> lstFunctionList = grvFunctionList.GetSelectedFieldValues(new string[] { GeneralVO.name }).ConvertAll<string>(c => string.Format("{0}", c));

            try
            {
                switch (synchronType)
                {
                    case "表结构同步":
                        if (lstTableList.Count == 0)
                        {
                            clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('您还没有选择要同步的数据');</script>");
                            return;
                        }
                        SynchronTableStructure(lstTableList);
                        break;
                    case "数据完全同步":
                        if (lstTableList.Count == 0)
                        {
                            clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('您还没有选择要同步的数据');</script>");
                            return;
                        }
                        SynchronCompleteData(lstTableList);
                        break;
                    case "数据增量同步(按ID)":
                        if (lstTableList.Count == 0)
                        {
                            clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('您还没有选择要同步的数据');</script>");
                            return;
                        }
                        SynchronIncrementData(lstTableList);
                        break;
                    case "存储过程同步":
                        if (lstProcedureList.Count == 0)
                        {
                            clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('您还没有选择要同步的数据');</script>");
                            return;
                        }
                        SynchronProcedure(lstProcedureList);
                        break;
                    case "函数同步":
                        if (lstFunctionList.Count == 0)
                        {
                            clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('您还没有选择要同步的数据');</script>");
                            return;
                        }
                        SynchronFunction(lstFunctionList);
                        break;
                }

                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('同步成功');</script>");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region 表查询
        /// <summary>
        /// 表查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTableSearch_Click(object sender, EventArgs e)
        {
            string pSourceConnection = hdnSourceConnection.Value;
            string pTableName = txtTableName.Text.Trim();

            if (string.IsNullOrEmpty(pSourceConnection) == true)
            {
                ClientScriptManager clientScript = Page.ClientScript;
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('源数据库链接为空，请先点击【链接数据库】');</script>");
                return;
            }

            DataTable dt = bllDataBase.GetTableList(pSourceConnection, pTableName);
            grvTableList.DataSource = dt;
            grvTableList.DataBind();
        }
        #endregion

        #region 存储过程查询
        /// <summary>
        /// 存储过程查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcedureSearch_Click(object sender, EventArgs e)
        {
            string pSourceConnection = hdnSourceConnection.Value;
            string pProcedureName = txtProcedureName.Text.Trim();

            if (string.IsNullOrEmpty(pSourceConnection) == true)
            {
                ClientScriptManager clientScript = Page.ClientScript;
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('源数据库链接为空，请先点击【链接数据库】');</script>");
                return;
            }

            DataTable dt = bll.GetProcedureOrFunctionList(pSourceConnection, 1, pProcedureName);
            grvProcedureList.DataSource = dt;
            grvProcedureList.DataBind();
        }
        #endregion

        #region 函数查询
        /// <summary>
        /// 函数查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFunction_Click(object sender, EventArgs e)
        {
            string pSourceConnection = hdnSourceConnection.Value;
            string pFunctionName = txtFunction.Text.Trim();

            if (string.IsNullOrEmpty(pSourceConnection) == true)
            {
                ClientScriptManager clientScript = Page.ClientScript;
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('源数据库链接为空，请先点击【链接数据库】');</script>");
                return;
            }

            DataTable dt = bll.GetProcedureOrFunctionList(pSourceConnection, 2, pFunctionName);
            grvProcedureList.DataSource = dt;
            grvProcedureList.DataBind();
        }

        #endregion

        #endregion

        #region 自定义事件

        #region 函数同步
        /// <summary>
        /// 函数同步
        /// </summary>
        /// <param name="lstSourceFunction"></param>
        private void SynchronFunction(List<string> lstSourceFunction)
        {
            string pSourceConnection = hdnSourceConnection.Value;
            string pTargetConnection = hdnTargetConnection.Value;

            foreach (string str in lstSourceFunction)
            {
                bll.SynchronFunction(pSourceConnection, pTargetConnection, 2, str);
            }
        }
        #endregion

        #region 存储过程同步
        /// <summary>
        /// 存储过程同步
        /// </summary>
        /// <param name="lstSourceTable"></param>
        private void SynchronProcedure(List<string> lstSourceProcedure)
        {
            string pSourceConnection = hdnSourceConnection.Value;
            string pTargetConnection = hdnTargetConnection.Value;

            foreach (string str in lstSourceProcedure)
            {
                bll.SynchronProcedure(pSourceConnection, pTargetConnection, 1, str);
            }
        }
        #endregion

        #region 数据增量同步(按ID)
        /// <summary>
        /// 数据增量同步(按ID)
        /// </summary>
        /// <param name="lstTableList"></param>
        private void SynchronIncrementData(List<string> lstSourceTable)
        {
            ClientScriptManager clientScript = Page.ClientScript;

            List<string> lstShow = new List<string>();

            List<string> lstJudge = JudgeSynchronData(lstSourceTable);
            if (lstJudge == null)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('数据同步，判断时出现异常');</script>");
                return;
            }

            string pSourceConnection = hdnSourceConnection.Value;
            string pTargetConnection = hdnTargetConnection.Value;

            //重新排序表的顺序
            List<string> lstTable = bll.GetTableListBySort(pSourceConnection, lstJudge);

            foreach (string str in lstTable)
            {
                //获取要处理的数据
                DataTable dtSource = bll.GetTableAllInfo(pSourceConnection, str);
                DataTable dtTarget = bll.GetTableAllInfo(pTargetConnection, str);

                List<DataRow> lstSourceData;
                switch (dtSource.Columns[0].DataType.Name)
                {
                    case "Int32":
                        List<int> lstInt = (from d in dtTarget.AsEnumerable() select d.Field<int>(GeneralVO.id)).ToList();
                        lstSourceData = dtSource.AsEnumerable().Where(p => lstInt.Contains(p.Field<int>(GeneralVO.id)) == false).ToList();
                        break;
                    case "Int64":
                        List<Int64> lstInt64 = (from d in dtTarget.AsEnumerable() select d.Field<Int64>(GeneralVO.id)).ToList();
                        lstSourceData = dtSource.AsEnumerable().Where(p => lstInt64.Contains(p.Field<Int64>(GeneralVO.id)) == false).ToList();
                        break;
                    case "String":
                        List<string> lstString = (from d in dtTarget.AsEnumerable() select d.Field<string>(GeneralVO.id)).ToList();
                        lstSourceData = dtSource.AsEnumerable().Where(p => lstString.Contains(p.Field<string>(GeneralVO.id)) == false).ToList();
                        break;
                    default:
                        clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('表" + str + "的ID既然不是Int32，也不是String类型');</script>");
                        return;
                }
                if (lstSourceData.Count == 0)
                {
                    lstShow.Add(str);   //记录两边数据库数据相同的表
                }
                else
                {
                    //判断表中是否有byte类型
                    DataTable dtStructure = bllDataBase.GetTableInfoByName(1, pSourceConnection, str);
                    List<DataRow> lst = dtStructure.AsEnumerable().Where(t => t.Field<string>(DataSynchronVO.FieldType).ToLower() == "varbinary").ToList();
                    if (lst.Count > 0)
                    {
                        bll.InsertDataIncrementByByte(pSourceConnection, pTargetConnection, str, lstSourceData);
                    }
                    else if (bll.InsertIncrementData(pSourceConnection, pTargetConnection, str, lstSourceData) == false)
                    {
                        clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('插入表" + str + "数据出现异常');</script>");
                    }
                }
            }

            if (lstShow.Count > 0)
            {
                string strMsg = "以下表的数据两边相同" + ConstantVO.ENTER_STRING + string.Join(",", lstShow.ToArray());
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('" + strMsg + "');</script>");
            }
        }
        #endregion

        #region 数据同步过滤及判断
        /// <summary>
        /// 数据同步过滤及判断
        /// </summary>
        /// <returns></returns>
        public List<string> JudgeSynchronData(List<string> lstSourceTable)
        {
            List<string> lst = null;

            try
            {
                //如果表结构不同的情况，将不能同步
                DataTable dt = GetTableStructureBy2DB();
                List<DataRow> lstRow = dt.AsEnumerable().Where(p => p.Field<string>(DataSynchronVO.ISIGN) == "x").ToList();
                List<string> lstTableName = (from d in lstRow.AsEnumerable() select d.Field<string>(DataSynchronVO.TableName)).ToList();
                List<string> lstErrorTableName = new List<string>();
                foreach (string str in lstSourceTable)
                {
                    if (lstTableName.Contains(str) == true)
                    {
                        lstErrorTableName.Add(str);
                        lstSourceTable.Remove(str);
                    }
                }
                if (lstErrorTableName.Count > 0)
                {
                    lblMessage.Text = DateTime.Now.ToString(ConstantVO.DATETIME_Y_M_D_H_M_S) + ConstantVO.ENTER_STRING
                        + "同步异常：以下表结构不同" + ConstantVO.ENTER_STRING + string.Join(",", lstErrorTableName.ToArray());
                }

                lst = lstSourceTable;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return lst;
        }
        #endregion

        #region 数据完全同步
        /// <summary>
        /// 数据完全同步
        /// </summary>
        /// <param name="lstSourceTable">需要同步的表列表</param>
        private void SynchronCompleteData(List<string> lstSourceTable)
        {
            ClientScriptManager clientScript = Page.ClientScript;

            List<string> lstJudge = JudgeSynchronData(lstSourceTable);
            if (lstJudge == null)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('数据同步，判断时出现异常');</script>");
                return;
            }

            string pSourceConnection = hdnSourceConnection.Value;
            string pTargetConnection = hdnTargetConnection.Value;

            //重新排序表的顺序
            List<string> lstTable = bll.GetTableListBySort(pSourceConnection, lstJudge);

            //删除数据
            for (int idx = lstTable.Count - 1; idx >= 0; idx--)
            {
                string tableName = lstTable[idx];

                if (bll.DeleteDataTableByName(pTargetConnection, tableName) == false)
                {
                    clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('删除表" + tableName + "数据失败');</script>");
                    return;
                }
            }

            //插入数据
            foreach (string str in lstTable)
            {
                //判断表中是否有byte类型
                DataTable dtStructure = bllDataBase.GetTableInfoByName(1, pSourceConnection, str);
                List<DataRow> lst = dtStructure.AsEnumerable().Where(t => t.Field<string>(DataSynchronVO.FieldType).ToLower() == "varbinary").ToList();
                if (lst.Count > 0)
                {
                    bll.InsertDataByByte(pSourceConnection, pTargetConnection, str);
                }
                else
                {
                    if (bll.InsertData(pSourceConnection, pTargetConnection, str) == false)
                    {
                        clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('插入表" + str + "数据出现异常');</script>");
                    }
                }
            }
        }
        #endregion

        #region 表结构同步
        /// <summary>
        /// 表结构同步，方法没有写完
        /// </summary>
        /// <param name="lstSourceTable"></param>
        private void SynchronTableStructure(List<string> lstSourceTable)
        {
            ClientScriptManager clientScript = Page.ClientScript;

            //获取目标表集合
            string targetConnection = hdnTargetConnection.Value;
            DataTable dtTargetTableList = bll.Get_TableName_DescriptionType_Description(targetConnection, lstSourceTable);
            List<string> lstTargetTable = (from d in dtTargetTableList.AsEnumerable() select d.Field<string>(DataSynchronVO.TableName)).ToList();

            string pSourceConnection = hdnSourceConnection.Value;
            string pTargetConnection = hdnTargetConnection.Value;

            //目标没有的表，创建新表
            if (new CreateTableSub().CreateTable(pSourceConnection, pTargetConnection, lstSourceTable, lstTargetTable) == false)
            {
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('创建表异常');</script>");
                return;
            }

            //获取源结构
            //获取目标结构

            //目标不存在表
            //目标存在表：1.多字段；2.少字段；3.主键是否一至
        }
        #endregion

        #region 获取选中表对应的两个数据库的表结构
        /// <summary>
        /// 获取选中表对应的两个数据库的表结构
        /// </summary>
        /// <returns></returns>
        private DataTable GetTableStructureBy2DB()
        {
            DataTable dtResult = new DataTable();

            string pSourceConnection = hdnSourceConnection.Value;
            string pTargetConnection = hdnTargetConnection.Value;
            List<string> lstTableList = grvTableList.GetSelectedFieldValues(new string[] { DataSynchronVO.TableName }).ConvertAll<string>(c => string.Format("{0}", c));

            //grvTableList.Columns[0].value

            DataTable dtSourceTableStructure = bllDataBase.GetTableInfoByName(1, pSourceConnection, lstTableList);
            DataTable dtTargetTableStructure = bllDataBase.GetTableInfoByName(1, pTargetConnection, lstTableList);

            #region DataTable初始化

            dtResult.Columns.Add(DataSynchronVO.TableName);
            dtResult.Columns.Add(DataSynchronVO.FieldName);

            dtResult.Columns.Add(DataSynchronVO.ISIGN, typeof(string));

            dtResult.Columns.Add(DataSynchronVO.FieldType);
            dtResult.Columns.Add(DataSynchronVO.FiledLen);
            dtResult.Columns.Add(DataSynchronVO.FieldDescription);
            dtResult.Columns.Add(DataSynchronVO.DecimalPrecision);
            dtResult.Columns.Add(DataSynchronVO.DecimalDigits);
            dtResult.Columns.Add(DataSynchronVO.IsNullable);
            dtResult.Columns.Add(DataSynchronVO.IsIdentity);
            dtResult.Columns.Add(DataSynchronVO.DefaultValue, typeof(string));

            dtResult.Columns.Add(DataSynchronVO.TargetFiledType);
            dtResult.Columns.Add(DataSynchronVO.TargetFiledLen);
            dtResult.Columns.Add(DataSynchronVO.TargetFiledDescription);
            dtResult.Columns.Add(DataSynchronVO.TargetDecimalPrecision);
            dtResult.Columns.Add(DataSynchronVO.TargetDecimalDigits);
            dtResult.Columns.Add(DataSynchronVO.TargetIsNullable);
            dtResult.Columns.Add(DataSynchronVO.TargetIsIdentity);
            dtResult.Columns.Add(DataSynchronVO.TargetDefaultValue, typeof(string));

            #endregion

            #region 数据处理

            var query =
               (from s in dtSourceTableStructure.AsEnumerable()
                from t in dtTargetTableStructure.AsEnumerable()
                where s.Field<string>(DataSynchronVO.TableName) == t.Field<string>(DataSynchronVO.TableName) && s.Field<string>(DataSynchronVO.FieldName) == t.Field<string>(DataSynchronVO.FieldName)
                select new { s, t });

            var lstTmp = query.ToList();

            for (int idx = 0; idx < lstTmp.Count; idx++)
            {
                DataRow rowSource = lstTmp[idx].s;
                DataRow rowTarget = lstTmp[idx].t;

                DataRow row = dtResult.NewRow();

                row[DataSynchronVO.TableName] = rowSource[DataSynchronVO.TableName];
                row[DataSynchronVO.FieldName] = rowSource[DataSynchronVO.FieldName];
                row[DataSynchronVO.FieldType] = rowSource[DataSynchronVO.FieldType];
                row[DataSynchronVO.FiledLen] = rowSource[DataSynchronVO.FiledLen];
                row[DataSynchronVO.FieldDescription] = rowSource[DataSynchronVO.FieldDescription];
                row[DataSynchronVO.DecimalPrecision] = rowSource[DataSynchronVO.DecimalPrecision];
                row[DataSynchronVO.DecimalDigits] = rowSource[DataSynchronVO.DecimalDigits];
                row[DataSynchronVO.IsNullable] = rowSource[DataSynchronVO.IsNullable];
                row[DataSynchronVO.IsIdentity] = rowSource[DataSynchronVO.IsIdentity];
                row[DataSynchronVO.DefaultValue] = rowSource[DataSynchronVO.DefaultValue];

                row[DataSynchronVO.TargetFiledType] = rowTarget[DataSynchronVO.FieldType];
                row[DataSynchronVO.TargetFiledLen] = rowTarget[DataSynchronVO.FiledLen];
                row[DataSynchronVO.TargetFiledDescription] = rowTarget[DataSynchronVO.FieldDescription];
                row[DataSynchronVO.TargetDecimalPrecision] = rowTarget[DataSynchronVO.DecimalPrecision];
                row[DataSynchronVO.TargetDecimalDigits] = rowTarget[DataSynchronVO.DecimalDigits];
                row[DataSynchronVO.TargetIsNullable] = rowTarget[DataSynchronVO.IsNullable];
                row[DataSynchronVO.TargetIsIdentity] = rowTarget[DataSynchronVO.IsIdentity];
                row[DataSynchronVO.TargetDefaultValue] = rowTarget[DataSynchronVO.DefaultValue];

                if (
                    (
                    row[DataSynchronVO.FieldType].ToString() == row[DataSynchronVO.TargetFiledType].ToString()
                    && row[DataSynchronVO.FiledLen].ToString() == row[DataSynchronVO.TargetFiledLen].ToString()
                    && row[DataSynchronVO.FieldDescription].ToString() == row[DataSynchronVO.TargetFiledDescription].ToString()
                    && row[DataSynchronVO.DecimalPrecision].ToString() == row[DataSynchronVO.TargetDecimalPrecision].ToString()
                    && row[DataSynchronVO.DecimalDigits].ToString() == row[DataSynchronVO.TargetDecimalDigits].ToString()
                    && row[DataSynchronVO.IsNullable].ToString() == row[DataSynchronVO.TargetIsNullable].ToString()
                    && row[DataSynchronVO.IsIdentity].ToString() == row[DataSynchronVO.TargetIsIdentity].ToString()
                    && row[DataSynchronVO.DefaultValue].ToString() == row[DataSynchronVO.TargetDefaultValue].ToString()
                    ) == false
                    )
                {
                    row[DataSynchronVO.ISIGN] = "x";
                }

                dtResult.Rows.Add(row);
            }

            #endregion

            return dtResult;
        }
        #endregion

        #region 表结构比较
        /// <summary>
        /// 表结构比较
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCompare_Click(object sender, EventArgs e)
        {
            grvTableStructure.DataSource = GetTableStructureBy2DB();
            grvTableStructure.DataBind();
        }
        #endregion

        #endregion
    }
}