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
    public class WorksController : ApiController
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
        public HttpResponseMessage GetWorksPage()
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
                    Works obj = new Works();
                    string strWhere = " 1=1";
                    if (!string.IsNullOrEmpty(key))
                    {
                        strWhere += string.Format(" and (Work_Type like '%{0}%' or Work_Title like '%{0}%')", key);
                    }
                    int begin = (Convert.ToInt32(page) - 1) * Convert.ToInt32(limit);
                    int end = Convert.ToInt32(page) * Convert.ToInt32(limit);
                    DataTable dt = obj.GetPage("*", "Report_Time desc", strWhere, begin, end);
                    if (dt.Rows.Count > 0)
                    {
                        rh.totals = SqlHelper.Count(string.Format("select * from Works where {0}", strWhere), SqlHelper.CreateConn());
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
        public HttpResponseMessage AddWork(Works obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                obj.Work_Id = Guid.NewGuid().ToString("N");
                obj.Report_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                int i = obj.Insert();
                if (i > 0)
                {
                    rh.msg = "添加作品成功";
                    rh.totals = i;
                }
                else
                {
                    rh.msg = "添加作品失败";
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
        public HttpResponseMessage UpdateWork(Works obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                obj.Update_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                int i = obj.Update(" Work_Id=@Work_Id", obj.Work_Id);
                if (i > 0)
                {
                    rh.msg = "更新作品成功";
                    rh.totals = i;
                }
                else
                {
                    rh.msg = "更新作品失败";
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
        public HttpResponseMessage DeleteWork(Works obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                int i = obj.Delete(" Work_Id=@Work_Id", obj.Work_Id);
                if (i > 0)
                {
                    rh.msg = "删除作品成功";
                    rh.totals = i;
                }
                else
                {
                    rh.msg = "删除作品失败";
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
