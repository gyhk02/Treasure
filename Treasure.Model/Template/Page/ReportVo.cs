using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.Model.Template.Page
{
    public class ReportVo
    {
        public static string tableName = "MenuItem";

        public static class Fields        {            public static string name = "NAME";
            public static string idIndex = "ID_INDEX";
            public static string isSys = "IS_SYS";
            public static string createDatetime = "CREATE_DATETIME";
        }
    }
}
