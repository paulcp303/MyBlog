using System;

namespace DAL
{ 
     public class Ecomments
     { 
         public Ecomments()
         {
             Conn = "Default";
         }

         public Ecomments(string conn)
          {
              Conn =conn;
          }

         /// <summary>
         /// 数据库连接字符串KEY
         /// </summary>
         public string Conn{get;set;}

         /// <summary>
         /// 文章评论id
         /// </summary>
         public string Ecomment_Id{get;set;}

         /// <summary>
         /// 文章id
         /// </summary>
         public string Essay_Id{get;set;}

         /// <summary>
         /// 评论内容
         /// </summary>
         public string Essay_Content{get;set;}

         /// <summary>
         /// 
         /// </summary>
         public string Ip{get;set;}

         /// <summary>
         /// 昵称
         /// </summary>
         public string Name{get;set;}

         /// <summary>
         /// 被评论内容id，可为空
         /// </summary>
         public string To_Ecomment_Id{get;set;}

         /// <summary>
         /// 评论时间
         /// </summary>
         public string Time{get;set;}

     }
} 

