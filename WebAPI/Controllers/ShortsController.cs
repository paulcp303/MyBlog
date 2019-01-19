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
    public class ShortsController : ApiController
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
        /// 获取微语列表
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetShortsPage()
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
                    Shorts obj = new Shorts();
                    string strWhere = " 1=1";
                    if (!string.IsNullOrEmpty(key))
                    {
                        strWhere += string.Format(" and (Short_Content like '%{0}%')", key);
                    }
                    int begin = (Convert.ToInt32(page) - 1) * Convert.ToInt32(limit);
                    int end = Convert.ToInt32(page) * Convert.ToInt32(limit);
                    DataTable dt = obj.GetPage("*", "Report_Time desc", strWhere, begin, end);
                    if (dt.Rows.Count > 0)
                    {
                        rh.totals = SqlHelper.Count(string.Format("select * from Shorts where {0}", strWhere), SqlHelper.CreateConn());
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
        /// 发表微语
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage AddShort(Shorts obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                obj.Shortid = Guid.NewGuid().ToString("N");
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
        /// 更新微语内容
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage UpdateShort(Shorts obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                obj.Update_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                obj.Ip = IPHelper.GetHostIP();
                obj.Address = IPHelper.GetHostAddress(obj.Ip);
                int i = obj.Update(" Shortid=@Shortid", obj.Shortid);
                if (i > 0)
                {
                    rh.msg = "更新微语成功";
                    rh.totals = i;
                }
                else
                {
                    rh.msg = "更新微语失败";
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
        /// 删除微语
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage DeleteShort(Shorts obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                int i = obj.Delete(" Shortid=@Shortid", obj.Shortid);
                if (i > 0)
                {
                    rh.msg = "删除微语成功";
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
        /// 查看微语
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage ViewShort(Shorts obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                string selectsql = string.Format(" select Views from Shorts where Shortid='{0}'", obj.Shortid);
                string times = SqlHelper.ShowData(selectsql, "Views", SqlHelper.CreateConn());
                if (!string.IsNullOrEmpty(times))
                {
                    obj.Views = Convert.ToInt32(times) + 1;//访问次数+1
                }
                else
                {
                    obj.Views = 1;//第一次访问
                }
                int i = obj.Update(" Shortid=@Shortid", obj.Shortid);
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

        /// <summary>
        /// 点赞微语
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage LikeShort(Shorts obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                string selectsql = string.Format(" select Likes from Shorts where Shortid='{0}'", obj.Shortid);
                string likes = SqlHelper.ShowData(selectsql, "Likes", SqlHelper.CreateConn());
                if (!string.IsNullOrEmpty(likes))
                {
                    obj.Likes = Convert.ToInt32(likes) + 1;//访问次数+1
                }
                else
                {
                    obj.Likes = 1;//第一次访问
                }
                int i = obj.Update(" Shortid=@Shortid", obj.Shortid);
                if (i > 0)
                {
                    rh.msg = "点赞成功";
                    rh.totals = i;
                }
                else
                {
                    rh.msg = "点赞失败";
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
