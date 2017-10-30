using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Treasure.Web.Treasure
{
    public partial class SecondToDatetime : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalculation_Click(object sender, EventArgs e)
        {
            string secondsStr = txtSecond.Text.Trim();
            long seconds = long.Parse(secondsStr);
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddMilliseconds(seconds).ToLocalTime();
            txtDatetime.Text = date.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}