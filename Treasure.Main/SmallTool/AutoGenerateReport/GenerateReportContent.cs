using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Treasure.Model.Frame;
using Treasure.Model.General;
using Treasure.Model.SmallTool.DataSynchron;
using Treasure.Utility.Utilitys;

namespace Treasure.Main.SmallTool.AutoGenerateReport
{
    public class GenerateReportContent
    {

        #region Bll文件
        /// <summary>
        /// Bll文件
        /// </summary>
        /// <returns></returns>
        public static string GetCreateBllFileContent(
            string pTableName, List<object> lstQueryField, string pProjectNamespace, string pClassName
            , string pSolutionName, DataTable pDtAll)
        {
            string result = "";

            #region 方法中的HTML

            //条件HTML
            StringBuilder sqlHtml = new StringBuilder();

            //参数HTML
            StringBuilder paraHtml = new StringBuilder();

            sqlHtml.Append("            string sql = \"SELECT");
            foreach (DataRow row in pDtAll.Rows)
            {
                string fieldName = TypeConversion.ToString(row[DataSynchronVO.FieldName]);
                string fieldType = TypeConversion.ToString(row[DataSynchronVO.FieldType]);
                Dictionary<string, string> pDic = new Dictionary<string, string>();
                pDic.Add("FieldName", fieldName);

                sqlHtml.Append(GenerateReportForDataType.GetString(fieldType, "GetCreateBllFileContent_BoolStr", pDic));
            }
            sqlHtml.Append(" * FROM " + pTableName + " WHERE 1 = 1\";");
            foreach (object[] arrQueryField in lstQueryField)
            {
                string fieldName = TypeConversion.ToString(arrQueryField[0]);
                string fieldType = TypeConversion.ToString(arrQueryField[1]);

                Dictionary<string, string> pDic = new Dictionary<string, string>();
                pDic.Add("FieldName", fieldName);

                sqlHtml.Append(GenerateReportForDataType.GetString(fieldType, "GetCreateBllFileContent_Where", pDic));

                paraHtml.Append("            "
                    + GenerateReportForDataType.GetString(fieldType, "GetCreateBllFileContent_Para", pDic)
                    + ConstantVO.ENTER_R);
            }

            #endregion

            #region 内容

            result = @"
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using " + pSolutionName + @".Bll.General;
using " + pSolutionName + @".Utility.Utilitys;
using " + pSolutionName + @".Model.General;
using " + pSolutionName + @".Utility.Utilitys;

namespace " + pProjectNamespace + @"
{
    public class " + pClassName + @"Bll : BasicBll
    {

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public DataTable Query(Dictionary<string, object> dicPara)
        {
            DataTable dt = null;

" + sqlHtml.ToString() + @"

            List<SqlParameter> lstPara = new List<SqlParameter>();
" + paraHtml.ToString() + @"

            dt = base.GetDataTable(sql, lstPara.ToArray());

            return dt;
        }
        #endregion

    }
}
";

            #endregion

            return result;
        }
        #endregion

        #region Model文件
        /// <summary>
        /// Model文件
        /// </summary>
        /// <param name="projectNamespace"></param>
        /// <param name="pClassName"></param>
        /// <param name="fieldTable"></param>
        /// <returns></returns>
        public static string GetCreateModelFileContent(string projectNamespace, string pClassName, DataTable fieldTable)
        {
            string result = "";

            #region 字段Html

            StringBuilder fieldHtml = new StringBuilder();
            foreach (DataRow row in fieldTable.Rows)
            {
                fieldHtml.Append("            public readonly static string "
                    + CamelName.getSmallCamelName(TypeConversion.ToString(row[SysReportColTable.Fields.name])) + " = \""
                    + TypeConversion.ToString(row[SysReportColTable.Fields.name]) + "\"; " + ConstantVO.ENTER_R);
            }

            #endregion

            #region 内容

            result = @"
namespace " + projectNamespace + @"
{
    public class " + pClassName + @"Vo
    {
        public static class Fields
        {
" + fieldHtml.ToString() + @"
        }
    }
}";

            #endregion

            return result;
        }
        #endregion

    }
}