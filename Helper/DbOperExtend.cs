using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Helper
{
    public static class DbOperExtend
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        public static int Insert<T>(this T entity) where T : new()
        {
            SqlEntityOper seo = new SqlEntityOper();
            return seo.ExecuteNonquery(entity, DbType.Insert);
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="sqlTran">sql事务</param>
        /// <returns>影响行数</returns>
        public static int Insert<T>(this T entity, SqlConnection conn, SqlTransaction sqlTran) where T : new()
        {
            SqlEntityOper seo = new SqlEntityOper();
            return seo.ExecuteNonquery(entity, DbType.Insert, sqlTran, true, conn);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="strWhere">更新条件 f0={@f0} and f1={@f1}</param>
        /// <param name="para">条件值</param>
        /// <returns>影响行数</returns>
        public static int Update<T>(this T entity, string strWhere, params object[] para)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            #region where条件参数
            MatchCollection mc = Regex.Matches(strWhere, @"\{.+?\}");
            for (int i = 0; i < mc.Count; i++)
            {
                string paraName = mc[i].Value.Replace("{", "").Replace("}", "");
                paraList.Add(new SqlParameter(paraName, para[i]));
            }
            #endregion
            SqlEntityOper seo = new SqlEntityOper();
            string strConn = "";
            string strSql = seo.JoneSqlString(entity, DbType.Update, strWhere, ref strConn, ref paraList);
            return SqlHelper.ExecuteNonQuery(strSql, strConn, paraList);
        }

        /// <summary>
        /// 更新（执行事务）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="strWhere">更新条件 f0={@f0} and f1={@f1}</param>
        /// <param name="conn"> sqlconnection连接 </param>
        /// <param name="sqlTran">sqltransaction</param>
        /// <param name="para">条件值</param>
        /// <returns>影响行数</returns>
        public static int Update<T>(this T entity, string strWhere, SqlConnection conn, SqlTransaction sqlTran, params object[] para)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            #region where条件参数
            MatchCollection mc = Regex.Matches(strWhere, @"\{.+?\}");
            for (int i = 0; i < mc.Count; i++)
            {
                string paraName = mc[i].Value.Replace("{", "").Replace("}", "");
                paraList.Add(new SqlParameter(paraName, para[i]));
            }
            #endregion


            SqlEntityOper seo = new SqlEntityOper();
            string strConn = "";
            string strSql = seo.JoneSqlString(entity, DbType.Update, strWhere, ref strConn, ref paraList);
            return SqlHelper.ExecuteNonQuery(strSql, conn, sqlTran, paraList);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="strWhere">删除条件 f0={@f0} and f1={@f1}</param>
        /// <param name="para">条件值</param>
        /// <returns>影响行数</returns>
        public static int Delete<T>(this T entity, string strWhere, params object[] para)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            #region where条件参数
            MatchCollection mc = Regex.Matches(strWhere, @"\{.+?\}");
            for (int i = 0; i < mc.Count; i++)
            {
                string paraName = mc[i].Value.Replace("{", "").Replace("}", "");
                paraList.Add(new SqlParameter(paraName, para[i]));
            }
            #endregion
            SqlEntityOper seo = new SqlEntityOper();
            string strConn = "";
            string strSql = seo.JoneSqlString(entity, DbType.Delete, strWhere, ref strConn, ref paraList);
            return SqlHelper.ExecuteNonQuery(strSql, strConn, paraList);
        }

        /// <summary>
        /// 删除（执行事务）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="strWhere">删除条件 f0={@f0} and f1={@f1}</param>
        /// <param name="conn"> sqlconnection连接 </param>
        /// <param name="sqlTran">sqltransaction</param>
        /// <param name="para">条件值</param>
        /// <returns>影响行数</returns>
        public static int Delete<T>(this T entity, string strWhere, SqlConnection conn, SqlTransaction sqlTran, params object[] para)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            #region where条件参数
            MatchCollection mc = Regex.Matches(strWhere, @"\{.+?\}");
            for (int i = 0; i < mc.Count; i++)
            {
                string paraName = mc[i].Value.Replace("{", "").Replace("}", "");
                paraList.Add(new SqlParameter(paraName, para[i]));
            }
            #endregion


            SqlEntityOper seo = new SqlEntityOper();
            string strConn = "";
            string strSql = seo.JoneSqlString(entity, DbType.Delete, strWhere, ref strConn, ref paraList);
            return SqlHelper.ExecuteNonQuery(strSql, conn, sqlTran, paraList);
        }

        /// <summary>
        /// 选择一条记录
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="fields">查询的字段</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="para">sqlparametor参数值</param>
        /// <returns>是否存在记录 存在:true 不存在:false 并为实体赋值</returns>
        public static bool Select<T>(this T entity, string fields, string strWhere, params object[] para)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            #region where条件参数
            MatchCollection mc = Regex.Matches(strWhere, @"\{.+?\}");
            for (int i = 0; i < mc.Count; i++)
            {
                string paraName = mc[i].Value.Replace("{", "").Replace("}", "");
                paraList.Add(new SqlParameter(paraName, para[i]));
            }
            #endregion

            SqlEntityOper seo = new SqlEntityOper();
            string strWhere0 = strWhere.Replace("{", "").Replace("}", "");
            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();
            string strConn = "Default";
            foreach (PropertyInfo pi in pis)
            {
                if (pi.Name == "Conn")
                {
                    strConn = pi.GetValue(entity, null) + "";
                }
            }
            string strSql = string.Format("select {0} from {1} where {2}", fields, t.Name, strWhere0);
            DataTable dt = SqlHelper.DataSet(strSql, SqlHelper.CreateConn(strConn), paraList);
            if(dt.Rows.Count>0)
            {
                 DataRow dr = dt.Rows[0];
                 DataRowToT<T>(entity,dt, dr);
                 return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 选择第一条记录
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="fields">查询的字段</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="order">排序字段</param>
        /// <param name="para">sqlparametor参数值</param>
        /// <returns>是否存在记录 存在:true 不存在:false 并为实体赋值</returns>
        public static bool First<T>(this T entity, string fields, string strWhere, string order, params object[] para)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            #region where条件参数
            MatchCollection mc = Regex.Matches(strWhere, @"\{.+?\}");
            for (int i = 0; i < mc.Count; i++)
            {
                string paraName = mc[i].Value.Replace("{", "").Replace("}", "");
                paraList.Add(new SqlParameter(paraName, para[i]));
            }
            #endregion

            SqlEntityOper seo = new SqlEntityOper();
            string strWhere0 = strWhere.Replace("{", "").Replace("}", "");
            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();
            string strConn = "Default";
            foreach (PropertyInfo pi in pis)
            {
                if (pi.Name == "Conn")
                {
                    strConn = pi.GetValue(entity, null) + "";
                }
            }
            string strSql = string.Format("select {0} from {1} where {2} order by {3}", fields, t.Name, strWhere0, order);
            DataTable dt = SqlHelper.DataSet(strSql, SqlHelper.CreateConn(strConn), paraList);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                DataRowToT<T>(entity, dt, dr);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 取某个字段的最大值
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="field">字段</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="para">where条件参数</param>
        /// <returns>取某个字段的最大值</returns>
        public static string Max<T>(this T entity, string field, string strWhere, params object[] para)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            #region where条件参数
            MatchCollection mc = Regex.Matches(strWhere, @"\{.+?\}");
            for (int i = 0; i < mc.Count; i++)
            {
                string paraName = mc[i].Value.Replace("{", "").Replace("}", "");
                paraList.Add(new SqlParameter(paraName, para[i]));
            }
            #endregion

            SqlEntityOper seo = new SqlEntityOper();
            string strWhere0 = strWhere.Replace("{", "").Replace("}", "");
            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();
            string strConn = "Default";
            foreach (PropertyInfo pi in pis)
            {
                if (pi.Name == "Conn")
                {
                    strConn = pi.GetValue(entity, null) + "";
                }
            }
            string strSql = string.Format("select ISNULL(max({0}),0) from {1} where {2}", field, t.Name, strWhere0);
            string maxValue = SqlHelper.ExecuteScalar(strSql, SqlHelper.CreateConn(strConn), paraList);
            return maxValue;
        }

        /// <summary>
        /// 取某个字段的最大值
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="field">字段</param>
        /// <returns>取某个字段的最大值</returns>
        public static string Max<T>(this T entity, string field)
        {
            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();
            string strConn = "Default";
            foreach (PropertyInfo pi in pis)
            {
                if (pi.Name == "Conn")
                {
                    strConn = pi.GetValue(entity, null) + "";
                }
            }
            string strSql = string.Format("select ISNULL(max({0}),0) from {1}", field, t.Name);
            string maxValue = SqlHelper.ExecuteScalar(strSql, SqlHelper.CreateConn(strConn), null);
            return maxValue;
        }

        /// <summary>
        /// 选择记录List
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="fields">查询的字段</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="para">sqlparametor参数值</param>
        /// <returns>选择记录List</returns>
        public static List<T> Fill<T>(this T entity, string fields, string strWhere, params object[] para)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            #region where条件参数
            MatchCollection mc = Regex.Matches(strWhere, @"\{.+?\}");
            for (int i = 0; i < mc.Count; i++)
            {
                string paraName = mc[i].Value.Replace("{", "").Replace("}", "");
                paraList.Add(new SqlParameter(paraName, para[i]));
            }
            #endregion

            SqlEntityOper seo = new SqlEntityOper();
            string strWhere0 = strWhere.Replace("{", "").Replace("}", "");
            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();
            string strConn = "Default";
            foreach (PropertyInfo pi in pis)
            {
                if (pi.Name == "Conn")
                {
                    strConn = pi.GetValue(entity, null) + "";
                }
            }
            string strSql = string.Format("select {0} from {1} where {2}", fields, t.Name, strWhere0);
            DataTable dt = SqlHelper.DataSet(strSql, SqlHelper.CreateConn(strConn), paraList);
            return DataTableToList<T>(dt);
        }

        /// <summary>
        /// 选择记录List
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="fields">查询的字段</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="order">排序字段</param>
        /// <param name="para">sqlparametor参数值</param>
        /// <returns>选择记录List</returns>
        public static List<T> FillOrder<T>(this T entity, string fields, string strWhere, string order, params object[] para)
        {
            List<SqlParameter> paraList = new List<SqlParameter>();
            #region where条件参数
            MatchCollection mc = Regex.Matches(strWhere, @"\{.+?\}");
            for (int i = 0; i < mc.Count; i++)
            {
                string paraName = mc[i].Value.Replace("{", "").Replace("}", "");
                paraList.Add(new SqlParameter(paraName, para[i]));
            }
            #endregion

            SqlEntityOper seo = new SqlEntityOper();
            string strWhere0 = strWhere.Replace("{", "").Replace("}", "");
            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();
            string strConn = "Default";
            foreach (PropertyInfo pi in pis)
            {
                if (pi.Name == "Conn")
                {
                    strConn = pi.GetValue(entity, null) + "";
                }
            }
            string strSql = string.Format("select {0} from {1} where {2} order by {3}", fields, t.Name, strWhere0, order);
            DataTable dt = SqlHelper.DataSet(strSql, SqlHelper.CreateConn(strConn), paraList);
            return DataTableToList<T>(dt);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <param name="selectfields">查询列（多列用,隔开）</param>
        /// <param name="orderfield">排序队列</param>
        /// <param name="strWhere">条件</param>
        /// <param name="beginindex">起始行数</param>
        /// <param name="endindex">结束行数</param>
        /// <returns></returns>
        public static DataTable GetPage<T>(this T entity, string selectfields, string orderfield, string strWhere, int beginindex, int endindex)
        {
            DataTable dt = null;
            Type t = typeof(T);
            if(string.IsNullOrEmpty(strWhere)){
                strWhere = " 1=1";
            }
            string sql = string.Format(" select * from "+
                                       " ( select row_number() over (order by {0}) rowid,{1} from {2} where {3}) d "+
                                       " where rowid>{4} and rowid<={5}", orderfield, selectfields, t.Name, strWhere, beginindex, endindex);
            dt = SqlHelper.DataSet(sql, SqlHelper.CreateConn());
            return dt;
        }


        public static void DataRowToT<T>(T entity,DataTable dt,DataRow dr)
        {
            Type t = typeof(T);
            PropertyInfo[] propers = t.GetProperties();
            var plist = new List<PropertyInfo>(propers);
            //T s = System.Activator.CreateInstance<T>();
            for (int i = 0; i <dt.Columns.Count; i++)
            {
                string strClm = dt.Columns[i].ColumnName.FormatChange();
                PropertyInfo info = plist.Find(p => p.Name ==strClm);
                if (info != null)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        info.SetValue(entity, dr[i], null);
                    }
                }
            }
        }

        /// <summary>
        /// 将dataTable转成List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable dt)
        {
            if (dt == null) return new List<T>();
            var list = new List<T>();
            Type type = typeof(T);
            PropertyInfo[] props =  type.GetProperties();

            var plist = new List<PropertyInfo>(props);
            foreach (DataRow item in dt.Rows)
            {
                T s = System.Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string strClm = dt.Columns[i].ColumnName.FormatChange();
                    PropertyInfo info = plist.Find(p => p.Name ==strClm);
                    if (info != null)
                    {
                        if (!Convert.IsDBNull(item[i]))
                        {
                            info.SetValue(s, item[i], null);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }

    }
}
