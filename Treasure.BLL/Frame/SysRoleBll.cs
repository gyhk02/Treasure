using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treasure.Bll.General;
using Treasure.Utility.Helpers;

namespace Treasure.BLL.Frame
{
    public class SysRoleBll : BasicBll
    {

        #region 根据角色ID删除对应的菜单
        /// <summary>
        /// 根据角色ID删除对应的菜单
        /// </summary>
        /// <param name="pRoleId">用户ID</param>
        /// <returns></returns>
        public bool DeleteMenuListByRoleId(string pRoleId)
        {
            bool result = false;

            string sql = @"DELETE FROM SYS_ROLE_MENU WHERE SYS_ROLE_ID = @SYS_ROLE_ID";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            lstPara.Add(new SqlParameter("@SYS_ROLE_ID", pRoleId));

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

        #region 根据角色ID获取菜单列表
        /// <summary>
        /// 根据角色ID获取菜单列表
        /// </summary>
        /// <param name="pRoleId"></param>
        /// <returns></returns>
        public DataTable GetMenuListByRoleId(string pRoleId)
        {
            DataTable dt = null;

            string sql = @"
SELECT SMI.ID, SMI.NAME, SMIT.NAME MENU_TYPE_NAME, SMI.PARENT_ID, IIF(SRM.ID IS NULL, 0, 1) IS_SELECTED
FROM SYS_MENU_ITEM SMI
JOIN SYS_MENU_ITEM_TYPE SMIT ON SMI.SYS_MENU_ITEM_TYPE_ID = SMIT.ID
LEFT JOIN SYS_ROLE_MENU SRM ON SMI.ID = SRM.SYS_MENU_ITEM_ID AND SRM.SYS_ROLE_ID = @SYS_USER_ID
ORDER BY SMI.NO";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            lstPara.Add(new SqlParameter("@SYS_USER_ID", pRoleId));

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
        
        #region 根据角色ID删除对应的用户
        /// <summary>
        /// 根据角色ID删除对应的用户
        /// </summary>
        /// <param name="pRoleId">角色ID</param>
        /// <returns></returns>
        public bool DeleteUserListByRoleId(string pRoleId)
        {
            bool result = false;

            string sql = @"DELETE FROM SYS_USER_ROLE_LIST WHERE SYS_ROLE_ID = @SYS_ROLE_ID";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            lstPara.Add(new SqlParameter("@SYS_ROLE_ID", pRoleId));

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

        #region 根据角色ID获取用户列表
        /// <summary>
        /// 根据角色ID获取用户列表
        /// </summary>
        /// <param name="pRoleId"></param>
        /// <returns></returns>
        public DataTable GetUserListByRoleId(string pRoleId)
        {
            DataTable dt = null;

            string sql = @"
SELECT U.ID, U.NO, U.NAME, U.LOGIN_NAME, IIF(URL.ID IS NULL, 0 ,1) IS_SEFT
FROM SYS_USER U
LEFT JOIN SYS_USER_ROLE_LIST URL ON U.ID = URL.SYS_USER_ID AND URL.SYS_ROLE_ID = @SYS_ROLE_ID
ORDER BY U.ID_INDEX";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            lstPara.Add(new SqlParameter("@SYS_ROLE_ID", pRoleId));

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
