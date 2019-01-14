using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;

namespace Helper
{
    public class SqlEntityOper
    {

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <param name="dbType">数据操作类型select insert update delete</param>
        /// <param name="tran">执行事务 true:执行 false:不执行</param>
        /// <returns>返回影响的行数</returns>
        public int ExecuteNonquery<T>(T entity, DbType dbType)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            string strConn = "";
            string strSql = JoneSqlString(entity, dbType, ref strConn, ref paraList);
            return SqlHelper.ExecuteNonQuery(strSql, strConn, paraList);
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <param name="dbType">数据操作类型select insert update delete</param>
        /// <param name="sqlTran">SqlTransaction</param>
        /// <param name="tran">执行事务 true:执行 false:不执行</param>
        /// <param name="conn">数据库连接</param>
        /// <returns>返回影响的行数</returns>
        public int ExecuteNonquery<T>(T entity, DbType dbType, SqlTransaction sqlTran, bool tran, SqlConnection conn)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            string strConn = "";
            string strSql = JoneSqlString(entity, dbType, ref strConn, ref paraList);
            if (tran)
            {
                return SqlHelper.ExecuteNonQuery(strSql, conn, sqlTran, paraList);
            }
            else
            {
                return SqlHelper.ExecuteNonQuery(strSql, conn, paraList);
            }
        }



        /// <summary>
        /// 拼接Sql串 
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="dbType">数据操作类型select insert update delete</param>
        /// <param name="strConn">数据库连接key</param>
        /// <param name="paralist">数据参数</param>
        /// <returns> 拼接后Sql串 </returns>
        public string JoneSqlString<T>(T entity, DbType dbType, ref string strConn, ref List<SqlParameter> paralist)
        {

            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();
            List<string> fieldsList = new List<string>();
            List<string> valuesList = new List<string>();
            strConn = "Default";
            List<SqlParameter> paras = new List<SqlParameter>();
            foreach (PropertyInfo pi in pis)
            {
                if (pi.GetValue(entity, null) != null && pi.Name != "Conn")
                {
                    fieldsList.Add(pi.Name);
                    valuesList.Add("@" + pi.Name);
                    paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(entity, null)));
                }
                else if (pi.Name == "Conn")
                {
                    strConn = pi.GetValue(entity, null) + "";
                }
            }
            paralist = paras;

            //where条件参数
            string strSql = "";
            string tableName = t.Name;
            switch (dbType)
            {
                case DbType.Select:
                    //strSql = "select * from " + tableName + " where ";
                    break;
                case DbType.Insert:
                    #region InsertSql
                    StringBuilder fieldsBuilder = new StringBuilder("");
                    StringBuilder valuesBuilder = new StringBuilder("");
                    bool flag = true;//第一个字段
                    int i = 0;
                    foreach (string strFiled in fieldsList)
                    {
                        if (flag)
                        {
                            fieldsBuilder.AppendFormat("{0}", strFiled);
                            valuesBuilder.AppendFormat("{0}", valuesList[i]);
                            flag = false;
                        }
                        else
                        {
                            fieldsBuilder.AppendFormat(",{0}", strFiled);
                            valuesBuilder.AppendFormat(",{0}", valuesList[i]);
                        }
                        i++;
                    }
                    strSql = string.Format("insert into {0}({1}) values({2})", tableName, fieldsBuilder.ToString(), valuesBuilder.ToString());
                    break;
                    #endregion
                case DbType.Update: break;
                case DbType.Delete: break;
            }
            return strSql;
        }


        /// <summary>
        /// 拼接Sql串 
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="dbType">数据操作类型select insert update delete</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="strConn">数据库连接key</param>
        /// <param name="paralist">数据参数(默认带的为where条件的参数值)</param>
        /// <returns> 拼接后Sql串 </returns>
        public string JoneSqlString<T>(T entity, DbType dbType, string strWhere, ref string strConn, ref List<SqlParameter> paralist)
        {
            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();
            List<string> fieldsList = new List<string>();
            List<string> valuesList = new List<string>();
            strConn = "Default";
            List<SqlParameter> paras = new List<SqlParameter>();
            foreach (PropertyInfo pi in pis)
            {
                if (pi.GetValue(entity, null) != null && pi.Name != "Conn")
                {
                    fieldsList.Add(pi.Name);
                    valuesList.Add("@" + pi.Name);
                    paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(entity, null)));
                }
                else if (pi.Name == "Conn")
                {
                    strConn = pi.GetValue(entity, null) + "";
                }
            }

            //where条件
            string strWhere0 = strWhere.Replace("{", "").Replace("}", "");
            List<SqlParameter> paraAll = paras;
            List<SqlParameter> paraouts = paralist;
            foreach (SqlParameter paraout in paraouts)
            {
                paraAll.Add(paraout);
            }
            paralist.Clear();
            paralist = paraAll;

            string strSql = "";
            string tableName = t.Name;
            switch (dbType)
            {
                case DbType.Select:
                    //strSql = "select * from " + tableName + " where ";
                    break;
                case DbType.Update:
                    #region UpdateSql
                    StringBuilder fieldsBuilderu = new StringBuilder("");
                    bool flagu = true;//第一个字段
                    foreach (string strFiled in fieldsList)
                    {
                        if (flagu)
                        {
                            fieldsBuilderu.AppendFormat("{0}=@{0}", strFiled);
                            flagu = false;
                        }
                        else
                        {
                            fieldsBuilderu.AppendFormat(",{0}=@{0}", strFiled);
                        }
                    }
                    strSql = string.Format("update {0} set {1} where {2}", tableName, fieldsBuilderu.ToString(), strWhere0);
                    #endregion
                    break;
                case DbType.Delete:
                    #region DeleteSql
                    strSql = string.Format("delete from {0} where {1}", tableName, strWhere0);
                    #endregion
                    break;
            }
            return strSql;
        }

    }
}
