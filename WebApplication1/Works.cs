using System;

namespace DAL
{ 
     public class Works
     { 
         public Works()
         {
             Conn = "Default";
         }

         public Works(string conn)
          {
              Conn =conn;
          }

         /// <summary>
         /// 数据库连接字符串KEY
         /// </summary>
         public string Conn{get;set;}

         /// <summary>
         /// 作品id
         /// </summary>
         public string Work_Id{get;set;}

         /// <summary>
         /// 作品分类
         /// </summary>
         public string Work_Type{get;set;}

         /// <summary>
         /// 作品标题
         /// </summary>
         public string Work_Title{get;set;}

         /// <summary>
         /// 作品封面图片url
         /// </summary>
         public string Work_Back{get;set;}

         /// <summary>
         /// 作品内容
         /// </summary>
         public string Work_Content{get;set;}

         /// <summary>
         /// 作品链接（下载地址）
         /// </summary>
         public string Work_Url{get;set;}

         /// <summary>
         /// 作品时间
         /// </summary>
         public string Work_Time{get;set;}

         /// <summary>
         /// 发表时间
         /// </summary>
         public string Report_Time{get;set;}

         /// <summary>
         /// 最新更新时间
         /// </summary>
         public string Update_Time{get;set;}

         /// <summary>
         /// 评论
         /// </summary>
         public string Remark{get;set;}

         /// <summary>
         /// 浏览次数
         /// </summary>
         public int? Views{get;set;}

     }
} 

