using System;

namespace DAL
{ 
     public class Shorts
     { 
         public Shorts()
         {
             Conn = "Default";
         }

         public Shorts(string conn)
          {
              Conn =conn;
          }

         /// <summary>
         /// 数据库连接字符串KEY
         /// </summary>
         public string Conn{get;set;}

         /// <summary>
         /// 微语id
         /// </summary>
         public string Shortid{get;set;}

         /// <summary>
         /// 微语内容
         /// </summary>
         public string Short_Content{get;set;}

         /// <summary>
         /// 微语图片url
         /// </summary>
         public string Short_Imgs{get;set;}

         /// <summary>
         /// 发表时间
         /// </summary>
         public string Report_Time{get;set;}

         /// <summary>
         /// 点赞数
         /// </summary>
         public int? Likes{get;set;}

         /// <summary>
         /// 浏览次数
         /// </summary>
         public int? Views{get;set;}

         /// <summary>
         /// 最新更新时间
         /// </summary>
         public string Update_Time{get;set;}

         /// <summary>
         /// 备注
         /// </summary>
         public string Remark{get;set;}

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

