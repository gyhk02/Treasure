using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Treasure.Bll.General;
using Treasure.Utility.Helpers;
using Treasure.Utility.Utilitys;

namespace Treasure.Bll.Frame
{
    public class SysMenuItemBll : BasicBll
    {

        #region 获取非系统的项目
        /// <summary>
        /// 获取非系统的项目
        /// </summary>
        /// <returns></returns>
        public DataTable GetProjectByNoSys()
        {
            DataTable dt = null;

            string sql = "SELECT * FROM SYS_MENU_ITEM WHERE (PARENT_ID IS NULL OR PARENT_ID = '') AND IS_SYS = 0";

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

        #region 获取菜单编号
        /// <summary>
        /// 获取菜单编号
        /// </summary>
        /// <param name="pParentId"></param>
        /// <returns></returns>
        public string GetMenuItemNo(string pParentId)
        {
            string result = "";

            #region sql             

            string sqlByNull = @"
SELECT TOP 1

    LEFT(
        SUBSTRING(P.NO, 1, P.SYS_MENU_ITEM_TYPE_ID * 2)
        + RIGHT('0' + CONVERT(NVARCHAR(2), (SUBSTRING(ISNULL(C.NO, '00000000'), 1 + P.SYS_MENU_ITEM_TYPE_ID * 2, 2) + 1)), 2)
        + '00000000', 8
    ) NEXTNO
FROM(
    SELECT NULL ID, '0000000000' NO, 0 SYS_MENU_ITEM_TYPE_ID
) P
LEFT JOIN SYS_MENU_ITEM C ON ISNULL(P.ID, '') = ISNULL(C.PARENT_ID, '')
ORDER BY C.NO DESC";

            string sql = @"
SELECT TOP 1 
	LEFT(
		SUBSTRING(P.NO, 1, P.SYS_MENU_ITEM_TYPE_ID * 2) 
		+ RIGHT('0' + CONVERT(NVARCHAR(2), (SUBSTRING(ISNULL(C.NO, '00000000'), 1 + P.SYS_MENU_ITEM_TYPE_ID * 2, 2) + 1)), 2)
		+ '00000000', 8
	) NEXTNO
FROM SYS_MENU_ITEM P
LEFT JOIN SYS_MENU_ITEM C ON P.ID = C.PARENT_ID
WHERE P.ID = @PARENT_ID
ORDER BY C.NO DESC";

            #endregion

            try
            {
                string strSql = "";
                if (string.IsNullOrEmpty(TypeConversion.ToString(pParentId)) == true)
                {
                    strSql = sqlByNull;
                }
                else
                {
                    strSql = sql;
                }

                List<SqlParameter> lstPara = new List<SqlParameter>();
                lstPara.Add(new SqlParameter("@PARENT_ID", SqlDbType.NVarChar) { Value = pParentId });

                DataTable dt = base.GetDataTable(strSql, lstPara.ToArray());
                if (dt.Rows.Count > 0)
                {
                    result = TypeConversion.ToString(dt.Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return result;
        }
        #endregion

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
SELECT SMI.ID, SMI.NAME, SMI.ENGLISH_NAME, SMIT.NAME MENU_TYPE_NAME, SMI.PICTURE_URL, SMI.FILE_URL, SMI.BUTTON_NAME, SMI.ENABLE, SMI.IS_SYS, SMI.PARENT_ID
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
