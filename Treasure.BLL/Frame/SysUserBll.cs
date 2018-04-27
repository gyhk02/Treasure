using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Treasure.Bll.General;
using Treasure.Model.General;
using Treasure.Utility.Helpers;

namespace Treasure.Bll.Frame
{
    public class SysUserBll : BasicBll
    {
        
        #region 根据用户ID删除对应的菜单
        /// <summary>
        /// 根据用户ID删除对应的菜单
        /// </summary>
        /// <param name="pUserId">用户ID</param>
        /// <returns></returns>
        public bool DeleteMenuListByUserId(string pUserId)
        {
            bool result = false;

            string sql = @"DELETE FROM SYS_USER_MENU WHERE SYS_USER_ID = @SYS_USER_ID";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            lstPara.Add(new SqlParameter("@SYS_USER_ID", pUserId));

            try
            {
                base.ExecuteNonQuery(CommandType.Text, sql, lstPara.ToArray());

                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return result;
        }
        #endregion

        #region 根据用户ID获取菜单列表
        /// <summary>
        /// 根据用户ID获取菜单列表
        /// </summary>
        /// <param name="pUserId"></param>
        /// <returns></returns>
        public DataTable GetMenuListByUserId(string pUserId)
        {
            DataTable dt = null;

            string sql = @"
SELECT SMI.ID, SMI.NAME, SMIY.NAME, IIF(SU.ID IS NULL, 0 ,1) IS_SELECTED
FROM SYS_MENU_ITEM SMI
JOIN SYS_MENU_ITEM_TYPE SMIY ON SMI.SYS_MENU_ITEM_TYPE_ID = SMIY.ID
LEFT JOIN SYS_USER_MENU SU ON SMI.ID = SU.SYS_MENU_ITEM_ID AND SU.SYS_USER_ID = @SYS_USER_ID
ORDER BY SMI.NO";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            lstPara.Add(new SqlParameter("@SYS_USER_ID", pUserId));

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
