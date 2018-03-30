using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Treasure.Model.General;

namespace Treasure.Utility.Extend
{
    public static class DropDownListExtend
    {

        #region 绑定下拉
        /// <summary>
        /// 绑定下拉
        /// </summary>
        /// <param name="lue">DropDownList</param>
        /// <param name="dt">DataTable</param>
        /// <param name="hasPleaseSelect">是否有请选择</param>
        public static void BindToShowNo(this DropDownList ddl, DataTable dt, bool hasPleaseSelect)
        {
            ddl.DataSource = dt;
            ddl.DataValueField = GeneralVO.id;
            ddl.DataTextField = GeneralVO.no;
            ddl.DataBind();

            if (hasPleaseSelect)
            {
                ddl.SelectedValue = ConstantVO.pleaseSelect;
            }
        }
        #endregion

        #region 绑定下拉
        /// <summary>
        /// 绑定下拉
        /// </summary>
        /// <param name="lue">DropDownList</param>
        /// <param name="dt">DataTable</param>
        /// <param name="hasPleaseSelect">是否有请选择</param>
        public static void BindToShowName(this DropDownList ddl, DataTable dt, bool hasPleaseSelect)
        {
            if (hasPleaseSelect)
            {
                DataRow row = dt.NewRow();
                row[GeneralVO.id] = "-1";
                row[GeneralVO.name] = ConstantVO.pleaseSelect;
                dt.Rows.InsertAt(row, 0);
            }

            ddl.DataSource = dt;
            ddl.DataValueField = GeneralVO.id;
            ddl.DataTextField = GeneralVO.name;
            ddl.DataBind();
        }
        #endregion

    }
}
