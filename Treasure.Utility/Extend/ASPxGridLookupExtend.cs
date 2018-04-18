using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treasure.Model.General;

namespace Treasure.Utility.Extend
{
    public static class ASPxGridLookupExtend
    {
        #region 绑定下拉
        /// <summary>
        /// 绑定下拉
        /// </summary>
        /// <param name="lue">ASPxGridLookup</param>
        /// <param name="dt">DataTable</param>
        /// <param name="hasPleaseSelect">是否有请选择</param>
        public static void BindToShowName(this ASPxGridLookup lup, DataTable dt, bool hasPleaseSelect)
        {
            if (hasPleaseSelect)
            {
                DataRow row = dt.NewRow();
                row[GeneralVO.id] = DBNull.Value;
                row[GeneralVO.name] = ConstantVO.pleaseSelect;
                dt.Rows.InsertAt(row, 0);
            }

            lup.NullText = ConstantVO.pleaseSelect;
            lup.DataSource = dt;
            lup.DataBind();
        }
        #endregion
    }
}
