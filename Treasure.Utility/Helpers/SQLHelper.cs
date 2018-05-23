using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treasure.Model.General;
using Treasure.Utility.Utilitys;

namespace Treasure.Utility.Helpers
{
    /// <summary>
    /// 数据库操作帮忙类
    /// </summary>
    public static class SqlHelper
    {
        public static string ConnString = "Data Source=.;Initial Catalog=Treasure;User ID=sa;Password=1;";
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        #region 后面添加

        public static void DataTableToSQLServer(DataTable dt, string connectString)
        {
            string connectionString = connectString;

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                destinationConnection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    try
                    {
                        bulkCopy.DestinationTableName = "checkinout";//要插入的表的表名
                        bulkCopy.BatchSize = dt.Rows.Count;
                        bulkCopy.ColumnMappings.Add("ID", "ID");//映射字段名 DataTable列名 ,数据库 对应的列名  
                        bulkCopy.ColumnMappings.Add("TIME", "TIME");

                        bulkCopy.WriteToServer(dt);

                        //System.Windows.Forms.MessageBox.Show("插入成功");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally { }
                }
            }
        }

        #region 删除

        #region 根据ID删除记录

        /// <summary>
        /// 根据ID删除记录
        /// </summary>
        /// <param name="pTableName">表名</param>
        /// <param name="pId">id</param>
        /// <returns></returns>
        public static bool DeleteById(string pTableName, string pId)
        {
            return DeleteById(null, pTableName, pId);
        }

        /// <summary>
        /// 根据ID删除记录
        /// </summary>
        /// <param name="pConnString">数据库链接</param>
        /// <param name="pTableName">表名</param>
        /// <param name="pId">id</param>
        /// <returns></returns>
        public static bool DeleteById(string pConnString, string pTableName, string pId)
        {
            bool result = false;

            string strsql = @"DELETE FROM [" + pTableName + "] WHERE ID = @ID";

            try
            {
                if (string.IsNullOrEmpty(pConnString) == true)
                {
                    pConnString = ConnString;
                }

                List<SqlParameter> lstPara = new List<SqlParameter>();
                lstPara.Add(new SqlParameter("@ID", SqlDbType.NVarChar) { Value = pId });

                ExecuteNonQuery(pConnString, CommandType.Text, strsql, lstPara.ToArray());
                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return result;
        }
        #endregion

        #region 根据表名删除表的全部数据

        /// <summary>
        /// 根据表名删除表的全部数据
        /// </summary>
        /// <param name="pTableName">表名</param>
        /// <returns></returns>
        public static bool DeleteDataTableByName(string pTableName)
        {
            return DeleteDataTableByName(null, pTableName);
        }

        /// <summary>
        /// 根据表名删除表的全部数据
        /// </summary>
        /// <param name="pConnString">数据库链接</param>
        /// <param name="pTableName">表名</param>
        /// <returns></returns>
        public static bool DeleteDataTableByName(string pConnString, string pTableName)
        {
            bool result = false;

            string strsql = @"DELETE FROM [" + pTableName + "]";

            try
            {
                if (string.IsNullOrEmpty(pConnString) == true)
                {
                    pConnString = ConnString;
                }

                ExecuteNonQuery(pConnString, CommandType.Text, strsql, null);
                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return result;
        }
        #endregion

        #endregion

        #region 查询

        #region 根据表名获取表结构
        /// <summary>
        /// 根据表名获取表结构
        /// </summary>
        /// <param name="pTableName"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string pTableName)
        {
            string strsql = @"SELECT TOP 0 * FROM [" + pTableName + "]";

            DataTable dt = ExecuteDataTable(CommandType.Text, strsql, null);

            return dt;
        }
        #endregion

        #region 根据表名和ID获取DataRow
        /// <summary>
        /// 根据表名和ID获取DataRow
        /// </summary>
        /// <param name="pTableName">表名</param>
        /// <param name="pID">ID</param>
        /// <returns>DataRow</returns>
        public static DataRow ExecuteDataTable(string pTableName, string pID)
        {
            DataRow row = null;

            string strsql = @"SELECT * FROM [" + pTableName + "] WHERE ID = @ID";

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@ID",SqlDbType.NVarChar)
            };
            parms[0].Value = pID;

            DataTable dt = ExecuteDataTable(CommandType.Text, strsql, parms);

            if (dt.Rows.Count > 0)
            {
                row = dt.Rows[0];
            }

            return row;
        }
        #endregion

        #region 根据表名获取对应的DataTable
        /// <summary>
        /// 根据表名获取对应的DataTable
        /// </summary>
        /// <param name="pTableName">表名</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteDataTableByName(string pTableName)
        {
            return ExecuteDataTableByName(null, pTableName);
        }

        /// <summary>
        /// 根据表名获取对应的DataTable
        /// </summary>
        /// <param name="pConnString">数据库链接</param>
        /// <param name="pTableName">表名</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTableByName(string pConnString, string pTableName)
        {
            DataTable dt;

            string strsql = @"SELECT * FROM " + "[" + pTableName + "]";

            if (string.IsNullOrEmpty(pConnString) == true)
            {
                dt = ExecuteDataTable(CommandType.Text, strsql, null);
            }
            else
            {
                dt = ExecuteDataTable(pConnString, CommandType.Text, strsql, null);
            }

            return dt;
        }
        #endregion

        #region 根据sql语句获取datatable
        /// <summary>
        /// 根据sql语句获取datatable
        /// </summary>
        /// <param name="cmdType">提交类型</param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteDataTable(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteDataTable(ConnString, cmdType, cmdText, cmdParms);
        }
        #endregion

        #region 根据sql语句获取datatable
        /// <summary>
        /// 根据sql语句获取datatable
        /// </summary>
        /// <param name="connString">数据库链接</param>
        /// <param name="cmdType">提交类型</param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteDataTable(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(connString);
            SqlDataAdapter sda = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                sda.SelectCommand = cmd;
                sda.SelectCommand.CommandText = cmdText;
                sda.SelectCommand.CommandType = cmdType;
                sda.SelectCommand.Connection = conn;
                //sda.TableMappings.Add()
                sda.MissingSchemaAction = MissingSchemaAction.Add;

                //获取表名
                System.Text.RegularExpressions.Regex rg
                    = new System.Text.RegularExpressions.Regex(@"(?<=FROM\s\[?)\w+|(?<=JOIN\s\[?)\w+|(?<=LEFT JOIN\s\[?)\w+");
                System.Text.RegularExpressions.MatchCollection mc = rg.Matches(cmdText);

                string bakTableName = "";
                if (mc.Count == 1)
                {
                    bakTableName = mc[0].Value;
                }

                if (cmdType == CommandType.StoredProcedure) sda.SelectCommand.CommandTimeout = 5000;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                    {
                        sda.SelectCommand.Parameters.Add(parm);
                    }
                }

                if (string.IsNullOrEmpty(bakTableName) == true)
                {
                    sda.Fill(ds);
                }
                else
                {
                    sda.Fill(ds, bakTableName);
                }

                sda.SelectCommand.Parameters.Clear();
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }
        #endregion

        #endregion

        #region 新增

        #region 插入DataTable
        /// <summary>
        /// 插入DataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<string> AddDataTable(DataTable dt)
        {
            List<string> lstId = new List<string>();

            foreach (DataRow row in dt.Rows)
            {
                lstId.Add(AddDataRow(row));
            }

            return lstId;
        }
        #endregion

        #region 新增一行数据
        /// <summary>
        /// 新增一行数据
        /// </summary>
        /// <param name="row">要插入的数据行</param>
        /// <returns></returns>
        public static string AddDataRow(DataRow row)
        {
            return AddDataRow(row, null);
        }
        #endregion

        #region 新增一行数据
        /// <summary>
        /// 新增一行数据
        /// </summary>
        /// <param name="row">要插入的数据行</param>
        /// <param name="pConnString">数据库链接</param>
        /// <returns></returns>
        public static string AddDataRow(DataRow row, string pConnString)
        {
            string id = "";

            try
            {
                if (string.IsNullOrEmpty(pConnString) == true)
                {
                    pConnString = ConnString;
                }

                #region SQL及参数

                List<string> lstColumnName = new List<string>();
                List<string> lstVar = new List<string>();
                List<SqlParameter> lstPara = new List<SqlParameter>();

                DataTable dt = row.Table;
                foreach (DataColumn col in dt.Columns)
                {
                    string columnName = col.ColumnName;

                    //ID_INDEX是自增长
                    if (columnName.Equals(GeneralVO.idIndex) == true)
                    {
                        continue;
                    }

                    lstColumnName.Add(columnName);
                    lstVar.Add("@" + columnName);

                    SqlDbType type = TypeConversion.ToSqlDbType(col.DataType);
                    lstPara.Add(new SqlParameter("@" + columnName, type) { Value = row[col] });

                    //如果是ID，就返回ID的值
                    if (columnName.Equals(GeneralVO.id) == true)
                    {
                        id = TypeConversion.ToString(row[col]);
                    }
                }

                string strsql = "INSERT INTO " + row.Table.TableName
                     + "(" + string.Join(", ", lstColumnName.ToArray()) + ") SELECT "
                     + string.Join(", ", lstVar.ToArray());

                #endregion

                ExecuteNonQuery(pConnString, CommandType.Text, strsql, lstPara.ToArray());
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return id;
        }
        #endregion

        #endregion

        #region 修改

        /// <summary>
        /// 修改一行数据
        /// </summary>
        /// <param name="row">要修改的数据行</param>
        /// <returns></returns>
        public static string UpdateDataRow(DataRow row)
        {
            return UpdateDataRow(row, null);
        }
        /// <summary>
        /// 修改一行数据
        /// </summary>
        /// <param name="row">要修改的数据行</param>
        /// <param name="pConnString">数据库链接</param>
        /// <returns></returns>
        public static string UpdateDataRow(DataRow row, string pConnString)
        {
            string id = "";

            try
            {
                if (string.IsNullOrEmpty(pConnString) == true)
                {
                    pConnString = ConnString;
                }

                #region SQL及参数

                List<string> lstColumn = new List<string>();
                List<SqlParameter> lstPara = new List<SqlParameter>();

                DataTable dt = row.Table;
                foreach (DataColumn col in dt.Columns)
                {
                    string columnName = col.ColumnName;

                    if (columnName.Equals(GeneralVO.idIndex) == false)
                    {
                        lstColumn.Add(columnName + " = @" + columnName);

                        SqlDbType type = TypeConversion.ToSqlDbType(col.DataType);
                        lstPara.Add(new SqlParameter("@" + columnName, type) { Value = row[col] });
                    }
                }

                string strsql = "UPDATE " + dt.TableName + " SET " + string.Join(", ", lstColumn.ToArray()) + " WHERE ID = @ID";

                #endregion

                ExecuteNonQuery(pConnString, CommandType.Text, strsql, lstPara.ToArray());
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

            return id;
        }

        #endregion

        #endregion

        #region 原SQLHelper文件代码

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            conn.Close();
            return val;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">an existing sql transaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public static SqlDataReader ExecuteReader(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connString);

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteReader(ConnString, cmdType, cmdText, cmdParms);
        }

        public static SqlDataReader ExecuteParmReader(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connString);

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// 专门用于打印报表
        /// </summary>
        public static DataSet ExecuteDataSet(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            DataSet ds = new DataSet("root");
            SqlConnection conn = new SqlConnection(connString);
            SqlDataAdapter sda = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                sda.SelectCommand = cmd;
                sda.SelectCommand.CommandText = cmdText;
                sda.SelectCommand.CommandType = cmdType;
                sda.SelectCommand.Connection = conn;

                if (cmdType == CommandType.StoredProcedure) sda.SelectCommand.CommandTimeout = 5000;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                        sda.SelectCommand.Parameters.Add(parm);
                }
                sda.Fill(ds, "david");

                sda.SelectCommand.Parameters.Clear();
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return ds;
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            //SqlCommand cmd = new SqlCommand();

            //using (SqlConnection conn = new SqlConnection(connString)) {
            //	PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            //	object val = cmd.ExecuteScalar();
            //	cmd.Parameters.Clear();
            //	return val;
            //}
            SqlConnection conn = new SqlConnection(connString);
            return ExecuteScalar(conn, cmdType, cmdText, cmdParms);
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">an existing database transaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] cmdParms)
        {
            parmCache[cacheKey] = cmdParms;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];
            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText
            , SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = 500;

            if (cmdType == CommandType.StoredProcedure) cmd.CommandTimeout = 10000;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion

    }
}
