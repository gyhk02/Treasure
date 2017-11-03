using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.Utility.Helpers
{
    /// <summary>
    /// DataTable操作帮忙类
    /// </summary>
    public class DataTableHelper
    {

        #region  合并DataTable的数据
        /// <summary>
        /// 合并DataTable的数据
        /// </summary>
        /// <param name="lstTable">DataTable列表</param>
        /// <returns>DataTable</returns>
        public DataTable Merge(List<DataTable> lstTable)
        {
            DataTable result = null;

            if (lstTable.Count == 1)
            {
                return lstTable[0];
            }

            try
            {
                for (int idx = 0; idx < lstTable.Count; idx++)
                {
                    DataTable dt = lstTable[idx];

                    if (idx == 0)
                    {
                        result = dt.Clone();
                    }
                    else
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            result.ImportRow(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return result;
        }
        #endregion

    }
}
