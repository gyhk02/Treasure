using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.Bll.Frame;
using Treasure.Bll.General;

namespace Treasure.Main.Frame
{
    public partial class Login : System.Web.UI.Page
    {

        #region 自定义变量

        SysUserBll bllSysUser = new SysUserBll();

        #endregion

        #region 系统事件

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #endregion

        #region 按钮

        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [WebMethod(EnableSession = true)]
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string password = txtPwd.Text.Trim();

            string userId = bllSysUser.Login(userName, password);
            if (string.IsNullOrEmpty(userId) == true)
            {
                ClientScriptManager clientScript = Page.ClientScript;
                clientScript.RegisterStartupScript(this.GetType(), "", "<script type=text/javascript>alert('用户名或密码不对');</script>");
                return;
            }

            //Session["UserId"] = userId;

            BasicWebBll.SeUserID = userId;

            Response.Redirect("Default.aspx");
        }
        #endregion

        #endregion

        #region 自定义事件
        #endregion





    }
}