using System.Data;
using Treasure.Model.General;

namespace Treasure.BLL.General
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
            rowYes[GeneralVO.id] = 1;
            rowYes[GeneralVO.name] = "是";
            dt.Rows.Add(rowYes);

            DataRow rowNot = dt.NewRow();
            rowNot[GeneralVO.id] = 0;
            rowNot[GeneralVO.name] = "否";
            dt.Rows.Add(rowNot);

            return dt;
        }
        #endregion

    }
}
