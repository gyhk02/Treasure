using System;
using System.Data;
using System.Data.SqlClient;
using Treasure.BLL.General;
using Treasure.Utility.Helpers;

namespace Treasure.BLL.Frame
{
    public class SysMenuItemBll : BasicBll
    {
        #region 获取树结构的菜单列表
        /// <summary>
        /// 获取树结构的菜单列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetMenuItemByTree()
        {
            DataTable dt = null;

            string sql = "SELECT ID, REPLACE(RIGHT('000000000000', (SYS_MENU_ITEM_TYPE_ID - 1) * 2), '0', '..') + NAME NAME FROM SYS_MENU_ITEM ORDER BY NO";

            try
            {
                dt = base.GetDataTable(sql, null);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return dt;
        }
        #endregion

        #region 获取菜单列表
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetMenuItemList()
        {
            DataTable dt = null;

            string sql = @"
SELECT SMI.ID, SMI.NAME, SMIT.NAME MENU_TYPE_NAME, SMI.PICTURE_URL, SMI.FILE_URL, SMI.BUTTON_NAME, SMI.ENABLE, SMI.IS_SYS, SMI.PARENT_ID
FROM SYS_MENU_ITEM SMI
JOIN SYS_MENU_ITEM_TYPE SMIT ON SMI.SYS_MENU_ITEM_TYPE_ID = SMIT.ID
";

            try
            {
                dt = base.GetDataTable(sql, null);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return dt;
        }
        #endregion

        #region 获取根菜单
        /// <summary>
        /// 获取根菜单
        /// </summary>
        /// <returns></returns>
        public DataTable GetRootMenu()
        {
            DataTable dt = null;

            string sql = "SELECT * FROM SYS_MENU_ITEM WHERE PARENT_ID IS NULL";

            try
            {
                dt = base.GetDataTable(sql, null);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return dt;
        }
        #endregion
        
        #region 获取菜单列表
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="pProjectId">项目id</param>
        /// <returns></returns>
        public DataTable GetFunctionsAndPagesMenu(string pProjectId)
        {
            DataTable dt = null;

            string sql = @"
WITH Sub(ID, NAME, PARENT_ID,TypeName, FILE_URL ) AS 
(
	SELECT SMI.ID, SMI.NAME, SMI.PARENT_ID, SMIT.NAME TypeName, SMI.FILE_URL
	FROM SYS_MENU_ITEM SMI
	JOIN SYS_MENU_ITEM_TYPE SMIT ON SMI.SYS_MENU_ITEM_TYPE_ID = SMIT.ID
	WHERE PARENT_ID = @PARENT_ID
	UNION ALL 
	SELECT SMI.ID, SMI.NAME, SMI.PARENT_ID , SMIT.NAME, SMI.FILE_URL
	FROM SYS_MENU_ITEM SMI 
	JOIN SYS_MENU_ITEM_TYPE SMIT ON SMI.SYS_MENU_ITEM_TYPE_ID = SMIT.ID
	JOIN Sub S ON SMI.PARENT_ID = S.ID
)
SELECT * FROM Sub
";
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter("@PARENT_ID", pProjectId);

            try
            {
                dt = base.GetDataTable(sql, paras);
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
