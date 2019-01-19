using DAL;
using Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class EssaysController : ApiController
    {
        /// <summary>
        /// 返回json格式数据
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public HttpResponseMessage ReturnJson(string content)
        {
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(content, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetEssaysPage()
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");
            string key = HttpContext.Current.Request["key"];
            string limit = HttpContext.Current.Request["limit"];
            string page = HttpContext.Current.Request["page"];
            try
            {
                if (string.IsNullOrEmpty(limit) || string.IsNullOrEmpty(page))
                {
                    rh.msg = "缺少分页参数";
                    rh.code = 300;
                }
                else
                {
                    Essays obj = new Essays();
                    string strWhere = " 1=1";
                    if (!string.IsNullOrEmpty(key))
                    {
                        strWhere += string.Format(" and (Essay_Type like '%{0}%' or Essay_Title like '%{0}%' or Essay_Label like '%{0}%')", key);
                    }
                    int begin = (Convert.ToInt32(page) - 1) * Convert.ToInt32(limit);
                    int end = Convert.ToInt32(page) * Convert.ToInt32(limit);
                    DataTable dt = obj.GetPage("*", "Report_Time desc", strWhere, begin, end);
                    if (dt.Rows.Count > 0)
                    {
                        rh.totals = SqlHelper.Count(string.Format("select * from Essays where {0}", strWhere), SqlHelper.CreateConn());
                        rh.data = dt;
                        rh.msg = "获取成功";
                    }
                }
            }
            catch (Exception e)
            {
                rh.code = 500;
                rh.msg = "处理错误";
            }
            return ReturnJson(JsonConvert.SerializeObject(rh));
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage AddEssay(Essays obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                obj.Essayid = Guid.NewGuid().ToString("N");
                obj.Report_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                obj.Ip = IPHelper.GetHostIP();
                obj.Address = IPHelper.GetHostAddress(obj.Ip);
                int i = obj.Insert();
                if (i > 0)
                {
                    rh.msg = "添加记录成功";
                    rh.totals = i;
                }
                else
                {
                    rh.msg = "添加记录失败";
                    rh.code = 400;
                }
            }
            catch (Exception e)
            {
                rh.code = 500;
                rh.msg = "服务器错误，请通知管理员";
            }

            return ReturnJson(JsonConvert.SerializeObject(rh));
        }

        /// <summary>
        /// 更新文章内容
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage UpdateEssay(Essays obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                obj.Update_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                obj.Ip = IPHelper.GetHostIP();
                obj.Address = IPHelper.GetHostAddress(obj.Ip);
                int i = obj.Update(" Essayid=@Essayid", obj.Essayid);
                if (i > 0)
                {
                    rh.msg = "更新文章成功";
                    rh.totals = i;
                }
                else
                {
                    rh.msg = "更新文章失败";
                    rh.code = 400;
                }
            }
            catch (Exception e)
            {
                rh.code = 500;
                rh.msg = "服务器错误，请通知管理员";
            }

            return ReturnJson(JsonConvert.SerializeObject(rh));
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage DeleteEssay(Essays obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                int i = obj.Delete(" Essayid=@Essayid", obj.Essayid);
                if (i > 0)
                {
                    rh.msg = "删除文章成功";
                    rh.totals = i;
                }
                else
                {
                    rh.msg = "删除文章失败";
                    rh.code = 400;
                }
            }
            catch (Exception e)
            {
                rh.code = 500;
                rh.msg = "服务器错误，请通知管理员";
            }

            return ReturnJson(JsonConvert.SerializeObject(rh));
        }

        /// <summary>
        /// 查看文章
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage ViewEssay(Essays obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                string selectsql = string.Format(" select Views from Essays where Essayid='{0}'", obj.Essayid);
                string times = SqlHelper.ShowData(selectsql, "Views", SqlHelper.CreateConn());
                if (!string.IsNullOrEmpty(times))
                {
                    obj.Views = Convert.ToInt32(times) + 1;//访问次数+1
                }
                else
                {
                    obj.Views = 1;//第一次访问
                }
                int i = obj.Update(" Essayid=@Essayid", obj.Essayid);
                if (i > 0)
                {
                    rh.msg = "查看成功";
                    rh.totals = i;
                }
                else
                {
                    rh.msg = "查看次数更新失败";
                    rh.code = 400;
                }
            }
            catch (Exception e)
            {
                rh.code = 500;
                rh.msg = "服务器错误，请通知管理员";
            }

            return ReturnJson(JsonConvert.SerializeObject(rh));
        }
    }
}
