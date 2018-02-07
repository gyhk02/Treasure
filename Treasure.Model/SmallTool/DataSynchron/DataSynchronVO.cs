using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.Model.SmallTool.DataSynchron
{
    public static class DataSynchronVO
    {
        public static string ISIGN = "ISIGN";

        //用于数据库链接
        public static string Ip = "IP";
        public static string LoginName = "LOGIN_NAME";
        public static string Pwd = "PASSWORD";
        public static string DbName = "DATABASE_NAME";
        public static string Version = "VERSION";

        //用于表结构
        public static string TableName = "TableName";
        public static string Description = "Description";
        public static string TableDescription = "TableDescription";

        //用于表字段
        public static string FiledName = "FiledName";
        public static string FiledType = "FiledType";
        public static string FiledLen = "FiledLen";
        public static string FiledDescription = "FiledDescription";
        public static string DecimalPrecision = "DecimalPrecision";
        public static string DecimalDigits = "DecimalDigits";
        public static string IsNullable = "IsNullable";
        public static string IsIdentity = "IsIdentity";
        public static string IsMax = "IsMax";
        public static string DefaultValue = "DefaultValue";

        //用于目标表字段
        public static string TargetTableName = "TargetTableName";
        public static string TargetFiledName = "TargetFiledName";
        public static string TargetFiledType = "TargetFiledType";
        public static string TargetFiledLen = "TargetFiledLen";
        public static string TargetFiledDescription = "TargetFiledDescription";
        public static string TargetDecimalPrecision = "TargetDecimalPrecision";
        public static string TargetDecimalDigits = "TargetDecimalDigits";
        public static string TargetIsNullable = "TargetIsNullable";
        public static string TargetIsIdentity = "TargetIsIdentity";
        public static string TargetDefaultValue = "TargetDefaultValue";

        //存储过程
        //public static string ProcedureName = "ProcedureName";
        public static string ProcedureDescription = "ProcedureDescription";

        //函数
        //public static string FunctionName = "FunctionName";
        public static string FunctionDescription = "FunctionDescription";

        #region 约束

        /// <summary>
        /// 约束名称
        /// </summary>
        public static string ConstraintName = "ConstraintName";
        /// <summary>
        /// 约束类型
        /// </summary>
        public static string ConstraintType = "ConstraintType";
        /// <summary>
        /// 外键表名
        /// </summary>
        public static string ForeignTableName = "ForeignTableName";
        /// <summary>
        /// 外键字段名
        /// </summary>
        public static string ForeignFiledName = "ForeignFiledName";
        /// <summary>
        /// 索引描述
        /// </summary>
        public static string IndexDescripton = "IndexDescripton";

        #endregion

        //字段描述
        public static string DescriptionName = "DescriptionName";
    }
}
