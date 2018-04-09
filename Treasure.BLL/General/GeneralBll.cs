using System.Collections.Generic;
using System.Data;
using Treasure.Model.General;

namespace Treasure.Bll.General
{
    public class GeneralBll
    {

        #region 得到一个是否的DataTable
        /// <summary>
        /// 得到一个是否的DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable GetYesOrNot()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(GeneralVO.id);
            dt.Columns.Add(GeneralVO.name);

            DataRow rowYes = dt.NewRow();
            rowYes[GeneralVO.id] = true;
            rowYes[GeneralVO.name] = "是";
            dt.Rows.Add(rowYes);

            DataRow rowNot = dt.NewRow();
            rowNot[GeneralVO.id] = false;
            rowNot[GeneralVO.name] = "否";
            dt.Rows.Add(rowNot);

            return dt;
        }
        #endregion

        #region Grid默认排除的字段
        /// <summary>
        /// Grid默认排除的字段
        /// </summary>
        /// <returns></returns>
        public List<string> GetExcludedFields()
        {
            List<string> lst = new List<string>();

            lst.Add(GeneralVO.id);
            lst.Add(GeneralVO.idIndex);
            lst.Add(GeneralVO.createUserId);
            lst.Add(GeneralVO.createDatetime);
            lst.Add(GeneralVO.modifyUserId);
            lst.Add(GeneralVO.modifyDatetime);

            return lst;
        }
        #endregion
    }
}
