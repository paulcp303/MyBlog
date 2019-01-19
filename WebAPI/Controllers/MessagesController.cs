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
    public class MessagesController : ApiController
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
        public HttpResponseMessage GetMessagesPage()
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
                    Messages obj = new Messages();
                    string strWhere = " 1=1";
                    if (!string.IsNullOrEmpty(key))
                    {
                        strWhere += string.Format(" and (Name like '%{0}%' or Message_Content like '%{0}%' or Ip like '%{0}%')", key);
                    }
                    int begin = (Convert.ToInt32(page) - 1) * Convert.ToInt32(limit);
                    int end = Convert.ToInt32(page) * Convert.ToInt32(limit);
                    DataTable dt = obj.GetPage("*", "Datetime desc", strWhere, begin, end);
                    if (dt.Rows.Count > 0)
                    {
                        rh.totals = SqlHelper.Count(string.Format("select * from Messages where {0}", strWhere), SqlHelper.CreateConn());
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
        public HttpResponseMessage AddMessage(Messages obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                obj.Message_Id = Guid.NewGuid().ToString("N");
                obj.Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                obj.Ip = IPHelper.GetHostIP();
                obj.Address = IPHelper.GetHostAddress(obj.Ip);
                int i = obj.Insert();
                if (i > 0)
                {
                    rh.msg = "添加留言成功";
                    rh.totals = i;
                }
                else
                {
                    rh.msg = "添加留言失败";
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
        public HttpResponseMessage UpdateMessage(Messages obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                obj.Ip = IPHelper.GetHostIP();
                obj.Address = IPHelper.GetHostAddress(obj.Ip);
                int i = obj.Update(" Message_Id=@Message_Id", obj.Message_Id);
                if (i > 0)
                {
                    rh.msg = "更新留言成功";
                    rh.totals = i;
                }
                else
                {
                    rh.msg = "更新留言失败";
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
        public HttpResponseMessage DeleteMessage(Messages obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                int i = obj.Delete(" Message_Id=@Message_Id", obj.Message_Id);
                if (i > 0)
                {
                    rh.msg = "删除留言成功";
                    rh.totals = i;
                }
                else
                {
                    rh.msg = "删除留言失败";
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
