using Quartz;
using System;
using System.Configuration;
using System.Data;
using System.Text;
using Treasure.BLL.Service;
using Treasure.Model.Service;
using Treasure.Utility.Helpers;

namespace Treasure.Service.ServiceEmail
{
    public class SrbFifthDecimal : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            ToExecute();
        }

        public void ToExecute()
        {
            string subjectStr = "至少有5位小数的情况";
            DateTime dToday = DateTime.Now;

            try
            {
                string pSubject = subjectStr + "(发送日期：" + dToday.ToString("yyyy.MM.dd") + ")";

                DataTable dt = new SrbFifthDecimalBll().GetSrbFifthDecimalList(ConfigurationManager.AppSettings["ConnectionString"]);
                if (dt.Rows.Count == 0)
                {
                    LogHelper.Error(subjectStr + "在" + dToday.ToString("yyyy.MM.dd") + "没有数据", System.Reflection.MethodBase.GetCurrentMethod());
                    return;
                }

                string pBodyInfo = GetBody(dt);
                BasicEmai.SendEmailDo(pSubject, pBodyInfo);
            }
            catch (Exception ex)
            {
                string errorMsg = "错误的方法：SrbFifthDecimal.Execute <br />" + ex.Message;
                BasicEmai.SendEmailByOtherError(subjectStr, errorMsg);
                LogHelper.Error(subjectStr + "异常：" + errorMsg, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

        #region 获取邮件内容
        /// <summary>
        /// 获取邮件内容
        /// </summary>
        /// <returns></returns>
        private string GetBody(DataTable dt)
        {
            StringBuilder result = new StringBuilder("");

            result.Append(@"
<style type='text/css'>
body table tr td {	font-size:12px;}
.nowrap{	white-space:nowrap;}
.centen{	text-align:center;}
.right{	text-align:right;}
.dtColor{ background-color:#CCCCCC;  }
.dtColor td{ background-color:#FFFFFF; }
</style>

SALES_REQEUST_BOM表中以下字段存在至少5位小数的情况：
<br><br>
");


            result.Append(@"
<table width='100%'  border='0' cellpadding='3' cellspacing='1'  class='dtColor'>
<tr style=' text-align:center; font-weight:bold;  '>
<td class='nowrap'>" + SrbFifthDecimalVO.FieldName + @"</td>
</tr>");

            foreach (DataRow row in dt.Rows)
            {
                result.Append(@"
<tr>
<td class='centen'>" + row[SrbFifthDecimalVO.FieldName] + @"</td>
</tr>");
            }

            result.Append("</table>");

            result.Append(@"
<br>
");

            return result.ToString();
        }
        #endregion

    }
}
