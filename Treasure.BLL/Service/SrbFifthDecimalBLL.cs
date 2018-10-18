using System;
using System.Data;
using Treasure.Utility.Helpers;

namespace Treasure.BLL.Service
{
    public class SrbFifthDecimalBll
    {
        #region SALES_REQUEST_BOM数字出现5位小数的情况
        /// <summary>
        /// SALES_REQUEST_BOM数字出现5位小数的情况
        /// </summary>
        /// <returns></returns>
        public DataTable GetSrbFifthDecimalList(string pConnection)
        {
            DataTable dt = null;

            try
            {
                string strsql = @"
SELECT O.name TABLE_NAME, C.name FIELD_NAME, 0 ISIGN
INTO #TMP
FROM sys.objects O 
JOIN sys.columns C ON O.object_id = C.object_id
JOIN sys.types T ON C.system_type_id = T.system_type_id AND C.user_type_id = T.user_type_id
WHERE O.type = 'U' AND T.name = 'numeric' AND C.max_length / 2 > 0
AND O.name = 'SALES_REQUEST_BOM'

DECLARE @TABLE_NAME NVARCHAR(100) = '', @FIELD_NAME NVARCHAR(100) = '', @SQL NVARCHAR(MAX) = ''

WHILE EXISTS(SELECT * FROM #TMP WHERE ISIGN = 0)
BEGIN
	SELECT @TABLE_NAME = TABLE_NAME, @FIELD_NAME = FIELD_NAME FROM #TMP WHERE ISIGN = 0

	SELECT @SQL = '
	IF EXISTS(
		SELECT COUNT(1) FROM [' + @TABLE_NAME + '] WHERE [' + @FIELD_NAME + '] <> ROUND([' + @FIELD_NAME + '], 4)
	)
	INSERT INTO #TMP(TABLE_NAME, FIELD_NAME, ISIGN)
	SELECT ''' + @TABLE_NAME + ''', ''' + @FIELD_NAME + ''', 1
	'

	EXEC(@SQL)

	DELETE FROM #TMP WHERE @TABLE_NAME = TABLE_NAME AND @FIELD_NAME = FIELD_NAME AND ISIGN = 0
END

SELECT TABLE_NAME, FIELD_NAME FROM #TMP ORDER BY TABLE_NAME, FIELD_NAME

DROP TABLE #TMP
";

                DataSet ds = SqlHelper.ExecuteDataSet(pConnection, CommandType.Text, strsql, null);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString(), ex);
            }

            return dt;
        }
        #endregion
    }
}
