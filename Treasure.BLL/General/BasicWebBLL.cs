using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Treasure.Bll.General
{
    /// <summary>
    /// Web页基类
    /// </summary>
    public class BasicWebBLL : System.Web.UI.Page
    {
        public BasicWebBLL()
        {
            if (HttpContext.Current.Session == null || string.IsNullOrEmpty(HttpContext.Current.Session["UserId"].ToString()) == true)
            {
                Response.Redirect("Login.aspx");
            }
        }
    }
}
