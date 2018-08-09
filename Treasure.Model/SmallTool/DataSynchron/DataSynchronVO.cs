namespace Treasure.Model.SmallTool.DataSynchron
{
    public static class DataSynchronVO
    {
        public readonly static string ISIGN = "ISIGN";

        //用于数据库链接
        public readonly static string Ip = "IP";
        public readonly static string LoginName = "LOGIN_NAME";
        public readonly static string PassWord = "PASSWORD";
        public readonly static string DbName = "DATABASE_NAME";
        public readonly static string Version = "VERSION";

        //用于表结构
        public readonly static string TableName = "TableName";
        public readonly static string Description = "Description";
        public readonly static string TableDescription = "TableDescription";

        //用于表字段
        public readonly static string FieldName = "FieldName";
        public readonly static string FieldType = "FieldType";
        public readonly static string FiledLen = "FiledLen";
        public readonly static string FieldDescription = "FieldDescription";
        public readonly static string DecimalPrecision = "DecimalPrecision";
        public readonly static string DecimalDigits = "DecimalDigits";
        public readonly static string IsNullable = "IsNullable";
        public readonly static string IsIdentity = "IsIdentity";
        public readonly static string IsMax = "IsMax";
        public readonly static string DefaultValue = "DefaultValue";

        //用于目标表字段
        public readonly static string TargetTableName = "TargetTableName";
        public readonly static string TargetFieldName = "TargetFieldName";
        public readonly static string TargetFiledType = "TargetFiledType";
        public readonly static string TargetFiledLen = "TargetFiledLen";
        public readonly static string TargetFiledDescription = "TargetFiledDescription";
        public readonly static string TargetDecimalPrecision = "TargetDecimalPrecision";
        public readonly static string TargetDecimalDigits = "TargetDecimalDigits";
        public readonly static string TargetIsNullable = "TargetIsNullable";
        public readonly static string TargetIsIdentity = "TargetIsIdentity";
        public readonly static string TargetDefaultValue = "TargetDefaultValue";

        //存储过程
        public readonly static string ProcedureDescription = "ProcedureDescription";

        //函数
        public readonly static string FunctionDescription = "FunctionDescription";

        #region 约束

        /// <summary>
        /// 约束名称
        /// </summary>
        public readonly static string ConstraintName = "ConstraintName";
        /// <summary>
        /// 约束类型
        /// </summary>
        public readonly static string ConstraintType = "ConstraintType";
        /// <summary>
        /// 外键表名
        /// </summary>
        public readonly static string ForeignTableName = "ForeignTableName";
        /// <summary>
        /// 外键字段名
        /// </summary>
        public readonly static string ForeignFieldName = "ForeignFieldName";
        /// <summary>
        /// 索引描述
        /// </summary>
        public readonly static string IndexDescripton = "IndexDescripton";

        #endregion

        //字段描述
        public readonly static string DescriptionName = "DescriptionName";
    }
}
