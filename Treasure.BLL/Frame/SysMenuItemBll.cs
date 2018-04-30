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

        #region 删除菜单
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="pNO"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public bool DeleteMenu(string pNO, string typeId)
        {
            bool result = false;

            try
            {
                string sql = @"
DELETE FROM A
FROM SYS_MENU_ITEM A 
WHERE NO LIKE LEFT(@NO, CONVERT(INT, @TYPE_ID) * 2 ) + '%'";

                List<SqlParameter> lstPara = new List<SqlParameter>();
                lstPara.Add(new SqlParameter("@NO", pNO));
                lstPara.Add(new SqlParameter("@TYPE_ID", typeId));

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

        #region 修改子节点编号
        /// <summary>
        /// 修改子节点编号
        /// </summary>
        /// <param name="pOldNo">旧编号</param>
        /// <param name="pNewNo">新编号</param>
        /// <param name="pTypeId">菜单类型</param>
        /// <returns></returns>
        public bool UpdateChildNo(string pOldNo, string pNewNo, string pTypeId)
        {
            bool result = false;

            try
            {
                string sql = @"
UPDATE A SET A.NO = LEFT(@NEW_NO, CONVERT(INT, @TYPE_ID) * 2) + RIGHT(A.NO, (4 - CONVERT(INT, @TYPE_ID)) * 2)
FROM SYS_MENU_ITEM A 
WHERE NO LIKE LEFT(@OLD_NO, CONVERT(INT, @TYPE_ID) * 2 ) + '%'";

                List<SqlParameter> lstPara = new List<SqlParameter>();
                lstPara.Add(new SqlParameter("@OLD_NO", pOldNo));
                lstPara.Add(new SqlParameter("@NEW_NO", pNewNo));
                lstPara.Add(new SqlParameter("@TYPE_ID", pTypeId));

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
ORDER BY SMI.NO
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

            string sql = "SELECT * FROM SYS_MENU_ITEM WHERE PARENT_ID IS NULL OR PARENT_ID = ''";

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
        public DataTable GetMenuListByUser(string pRootMenuId, string pUserId)
        {
            DataTable dt = null;

            if (string.IsNullOrEmpty(pUserId) == true)
            {
                return dt;
            }

            SysUserBll bll = new SysUserBll();
            List<string> lstSpecialUser = bll.GetSpecialUserIds();

            #region sql

            string sql = "";
            if (lstSpecialUser.Contains(pUserId))
            {
                sql = @"
WITH Sub(ID, NO, NAME, PARENT_ID,TypeName, FILE_URL ) AS 
(
	SELECT SMI.ID, SMI.NO, SMI.NAME, SMI.PARENT_ID, SMIT.NAME TypeName, SMI.FILE_URL
	FROM SYS_MENU_ITEM SMI
	JOIN SYS_MENU_ITEM_TYPE SMIT ON SMI.SYS_MENU_ITEM_TYPE_ID = SMIT.ID
	WHERE PARENT_ID = @ROOT_MENU_ID
	UNION ALL 
	SELECT SMI.ID, SMI.NO, SMI.NAME, SMI.PARENT_ID , SMIT.NAME, SMI.FILE_URL
	FROM SYS_MENU_ITEM SMI 
	JOIN SYS_MENU_ITEM_TYPE SMIT ON SMI.SYS_MENU_ITEM_TYPE_ID = SMIT.ID
	JOIN Sub S ON SMI.PARENT_ID = S.ID
)
SELECT * FROM Sub ORDER BY NO
";
            }
            else
            {
                sql = @"
DECLARE @ROOT_NO_PRE NVARCHAR(32) = ''

SELECT @ROOT_NO_PRE = SUBSTRING(NO, 1, 2) FROM SYS_MENU_ITEM WHERE ID = @ROOT_MENU_ID

SELECT SRM.SYS_MENU_ITEM_ID INTO #FIRST_MENU
FROM SYS_USER_ROLE_LIST SURL 
JOIN SYS_ROLE_MENU SRM ON SURL.SYS_ROLE_ID = SRM.SYS_ROLE_ID
WHERE SURL.SYS_USER_ID = @USER_ID
UNION
SELECT SUME.SYS_MENU_ITEM_ID
FROM SYS_USER SU
JOIN SYS_USER_MENU SUME ON SU.ID = SUME.SYS_USER_ID
WHERE SU.ID = @USER_ID

;
WITH Sub(ID, NO, NAME, PARENT_ID,TypeName, FILE_URL ) AS 
(
	SELECT SMI.ID, SMI.NO, SMI.NAME, SMI.PARENT_ID, SMIT.NAME TypeName, SMI.FILE_URL
	FROM #FIRST_MENU T
	JOIN SYS_MENU_ITEM SMI ON T.SYS_MENU_ITEM_ID = SMI.ID
	JOIN SYS_MENU_ITEM_TYPE SMIT ON SMI.SYS_MENU_ITEM_TYPE_ID = SMIT.ID

	UNION ALL 
	SELECT SMI.ID, SMI.NO, SMI.NAME, SMI.PARENT_ID , SMIT.NAME, SMI.FILE_URL
	FROM SYS_MENU_ITEM SMI 
	JOIN SYS_MENU_ITEM_TYPE SMIT ON SMI.SYS_MENU_ITEM_TYPE_ID = SMIT.ID
	JOIN Sub S ON SMI.ID = S.PARENT_ID
)
SELECT DISTINCT * FROM Sub WHERE NO LIKE @ROOT_NO_PRE + '%' ORDER BY NO

DROP TABLE #FIRST_MENU
";
            }

            #endregion

            List<SqlParameter> lstPara = new List<SqlParameter>();
            lstPara.Add(new SqlParameter("@ROOT_MENU_ID", pRootMenuId));
            lstPara.Add(new SqlParameter("@USER_ID", pUserId));
            
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
