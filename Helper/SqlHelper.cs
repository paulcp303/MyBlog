using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace Helper
{
    public class SqlHelper
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        /// <returns>默认的数据库连接[Default]</returns>
        public static SqlConnection CreateConn()
        {
            string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["Default"].ToString();//
            SqlConnection conn = new SqlConnection(strConn);
            return conn;
        }

        /// <summary>
        /// 数据库连接
        /// </summary>
        /// <param name="connName">web.config配置的连接名称</param>
        /// <returns>指定的数据库连接</returns>
        public static SqlConnection CreateConn(string connName)
        {
            string strConn = System.Configuration.ConfigurationManager.ConnectionStrings[connName].ToString();
            SqlConnection conn = new SqlConnection(strConn);
            return conn;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="tran">事务</param>
        public static void RollBack(SqlTransaction tran)
        {
            if (tran != null)
            {
                tran.Rollback();
                tran = null;
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="tran">事务</param>
        public static void Commit(SqlTransaction tran)
        {
            if (tran != null)
            {
                tran.Commit();
                tran = null;
            }
        }

        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="conn">数据库连接</param>
        public static void Open(SqlConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        /// <param name="conn">数据库连接</param>
        public static void Close(SqlConnection conn)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 数据阅读器  需手动关闭数据库、dataReader
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>DataReader</returns>
        public static SqlDataReader DataReader(string strSql, SqlConnection conn)
        {
            //打开数据库
            Open(conn);
            //取数据命令
            SqlCommand cmd = new SqlCommand(strSql, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        /// <summary>
        /// 数据阅读器  需手动关闭数据库、dataReader
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="paras">sqlparameter参数</param>
        /// <returns>DataReader</returns>
        public static SqlDataReader DataReader(string strSql, SqlConnection conn, List<SqlParameter> paras)
        {
            //打开数据库
            Open(conn);
            //取数据命令
            SqlCommand cmd = new SqlCommand(strSql, conn);
            if (paras != null)
            {
                foreach (SqlParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            SqlDataReader dr = cmd.ExecuteReader();
            cmd.Parameters.Clear();
            return dr;
        }

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>DataTable</returns>
        public static DataTable DataSet(string sql, SqlConnection conn)
        {
            SqlDataAdapter sda = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            sda.Fill(ds, "table");
            return ds.Tables["table"];
        }

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="paras">sqlparameter参数</param>
        /// <returns>DataTable</returns>
        public static DataTable DataSet(string sql, SqlConnection conn, List<SqlParameter> paras)
        {
            SqlDataAdapter sda = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sql, conn);
            sda.SelectCommand = cmd;
            if (paras != null)
            {
                foreach (SqlParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            DataSet ds = new DataSet();
            sda.Fill(ds, "table");
            cmd.Parameters.Clear();
            return ds.Tables["table"];
        }

        /// <summary>
        /// 记录总数
        /// </summary>
        /// <param name="sql">sql语句(select count(1) from tablename where condition)</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>记录总数</returns>
        public static int Count(string sql, SqlConnection conn)
        {
            Open(conn);
            SqlCommand cmd = new SqlCommand(sql, conn);
            int cou = Convert.ToInt32(cmd.ExecuteScalar());
            Close(conn);
            return cou;
        }

        /// <summary>
        /// 记录总数
        /// </summary>
        /// <param name="sql">sql语句(select count(1) from tablename where condition)</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="paras">sqlparameter参数</param>
        /// <returns>记录总数</returns>
        public static int Count(string sql, SqlConnection conn, List<SqlParameter> paras)
        {
            Open(conn);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (paras != null)
            {
                foreach (SqlParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            int cou = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Parameters.Clear();
            Close(conn);
            return cou;
        }


        /// <summary>
        /// 取查询语句中第一个字段值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>查询语句中第一个字段值</returns>
        public static string ExecuteScalar(string sql, SqlConnection conn)
        {
            Open(conn);
            SqlCommand cmd = new SqlCommand(sql, conn);
            string sf = cmd.ExecuteScalar().ToString();
            Close(conn);
            return sf;
        }

        /// <summary>
        /// 取查询语句中第一个字段值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="paras">sqlparameter参数</param>
        /// <returns>查询语句中第一个字段值</returns>
        public static string ExecuteScalar(string sql, SqlConnection conn, List<SqlParameter> paras)
        {
            Open(conn);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (paras != null)
            {
                foreach (SqlParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            string sf = cmd.ExecuteScalar().ToString();
            cmd.Parameters.Clear();
            Close(conn);
            return sf;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>影响数据库的条数</returns>
        public static int ExecuteNonQuery(string sql, SqlConnection conn)
        {
            Open(conn);
            SqlCommand cmd = new SqlCommand(sql, conn);
            int recountNum = cmd.ExecuteNonQuery();
            Close(conn);
            return recountNum;
        }

        /// <summary>
        ///  执行SQL语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="paras">sqlparameter</param>
        /// <returns>影响数据库的条数</returns>
        public static int ExecuteNonQuery(string sql, string conn, List<SqlParameter> paras)
        {
            return ExecuteNonQuery(sql, CreateConn(conn), paras);
        }

        /// <summary>
        ///  执行SQL语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="sqlTran">SqlTransaction</param>
        /// <param name="paras">sqlparameter</param>
        /// <returns>影响行数</returns>
        public static int ExecuteNonQuery(string sql, SqlConnection conn, SqlTransaction sqlTran, List<SqlParameter> paras)
        {
            int recountNum = 0;
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (paras != null)
            {
                foreach (SqlParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            cmd.Transaction = sqlTran;
            recountNum = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return recountNum;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="paras">sqlparameter参数</param>
        /// <returns>影响数据库的条数</returns>
        public static int ExecuteNonQuery(string sql, SqlConnection conn, List<SqlParameter> paras)
        {
            Open(conn);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (paras != null)
            {
                foreach (SqlParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            int recountNum = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            Close(conn);
            return recountNum;
        }

        /// <summary>
        /// 判断记录是否已存在
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>存在:true 不存在:false</returns>
        public static Boolean Exists(string sql, SqlConnection conn)
        {
            DataTable dt = DataSet(sql, conn);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断记录是否已存在
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="paras">sqlparameter参数</param>
        /// <returns>存在:true 不存在:false</returns>
        public static Boolean Exists(string sql, SqlConnection conn, List<SqlParameter> paras)
        {
            DataTable dt = DataSet(sql, conn, paras);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 显示指定字段的值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="fields">指定字段</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>指定字段的值</returns>
        public static string ShowData(string sql, string fields, SqlConnection conn)
        {
            Open(conn);
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            string zhi = "";
            while (dr.Read())
            {
                zhi = dr[fields].ToString();
            }
            dr.Dispose();
            dr.Close();
            Close(conn);
            return zhi;
        }

        /// <summary>
        /// 显示指定字段的值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="fields">指定字段</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="paras">sqlparameter参数</param>
        /// <returns>指定字段的值</returns>
        public static string ShowData(string sql, string fields, SqlConnection conn, List<SqlParameter> paras)
        {
            Open(conn);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (paras != null)
            {
                foreach (SqlParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            SqlDataReader dr = cmd.ExecuteReader();
            cmd.Parameters.Clear();
            string zhi = "";
            while (dr.Read())
            {
                zhi = dr[fields].ToString();
            }
            dr.Dispose();
            dr.Close();
            Close(conn);
            return zhi;
        }
    }
}
