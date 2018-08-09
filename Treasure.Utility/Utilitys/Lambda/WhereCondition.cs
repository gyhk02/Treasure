using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.Utility.Utilitys.Lambda
{
    public class WhereCondition
    {

        /// <summary>
        /// 条件字符串
        /// </summary>
        public string sqlWhere = "";

        public WhereCondition Add(string pFieldName, CompareType pFormatCondition, object pValue)
        {
            switch (pFormatCondition)
            {
                case CompareType.Equal:
                    this.sqlWhere = pFieldName + "='" + pValue + "'";
                    break;
            }

            return this;
        }
    }
    
}
