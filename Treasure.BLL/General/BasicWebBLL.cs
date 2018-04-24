using System.Web;

namespace Treasure.Bll.General
{
    /// <summary>
    /// Web页基类
    /// </summary>
    public class BasicWebBll : System.Web.UI.Page
    {
        public BasicWebBll() { }

        public static string SeUserID
        {
            get
            {
                return HttpContext.Current.Session["SeUserID"] == null ? null : HttpContext.Current.Session["SeUserID"].ToString();
            }
            set
            {
                HttpContext.Current.Session["SeUserID"] = value;
            }
        }
        /// <summary>
        /// 检查用户是否登录，如果未登录就转到登录页面
        /// </summary>
        public static void CheckLogin()
        {
            if (string.IsNullOrEmpty(SeUserID) == true)
            {
                HttpContext.Current.Response.Redirect("Login.aspx");
            }
        }
    }
}
