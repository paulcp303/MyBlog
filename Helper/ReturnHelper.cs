using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Helper
{
    public class ReturnHelper
    {
        public int code { get; set; }
        public DataTable data { get; set; }
        public int totals { get; set; }
        public string msg { get; set; }

        /// <summary>
        /// json返回帮助类;返回码（200正常，500出错，300缺少参数,400数据库执行问题）
        /// </summary>
        /// <param name="code">返回码（200正常，500出错，300缺少参数,400数据库执行问题）</param>
        /// <param name="data"></param>
        /// <param name="totals"></param>
        /// <param name="msg"></param>
        /// <returns>ss</returns>
        public ReturnHelper(int code, DataTable data, int totals, string msg)
        {
            this.code = code;
            this.data = data;
            this.totals = totals;
            this.msg = msg;
        }
    }
}