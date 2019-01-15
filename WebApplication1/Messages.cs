using System;

namespace DAL
{ 
     public class Messages
     { 
         public Messages()
         {
             Conn = "Default";
         }

         public Messages(string conn)
          {
              Conn =conn;
          }

         /// <summary>
         /// 数据库连接字符串KEY
         /// </summary>
         public string Conn{get;set;}

         /// <summary>
         /// 留言id
         /// </summary>
         public string Message_Id{get;set;}

         /// <summary>
         /// 昵称
         /// </summary>
         public string Name{get;set;}

         /// <summary>
         /// 留言内容
         /// </summary>
         public string Message_Content{get;set;}

         /// <summary>
         /// 留言时间
         /// </summary>
         public string Datetime{get;set;}

         /// <summary>
         /// 回复留言id
         /// </summary>
         public string To_Message_Id{get;set;}

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

