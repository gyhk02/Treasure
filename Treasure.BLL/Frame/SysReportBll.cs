using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Treasure.Bll.General;
using Treasure.Utility.Utilitys;

namespace Treasure.BLL.Frame
{
    public class SysReportBll : BasicBll
    {

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public DataTable Query(Dictionary<string, object> dicPara)
        {
            DataTable dt = null;

            string sql = "SELECT  A.* FROM SYS_REPORT A  WHERE 1 = 1";
            if (string.IsNullOrEmpty(TypeConversion.ToString(dicPara["NAME"])) == false)
            {
                sql = sql + " AND NAME LIKE '%' + @NAME + '%'";
            }

            List<SqlParameter> lstPara = new List<SqlParameter>();
            lstPara.Add(new SqlParameter("@NAME", SqlDbType.NVarChar) { Value = dicPara["NAME"] });

            dt = base.GetDataTable(sql, lstPara.ToArray());

            return dt;
        }
        #endregion

    }
}
