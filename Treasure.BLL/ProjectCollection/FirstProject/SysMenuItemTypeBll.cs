using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Treasure.Bll.General;

namespace Treasure.Bll.ProjectCollection.FirstProject
{
    public class SysMenuItemTypeBll : BasicBll
    {

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public DataTable Query(Dictionary<string, object> dicPara)
        {
            DataTable dt = null;

            string sql = "SELECT * FROM SYS_MENU_ITEM_TYPE WHERE NO LIKE '%' + @NO + '%' AND NAME LIKE '%' + @NAME + '%'";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            lstPara.Add(new SqlParameter("@NO", SqlDbType.NVarChar) { Value = dicPara["NO"] });
            lstPara.Add(new SqlParameter("@NAME", SqlDbType.NVarChar) { Value = dicPara["NAME"] });

            dt = base.GetDataTable(sql, lstPara.ToArray());

            return dt;
        }
        #endregion

    }
}
