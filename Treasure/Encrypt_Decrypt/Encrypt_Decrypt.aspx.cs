using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Treasure.Web.Extend;
//using Treasure.Utils.Web.EnumUtils;

namespace Treasure.Web.Treasure.Encrypt_Decrypt
{
    public partial class Encrypt_Decrypt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EnumHelper.BindToEnum<Encrypt_Decrypt_Type>(ddlType);
            }
        }

        #region 加密
        protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtKey.Text.Trim() == "")
                {
                    Response.Write("<script>alert('key不能为空')</script> ");
                    txtKey.Focus();
                    return;
                }
                if (txtTop.Text.Trim() == "")
                {
                    Response.Write("<script>alert('原字符串不能为空')</script> ");
                    txtTop.Focus();
                    return;
                }

                txtBottom.Text = "";
                switch (Int32.Parse(ddlType.SelectedValue))
                {
                    case 1:     //子项目
                    case 4:     //CF
                        txtBottom.Text = ChildProjDEncrypt.Encrypt(txtTop.Text.Trim(), txtKey.Text.Trim());
                        break;
                    case 2:     //main项目
                    case 3:     //AX ERP
                    case 5:     //RFID
                        txtBottom.Text = MainAndAXDESCrypt.Encrypt(txtTop.Text.Trim(), txtKey.Text.Trim());
                        break;
                    case 6:
                        txtBottom.Text = EncryptHelper.EncryptString(txtTop.Text.Trim(), txtKey.Text.Trim());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "')</script> ");
            }
        }
        #endregion

        #region 解密
        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtKey.Text.Trim() == "")
                {
                    Response.Write("<script>alert('key不能为空')</script> ");
                    txtKey.Focus();
                    return;
                }
                if (txtTop.Text.Trim() == "")
                {
                    Response.Write("<script>alert('原字符串不能为空')</script> ");
                    txtTop.Focus();
                    return;
                }

                txtBottom.Text = "";
                switch (Int32.Parse(ddlType.SelectedValue))
                {
                    case 1:     //子项目
                    case 4:     //CF
                        txtBottom.Text = ChildProjDEncrypt.Decrypt(txtTop.Text.Trim(), txtKey.Text.Trim());
                        break;
                    case 2:     //main项目   
                    case 3:     //AX ERP
                    case 5:     //RFID
                        txtBottom.Text = MainAndAXDESCrypt.Decrypt(txtTop.Text.Trim(), txtKey.Text.Trim());
                        break;
                    case 6:     //JIT Weight
                        txtBottom.Text = EncryptHelper.DecryptString(txtTop.Text.Trim(), txtKey.Text.Trim());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "')</script> ");
            }
        }
        #endregion

        #region 选择 加密|解密 类型
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iType = Int32.Parse(ddlType.SelectedValue);
            switch (iType)
            {
                case 1:
                    txtKey.Text = "MATICSOFT";
                    break;
                case 2:
                    txtKey.Text = "?_MainProject$168";
                    break;
                case 3:
                    txtKey.Text = "Axapta3.0";
                    break;
                case 4:
                    txtKey.Text = "GTSOFT";
                    break;
                case 5:
                    txtKey.Text = "__RFID__KEY__";
                    break;
                case 6:
                    txtKey.Text = "QCDGDCHG";
                    break;
            }
        }
        #endregion

    }
}