using System;
using System.Configuration;
using System.Web;

public class CookieRelated
{
   // Fields
   private static int _exprise = 10;
   public static readonly string Domain = ConfigurationManager.AppSettings["Domain"];

   /// <summary>
   /// 删除Cookies,使其过期的方式
   /// </summary>
   /// <param name="strCookiesName">key Name</param>
   public static void DeleteCookies(string strCookiesName)
   {
      HttpCookie cookie = HttpContext.Current.Request.Cookies[strCookiesName];
      if (cookie != null)
      {
         TimeSpan span = new TimeSpan(-1, 0, 0, 0);
         cookie.Expires = DateTime.Now.Add(span);
         HttpContext.Current.Response.AppendCookie(cookie);
      }
   }

   /// <summary>
   /// 删除Cookies
   /// </summary>
   /// <param name="strCookiesName"></param>
   /// <param name="strName"></param>
   public static void DeleteCookies(string strCookiesName, string strName)
   {
      HttpCookie cookie = HttpContext.Current.Request.Cookies[strCookiesName];
      if (cookie != null)
      {
         cookie.Values.Remove(strName);
         HttpContext.Current.Response.AppendCookie(cookie);
      }
   }

   /// <summary>
   /// 获取Cookies的值
   /// </summary>
   /// <param name="strName"></param>
   /// <returns></returns>
   public static string GetCookie(string strName)
   {
      if (HttpContext.Current.Request.Cookies[strName] != null)
      {
         return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies[strName].Value);
      }
      return string.Empty;
   }

   /// <summary>
   /// 判断Cookies是否存在
   /// </summary>
   /// <param name="strCookiesName"></param>
   /// <returns></returns>
   public static bool IsCookies(string strCookiesName)
   {
      return (HttpContext.Current.Request.Cookies[strCookiesName] == null);
   }

   /// <summary>
   /// 读取Cookies
   /// </summary>
   /// <param name="strCookieName"></param>
   /// <param name="strName"></param>
   /// <returns></returns>
   public static string ReadCookies(string strCookieName, string strName)
   {
      if (HttpContext.Current.Request.Cookies[strCookieName] != null)
      {
         return HttpContext.Current.Request.Cookies[strCookieName][strName];
      }
      return "";
   }

   /// <summary>
   /// 设置Cookies的相关参数
   /// </summary>
   /// <param name="name"></param>
   /// <param name="value"></param>
   /// <param name="expiresDays"></param>
   public static void SetCookie(string name, string value, int expiresDays)
   {
      HttpCookie cookie;
      if (HttpContext.Current.Request.Cookies[name] == null)
      {
         cookie = new HttpCookie(name);
      }
      else
      {
         cookie = HttpContext.Current.Request.Cookies[name];
      }
      cookie.Domain = Domain;
      cookie.Value = HttpUtility.UrlEncode(value);
      cookie.Expires = DateTime.Now.AddDays((double)expiresDays);
      HttpContext.Current.Response.AppendCookie(cookie);
   }

   /// <summary>
   /// 更新Cookies
   /// </summary>
   /// <param name="strCookieName"></param>
   /// <param name="strName"></param>
   /// <param name="strValue"></param>
   public static void UpdateCookies(string strCookieName, string strName, string strValue)
   {
      if (!string.IsNullOrEmpty(strCookieName))
      {
         HttpCookie cookie = HttpContext.Current.Request.Cookies[strCookieName];
         if (cookie != null)
         {
            cookie.Values.Set(strName, strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
         }
      }
   }

   public static void WriteCookies(string cookiesName, string strName, string strValue)
   {
      WriteCookies(cookiesName, strName, strValue, Exprise, false);
   }

   public static void WriteCookies(string CookiesName, string strName, string strValue, int expires, bool isAdd)
   {
      if (!string.IsNullOrEmpty(CookiesName) && !string.IsNullOrEmpty(strName))
      {
         HttpCookie cookie = HttpContext.Current.Request.Cookies[CookiesName];
         if (cookie == null)
         {
            cookie = new HttpCookie(CookiesName);
         }
         if (!isAdd)
         {
            cookie.Expires = DateTime.Now.AddMinutes((double)expires);
         }
         else
         {
            cookie.Expires = DateTime.MaxValue;
         }
         cookie.Domain = Domain;
         cookie.Values.Add(strName, strValue);
         HttpContext.Current.Response.AppendCookie(cookie);
      }
   }

   // Properties
   public static int Exprise
   {
      get
      {
         return _exprise;
      }
      set
      {
         _exprise = value;
      }
   }
}



