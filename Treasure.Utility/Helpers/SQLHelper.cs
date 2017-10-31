using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.Utility.Helpers
{
    public class SQLHelper
    {
        public static string ConnString = "Data Source=172.16.96.77;Initial Catalog=EVNJITDB;User ID=evnjit;Password=Myjit.2012.963;";

        // Hashtable to store cached parameters
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        #region 后面添加

        //public static 

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

        #region 根据表名和ID获取DataRow
        /// <summary>
        /// 根据表名和ID获取DataRow
        /// </summary>
        /// <param name="pTableName">表名</param>
        /// <param name="pCN_ID">ID</param>
        /// <returns>DataRow</returns>
        public static DataRow ExecuteDataTable(string pTableName, string pCN_ID)
        {
            string strsql = @"SELECT * FROM " + pTableName + " WHERE CN_ID = @CN_ID";

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@CN_ID",SqlDbType.Int)
            };
            parms[0].Value = pCN_ID;

            DataTable dt = new DataTable();
            dt = ExecuteDataTable(CommandType.Text, strsql, parms);

            return dt.Rows[0];
        }
        #endregion

        #region 根据表名获取对应的DataTable
        /// <summary>
        /// 根据表名获取对应的DataTable
        /// </summary>
        /// <param name="pTableName">表名</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteDataTable(string pTableName)
        {
            DataTable dt = new DataTable();

            string strsql = @"SELECT * FROM " + pTableName;

            dt = ExecuteDataTable(CommandType.Text, strsql, null);

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
                sda.Fill(dt);

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
            return dt;
        }
        #endregion

        public static int InsertDatabase()
        {
            int val = -1;

            //            string strsql = @"
            //INSERT INTO SYS_USER_TYPE(CN_ID, CN_NO, CN_NAME, CN_CREATE_DATE, CN_UPDATE_DATE)
            //SELECT @CN_ID, @CN_NO, @CN_NAME, @CN_CREATE_DATE, @CN_UPDATE_DATE
            //";

            //            SqlParameter[] parms = new SqlParameter[]
            //            {
            //                new SqlParameter("@BusinessProcessId",SqlDbType.Int),
            //                new SqlParameter("@ShoesPartId",SqlDbType.Int),
            //            };
            //            parms[0].Value = pBusinessProcessId;
            //            parms[1].Value = pShoesPartId;

            //            val = ExecuteNonQuery(ConnString, CommandType.Text, strsql, parms);

            return val;
        }

        #endregion

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
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
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
    }
}
