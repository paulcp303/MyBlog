using System;

namespace DAL
{ 
     public class Scomments
     { 
         public Scomments()
         {
             Conn = "Default";
         }

         public Scomments(string conn)
          {
              Conn =conn;
          }

         /// <summary>
         /// 数据库连接字符串KEY
         /// </summary>
         public string Conn{get;set;}

         /// <summary>
         /// 微语评论id
         /// </summary>
         public string Scomment_Id{get;set;}

         /// <summary>
         /// 微语id
         /// </summary>
         public string Short_Id{get;set;}

         /// <summary>
         /// 微语内容
         /// </summary>
         public string Short_Content{get;set;}

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
         public string To_Scomment_Id{get;set;}

         /// <summary>
         /// 评论时间
         /// </summary>
         public string Time{get;set;}

     }
} 

