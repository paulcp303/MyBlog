using System;

namespace DAL
{ 
     public class Visits
     { 
         public Visits()
         {
             Conn = "Default";
         }

         public Visits(string conn)
          {
              Conn =conn;
          }

         /// <summary>
         /// 数据库连接字符串KEY
         /// </summary>
         public string Conn{get;set;}

         /// <summary>
         /// 访问id
         /// </summary>
         public string Visitid{get;set;}

         /// <summary>
         /// 访问id
         /// </summary>
         public string Ip{get;set;}

         /// <summary>
         /// 访问地址
         /// </summary>
         public string Address{get;set;}

         /// <summary>
         /// 访问时间
         /// </summary>
         public string Visittime{get;set;}

         /// <summary>
         /// 访问次数
         /// </summary>
         public int? Times{get;set;}

         /// <summary>
         /// 来自国家
         /// </summary>
         public string Contry{get;set;}

         /// <summary>
         /// 访问浏览器
         /// </summary>
         public string Browser{get;set;}

     }
} 

