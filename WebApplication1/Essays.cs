using System;

namespace DAL
{ 
     public class Essays
     { 
         public Essays()
         {
             Conn = "Default";
         }

         public Essays(string conn)
          {
              Conn =conn;
          }

         /// <summary>
         /// 数据库连接字符串KEY
         /// </summary>
         public string Conn{get;set;}

         /// <summary>
         /// 文章id
         /// </summary>
         public string Essayid{get;set;}

         /// <summary>
         /// 文章类别
         /// </summary>
         public string Essay_Type{get;set;}

         /// <summary>
         /// 文章标题
         /// </summary>
         public string Essay_Title{get;set;}

         /// <summary>
         /// 文章标签，用,隔开
         /// </summary>
         public string Essay_Label{get;set;}

         /// <summary>
         /// 文章内容
         /// </summary>
         public string Essay_Content{get;set;}

         /// <summary>
         /// 备注
         /// </summary>
         public string Remark{get;set;}

         /// <summary>
         /// 发表时间
         /// </summary>
         public string Report_Time{get;set;}

         /// <summary>
         /// 浏览次数
         /// </summary>
         public int? Views{get;set;}

         /// <summary>
         /// 最新更新时间
         /// </summary>
         public string Update_Time{get;set;}

         /// <summary>
         /// 发布地ip
         /// </summary>
         public string Ip{get;set;}

         /// <summary>
         /// 发布地
         /// </summary>
         public string Address{get;set;}

     }
} 

