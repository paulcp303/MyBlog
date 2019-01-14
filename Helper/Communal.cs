using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Helper
{
    public static class Communal
    {
        /// <summary>
        /// 格式转换 转换后的格式:Feed_Type
        /// </summary>
        /// <param name="strSource">原格式：Feed_Type/FEED_TYPE等</param>
        /// <returns>格式转换 转换后的格式:Feed_Type</returns>
        public static string FormatChange(this string strSource)
        {
            string[] strObj = strSource.Split('_');
            string strValue = "";
            foreach (string strSingle in strObj)
            {
                strValue += strSingle.Substring(0, 1).ToUpper();
                if (strSingle.Length > 1)
                {
                    strValue += strSingle.Substring(1).ToLower();
                }
                strValue += "_";
            }
            return strValue.Substring(0, strValue.Length - 1);
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="proName">属性</param>
        /// <returns>属性值</returns>
        public static string GetProValue<T>(T entity, string proName)
        {
            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();
            var plist = new List<PropertyInfo>(pis);
            string value = "";
            PropertyInfo info = plist.Find(p => p.Name == proName);
            if (info != null)
            {
                value = info.GetValue(entity, null) + "";
            }
            return value;
        }

        /// <summary>
        /// 返回格式化的查询条件【字段0>=@p0 and 字段1=@p1 and 字段2  like'%@p2%'】
        /// </summary>
        /// <param name="strWhere">where条件 [字段>={值}$字段={值}$字段 like '%?%' {值}]</param>
        /// <param name="paras">查询参数</param>
        /// <returns>格式化的查询条件【字段0>=@p0 and 字段1=@p1 and 字段2  like'%@p2%'】</returns>
        public static string ToSqlWhere(this string strWhere, ref List<SqlParameter> paras)
        {
            string result = "";
            if (strWhere != "")
            {
                int pIndex = 0;
                string[] strFieldsValue = strWhere.Split('$');
                foreach (string strFieldValue in strFieldsValue)
                {
                    string[] fv = strFieldValue.Split('{');
                    //条件
                    string str0 = fv[0];
                    //值
                    string str1 = fv[1].Replace("}", "");
                    if (str0.Contains("like"))
                    {
                        result += "and " + str0.Replace("?", "+@p" + pIndex + "+");
                        paras.Add(new SqlParameter("@p" + pIndex, HttpContext.Current.Server.UrlDecode(str1)));

                    }
                    else
                    {
                        result += "and " + str0 + "@p" + pIndex + " ";
                        paras.Add(new SqlParameter("@p" + pIndex, HttpContext.Current.Server.UrlDecode(str1)));
                    }

                    pIndex++;
                }

                if (result != "")
                {
                    result = result.Substring(3);
                }
            }
            else
            {
                result = "1=1";
            }
            return result;
        }



        /// <summary>
        /// 值填充entity  值格式为  字段=值&字段=值&字段=值
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="dataValue">填充entity的值( 值格式为  字段=值&字段=值&字段=值）</param>
        public static void FillEntity<T>(this T entity, string dataValue)
        {
            Type t = typeof(T);
            string[] arrKv = dataValue.Split(new char[] { '&' });
            PropertyInfo[] propers = t.GetProperties();
            var plist = new List<PropertyInfo>(propers);
            foreach (string kv in arrKv)
            {
                string[] pv = kv.Split(new char[] { '=' });
                if (pv[1] + "" != "")
                {
                    string pName = pv[0].FormatChange();
                    PropertyInfo info = plist.Find(p => p.Name == pName);
                    if (info != null)
                    {
                        object value = HttpContext.Current.Server.UrlDecode(pv[1] + "");
                        Type genericType = info.PropertyType;
                        try
                        {
                            info.SetValue(entity, Convert.ChangeType(value, genericType), null);
                        }
                        catch
                        {
                            genericType = info.PropertyType.GetGenericArguments()[0];
                            info.SetValue(entity, Convert.ChangeType(value, genericType), null);
                        }
                    }
                }
            }
        }

    }
}
