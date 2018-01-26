using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.BLL.General;
using Treasure.BLL.SmallTool.DataSynchron;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Helpers;

namespace Treasure.Main.SmallTool.evq
{
    public partial class MianSynchron : System.Web.UI.Page
    {

        #region 自定义变量

        string priSourceConnection = "Data Source=172.16.96.48;Initial Catalog=Frame;User ID=csharp;Password=csharp.123;Persist Security Info=True;";
        string priTargetConnection = "Data Source=172.16.96.55;Initial Catalog=Frame;User ID=programmer;Password=123456;Persist Security Info=True;";
        DataSynchronBLL bllSynchron = new DataSynchronBLL();
        DataBaseBLL bllDataBase = new DataBaseBLL();
        List<string> priSourceTableList = new List<string>();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                priSourceTableList.Add("SYS_Menu");
                priSourceTableList.Add("SYS_User");
                priSourceTableList.Add("SYS_UserMenu");

                ClientScriptManager clientScript = Page.ClientScript;

                List<string> lstShow = new List<string>();

                List<string> lstTableName = JudgeSynchronData(priSourceTableList);
                if (lstTableName == null)
                {
                    clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('数据同步，判断时出现异常');</script>");
                    return;
                }

                foreach (string str in lstTableName)
                {
                    //获取要处理的数据
                    DataTable dtSource = bllSynchron.GetTableAllInfo(priSourceConnection, str);
                    DataTable dtTarget = bllSynchron.GetTableAllInfo(priTargetConnection, str);

                    List<DataRow> lstSourceData = new List<DataRow>();

                    string columnName = dtSource.Columns[0].ColumnName;
                    string columnDataType = dtSource.Columns[0].DataType.Name;

                    if (str.Equals("SYS_Menu") == true)
                    {
                        columnName = dtSource.Columns[1].ColumnName;
                        columnDataType = dtSource.Columns[1].DataType.Name;
                    }

                    switch (columnDataType)
                    {
                        case "Int32":
                            List<int> lstInt = (from d in dtTarget.AsEnumerable() select d.Field<int>(columnName)).ToList();
                            lstSourceData = dtSource.AsEnumerable().Where(p => lstInt.Contains(p.Field<int>(columnName)) == false).ToList();
                            break;
                        case "Int64":
                            List<Int64> lstInt64 = (from d in dtTarget.AsEnumerable() select d.Field<Int64>(columnName)).ToList();
                            lstSourceData = dtSource.AsEnumerable().Where(p => lstInt64.Contains(p.Field<Int64>(columnName)) == false).ToList();
                            break;
                        case "String":
                            List<string> lstString = (from d in dtTarget.AsEnumerable() select d.Field<string>(columnName)).ToList();
                            lstSourceData = dtSource.AsEnumerable().Where(p => lstString.Contains(p.Field<string>(columnName)) == false).ToList();
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
                        if (bllSynchron.InsertIncrementData(priSourceConnection, priTargetConnection, str, lstSourceData) == false)
                        {
                            clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('插入表" + str + "数据出现异常');</script>");
                        }
                    }
                }

                if (lstShow.Count > 0)
                {
                    clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('" + "以下表的数据两边相同" + ConstantVO.ENTER_RN_JS + string.Join(",", lstShow.ToArray()) + "');</script>");
                }
                else
                {
                    clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('完成');</script>");
                }
            }
        }

        #endregion

        #region 自定义事件

        #region 获取选中表对应的两个数据库的表结构
        /// <summary>
        /// 获取选中表对应的两个数据库的表结构
        /// </summary>
        /// <returns></returns>
        private DataTable GetTableStructureBy2DB()
        {
            DataTable dtResult = new DataTable();

            DataTable dtSourceTableStructure = bllDataBase.GetTableInfoByName(1, priSourceConnection, priSourceTableList);
            DataTable dtTargetTableStructure = bllDataBase.GetTableInfoByName(1, priTargetConnection, priSourceTableList);

            #region DataTable初始化

            dtResult.Columns.Add(DataSynchronVO.TableName);
            dtResult.Columns.Add(DataSynchronVO.FiledName);

            dtResult.Columns.Add(DataSynchronVO.ISIGN, typeof(string));

            dtResult.Columns.Add(DataSynchronVO.FiledType);
            dtResult.Columns.Add(DataSynchronVO.FiledLen);
            dtResult.Columns.Add(DataSynchronVO.FiledDescription);
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
                where s.Field<string>(DataSynchronVO.TableName) == t.Field<string>(DataSynchronVO.TableName) && s.Field<string>(DataSynchronVO.FiledName) == t.Field<string>(DataSynchronVO.FiledName)
                select new { s, t });

            var lstTmp = query.ToList();

            for (int idx = 0; idx < lstTmp.Count; idx++)
            {
                DataRow rowSource = lstTmp[idx].s;
                DataRow rowTarget = lstTmp[idx].t;

                DataRow row = dtResult.NewRow();

                row[DataSynchronVO.TableName] = rowSource[DataSynchronVO.TableName];
                row[DataSynchronVO.FiledName] = rowSource[DataSynchronVO.FiledName];
                row[DataSynchronVO.FiledType] = rowSource[DataSynchronVO.FiledType];
                row[DataSynchronVO.FiledLen] = rowSource[DataSynchronVO.FiledLen];
                row[DataSynchronVO.FiledDescription] = rowSource[DataSynchronVO.FiledDescription];
                row[DataSynchronVO.DecimalPrecision] = rowSource[DataSynchronVO.DecimalPrecision];
                row[DataSynchronVO.DecimalDigits] = rowSource[DataSynchronVO.DecimalDigits];
                row[DataSynchronVO.IsNullable] = rowSource[DataSynchronVO.IsNullable];
                row[DataSynchronVO.IsIdentity] = rowSource[DataSynchronVO.IsIdentity];
                row[DataSynchronVO.DefaultValue] = rowSource[DataSynchronVO.DefaultValue];

                row[DataSynchronVO.TargetFiledType] = rowTarget[DataSynchronVO.FiledType];
                row[DataSynchronVO.TargetFiledLen] = rowTarget[DataSynchronVO.FiledLen];
                row[DataSynchronVO.TargetFiledDescription] = rowTarget[DataSynchronVO.FiledDescription];
                row[DataSynchronVO.TargetDecimalPrecision] = rowTarget[DataSynchronVO.DecimalPrecision];
                row[DataSynchronVO.TargetDecimalDigits] = rowTarget[DataSynchronVO.DecimalDigits];
                row[DataSynchronVO.TargetIsNullable] = rowTarget[DataSynchronVO.IsNullable];
                row[DataSynchronVO.TargetIsIdentity] = rowTarget[DataSynchronVO.IsIdentity];
                row[DataSynchronVO.TargetDefaultValue] = rowTarget[DataSynchronVO.DefaultValue];

                if (
                    (
                    row[DataSynchronVO.FiledType].ToString() == row[DataSynchronVO.TargetFiledType].ToString()
                    && row[DataSynchronVO.FiledLen].ToString() == row[DataSynchronVO.TargetFiledLen].ToString()
                    && row[DataSynchronVO.FiledDescription].ToString() == row[DataSynchronVO.TargetFiledDescription].ToString()
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

        #region 数据同步过滤及判断
        /// <summary>
        /// 数据同步过滤及判断
        /// </summary>
        /// <returns></returns>
        public List<string> JudgeSynchronData(List<string> lstSourceTable)
        {
            List<string> lst = new List<string>();

            ClientScriptManager clientScript = Page.ClientScript;

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
                    clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('"
                        + "同步异常：以下表结构不同" + ConstantVO.ENTER_STRING + string.Join(",", lstErrorTableName.ToArray())
                        + "');</script>");
                }

                lst = lstSourceTable;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
                return null;
            }

            return lst;
        }
        #endregion

        #endregion


    }
}