using System;
using System.Data;
using System.Data.SqlClient;
using Treasure.Bll.General;
using Treasure.Model.General;
using Treasure.Utility.Helpers;

namespace Treasure.Bll.Frame
{
    public class SysUserBll : BasicBll
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
                    result = dt.Rows[0][GeneralVO.id].ToString();
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
