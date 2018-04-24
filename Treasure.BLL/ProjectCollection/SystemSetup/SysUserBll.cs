
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Treasure.Bll.General;
using Treasure.Utility.Utilitys;
using Treasure.Model.General;
using Treasure.Utility.Utilitys;

namespace Treasure.Bll.ProjectCollection.SystemSetup
{
    public class SysUserBll : BasicBll
    {

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public DataTable Query(Dictionary<string, object> dicPara)
        {
            DataTable dt = null;

            string sql = "SELECT  A.* FROM SYS_USER A  WHERE 1 = 1";
            if (string.IsNullOrEmpty(TypeConversion.ToString(dicPara["NO"])) == false)
            {
                sql = sql + " AND NO LIKE '%' + @NO + '%'";
            }
            if (string.IsNullOrEmpty(TypeConversion.ToString(dicPara["NAME"])) == false)
            {
                sql = sql + " AND NAME LIKE '%' + @NAME + '%'";
            }
            if (string.IsNullOrEmpty(TypeConversion.ToString(dicPara["LOGIN_NAME"])) == false)
            {
                sql = sql + " AND LOGIN_NAME LIKE '%' + @LOGIN_NAME + '%'";
            }

            List<SqlParameter> lstPara = new List<SqlParameter>();
            lstPara.Add(new SqlParameter("@NO", SqlDbType.NVarChar) { Value = dicPara["NO"] });            lstPara.Add(new SqlParameter("@NAME", SqlDbType.NVarChar) { Value = dicPara["NAME"] });            lstPara.Add(new SqlParameter("@LOGIN_NAME", SqlDbType.NVarChar) { Value = dicPara["LOGIN_NAME"] });

            dt = base.GetDataTable(sql, lstPara.ToArray());

            return dt;
        }
        #endregion

    }
}
