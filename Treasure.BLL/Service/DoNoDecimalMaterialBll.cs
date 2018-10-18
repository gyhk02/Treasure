using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treasure.Utility.Helpers;

namespace Treasure.BLL.Service
{
    public class DoNoDecimalMaterialBll
    {

        #region 可用利库不该有小数的物料查询
        /// <summary>
        /// 可用利库不该有小数的物料查询
        /// </summary>
        /// <param name="pConnection">数据库链接</param>
        /// <returns></returns>
        public bool JudgeHasDecimalMaterial(string pConnection)
        {
            bool result = false;

            try
            {
                string strsql = @"
SELECT TOP 1 '在途利库' 在途利库 , '采购单' 单据类型, PT.NO 单号
	, M.NO 物料, S.NO 尺码, U.NO 单位, FSO.UNUSED_FREE_STOCK_QTY 可用利库数, PL.PURCH_QTY 采购数, FSO.ID 在途利库行ID
FROM FREE_STOCK_ONWAY FSO
JOIN FREE_STOCK_TABLE FST ON FSO.FREE_STOCK_TABLE_ID = FST.ID
JOIN MATERIAL M ON FST.MATERIAL_ID = M.ID
JOIN MATERIAL_MAIN_TYPE MMT ON M.MAIN_TYPE_ID = MMT.ID
JOIN UNIT U ON M.UNIT_ID = U.ID
LEFT JOIN SIZE S ON S.SIZE_TYPE_ID = 6 AND (FST.SIZE_ID = S.ID OR (FST.SIZE_ID IS NULL AND S.ID IS NULL))
LEFT JOIN PURCH_LINE PL ON FSO.PURCH_LINE_ID = PL.ID AND (FST.SIZE_ID = PL.MATERIAL_SIZE_ID OR (FST.SIZE_ID IS NULL AND PL.MATERIAL_SIZE_ID IS NULL))
LEFT JOIN PURCH_TABLE PT ON PL.PURCH_TABLE_ID = PT.ID
WHERE MMT.NO <> 'G' AND U.USAGE_DECIMALS = 0 AND FSO.UNUSED_FREE_STOCK_QTY <> FLOOR(FSO.UNUSED_FREE_STOCK_QTY) 
ORDER BY M.NO, S.NO, PT.NO

SELECT TOP 1 '在库利库' 在库利库
	, CASE 
		WHEN PT.NO IS NOT NULL THEN  '采购单'
		WHEN SCT.NO IS NOT NULL THEN '盘点单'
	END 单据类型
	, CASE 
		WHEN PT.NO IS NOT NULL THEN PT.NO
		WHEN SCT.NO IS NOT NULL THEN SCT.NO
	END 单号
	, M.NO 物料, S.NO 尺码, U.NO 单位, FSI.UNUSED_FREE_STOCK_QTY 可用利库数, PL.PURCH_QTY 采购数, FSI.ID 在库利库行ID
	--, IL.INSTOCK_QTY 入库数
FROM FREE_STOCK_INSTOCK FSI
JOIN FREE_STOCK_TABLE FST ON FSI.FREE_STOCK_TABLE_ID = FST.ID
JOIN MATERIAL M ON FST.MATERIAL_ID = M.ID
JOIN MATERIAL_MAIN_TYPE MMT ON M.MAIN_TYPE_ID = MMT.ID
JOIN UNIT U ON M.UNIT_ID = U.ID
LEFT JOIN SIZE S ON S.SIZE_TYPE_ID = 6 
	--AND FST.SIZE_ID = S.ID 
	AND (FST.SIZE_ID = S.ID OR (FST.SIZE_ID IS NULL AND S.ID IS NULL))
LEFT JOIN INSTOCK_LINE IL ON FSI.INSTOCK_LINE_ID = IL.ID
LEFT JOIN PURCH_LINE PL ON IL.PURCH_LINE_ID = PL.ID AND (FST.SIZE_ID = PL.MATERIAL_SIZE_ID OR (FST.SIZE_ID IS NULL AND PL.MATERIAL_SIZE_ID IS NULL))
LEFT JOIN PURCH_TABLE PT ON PL.PURCH_TABLE_ID = PT.ID
LEFT JOIN STOCK_COUNT_LINE SCL ON FSI.STOCK_COUNT_LINE_ID = SCL.ID AND (SCL.SIZE_ID = FST.SIZE_ID OR (SCL.SIZE_ID IS NULL AND FST.SIZE_ID IS NULL))
LEFT JOIN STOCK_COUNT_TABLE SCT ON SCL.STOCK_COUNT_TABLE_ID = SCT.ID
WHERE MMT.NO <> 'G' AND U.USAGE_DECIMALS = 0 AND FSI.UNUSED_FREE_STOCK_QTY <> FLOOR(FSI.UNUSED_FREE_STOCK_QTY) 
ORDER BY CASE WHEN PT.NO IS NOT NULL THEN  '采购单' WHEN SCT.NO IS NOT NULL THEN '盘点单' END, M.NO, S.NO, PT.NO

";

                DataSet ds = SqlHelper.ExecuteDataSet(pConnection, CommandType.Text, strsql, null);
                foreach (DataTable dt in ds.Tables) {
                    if (dt.Rows.Count > 0) {
                        result = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString(), ex);
            }

            return result;
        }
        #endregion

    }
}
