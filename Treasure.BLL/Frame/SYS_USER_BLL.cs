using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treasure.BLL.General;
using Treasure.Model.General;
using Treasure.Utility.Helpers;

namespace Treasure.BLL.Frame
{
    public class SYS_USER_BLL : BasicBLL
    {

        #region 登录判断
        /// <summary>
        /// 登录判断
        /// 登录成功：返回用户ID
        /// 登录失败：返回空字符串
        /// </summary>
        /// <param name="pUserName"></param>
        /// <param name="pPassword"></param>
        /// <returns></returns>
        public string Login(string pUserName, string pPassword)
        {
            string result = "";

            string sql = "SELECT * FROM SYS_USER WHERE LOGIN_NAME = @LOGIN_NAME AND [PASSWORD] = @PASSWORD";

            SqlParameter[] paras = new SqlParameter[2];
            paras[0] = new SqlParameter("@LOGIN_NAME", System.Data.SqlDbType.NVarChar) { Value = pUserName };
            paras[1] = new SqlParameter("@PASSWORD", System.Data.SqlDbType.NVarChar) { Value = pPassword };

            try
            {
                DataTable dt = base.GetDataTable(sql, paras);
                if (dt.Rows.Count == 1)
                {
                    result = dt.Rows[0][GeneralVO.Id].ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return result;
        }
        #endregion



    }
}
