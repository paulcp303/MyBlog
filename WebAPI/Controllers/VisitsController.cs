using DAL;
using Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class VisitsController : ApiController
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
        /// 添加访问记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage AddVisitRecord(Visits obj)
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");

            try
            {
                obj.Visitid = Guid.NewGuid().ToString("N");
                obj.Visittime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                obj.Ip = IPHelper.GetHostIP();
                obj.Address = IPHelper.GetHostAddress(obj.Ip);
                string selectsql = string.Format(" select Times from Visits where IP='{0}'",obj.Ip);
                string times = SqlHelper.ShowData(selectsql, "Times", SqlHelper.CreateConn());
                if (!string.IsNullOrEmpty(times))
                {
                    obj.Times = Convert.ToInt32(times) + 1;//访问次数+1
                }
                else
                {
                    obj.Times =  1;//第一次访问
                }
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

        public HttpResponseMessage GetVisitsPage()
        {
            ReturnHelper rh = new ReturnHelper(200, null, 0, "");
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

                }
            }
            catch (Exception e)
            {
                return ReturnJson(JsonConvert.SerializeObject(rh));
            }

            return ReturnJson(JsonConvert.SerializeObject(rh));
        }
    }
}
