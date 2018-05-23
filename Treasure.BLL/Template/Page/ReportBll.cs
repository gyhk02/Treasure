using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treasure.Bll.General;
using Treasure.Utility.Helpers;
using Treasure.Utility.Utilitys;

namespace Treasure.BLL.Template.Page
{
    public class ReportBll : BasicBll
    {

        #region 获取报表数据源
        /// <summary>
        /// 获取报表数据源
        /// </summary>
        /// <param name="pPageIndex">页码</param>
        /// <param name="pDic">参数集</param>
        /// <returns></returns>
        public DataTable Query( Dictionary<string, object> pDic)
        {
            DataTable dt = null;

            string sql_source = @"SELECT NAME, ID_INDEX, IS_SYS, CREATE_DATETIME FROM SYS_MENU_ITEM";
            StringBuilder sql_where = new StringBuilder();
            sql_where.Append(" WHERE 1 = 1");

            #region 参数

            List<SqlParameter> lstPara = new List<SqlParameter>();

            if (string.IsNullOrEmpty(TypeConversion.ToString(pDic["NAME"])) == false)
            {
                sql_where.Append(" AND T.NAME LIKE '%' + @NAME + '%'");
                lstPara.Add(new SqlParameter("@NAME", SqlDbType.NVarChar) { Value = pDic["NAME"] });
            }

            if (TypeConversion.ToInt(pDic["ID_INDEX"]) > 0)
            {
                sql_where.Append(" AND T.ID_INDEX = @ID_INDEX");
                lstPara.Add(new SqlParameter("@ID_INDEX", SqlDbType.Int) { Value = pDic["ID_INDEX"] });
            }

            if (TypeConversion.ToBool(pDic["IS_SYS"]) != null)
            {
                sql_where.Append(" AND T.IS_SYS = @IS_SYS");
                lstPara.Add(new SqlParameter("@IS_SYS", SqlDbType.Bit) { Value = pDic["IS_SYS"] });
            }

            if (TypeConversion.ToDateTime(pDic["CREATE_DATETIME_FROM"]) != null)
            {
                sql_where.Append(" AND T.CREATE_DATETIME > @CREATE_DATETIME_FROM");
                lstPara.Add(new SqlParameter("@CREATE_DATETIME_FROM", SqlDbType.DateTime) { Value = pDic["CREATE_DATETIME_FROM"] });
            }

            if (TypeConversion.ToDateTime(pDic["CREATE_DATETIME_TO"]) != null)
            {
                sql_where.Append(" AND T.CREATE_DATETIME_TO < @CREATE_DATETIME_TO");
                lstPara.Add(new SqlParameter("@CREATE_DATETIME_TO", SqlDbType.DateTime) { Value = pDic["CREATE_DATETIME_TO"] });
            }

            #endregion

            string sql = @"
SELECT *, IIF(T.IS_SYS = 1, '√', '') IS_SYS_STR, REPLACE(NEWID(), '-', '') T_ID 
FROM (" + sql_source + @") T " + sql_where + @"
ORDER BY ID_INDEX ";

            try
            {
                dt = base.GetDataTable(sql, lstPara.ToArray());
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return dt;
        }
        #endregion

    }
}
