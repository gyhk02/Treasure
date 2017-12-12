using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Treasure.BLL.Frame;
using Treasure.BLL.General;

namespace Treasure.Main.Frame
{
    public partial class Login : BasicWebBLL
    {

        #region 自定义变量

        SYS_USER_BLL bllSysUser = new SYS_USER_BLL();

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
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string password = txtPwd.Text.Trim();

            string userId = bllSysUser.Login(userName, password);
            if (string.IsNullOrEmpty(userId) == true)
            {
                MessageBox.Show("用户名或密码不对");
                return;
            }

            base.Session["UserId"] = userId;

            Response.Redirect("");
        }
        #endregion

        #endregion

        #region 自定义事件
        #endregion





    }
}