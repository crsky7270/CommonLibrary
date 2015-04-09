using System;
using System.Configuration;
using System.Web;

public class CookieRelated
{
    // Fields
    private static int _exprise = 10;
    public static readonly string Domain = ConfigurationManager.AppSettings["Domain"];

    // Methods
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

    public static void DeleteCookies(string strCookiesName, string strName)
    {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[strCookiesName];
        if (cookie != null)
        {
            cookie.Values.Remove(strName);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
    }

    public static string GetCookie(string strName)
    {
        if (HttpContext.Current.Request.Cookies[strName] != null)
        {
            return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies[strName].Value);
        }
        return string.Empty;
    }

    public static bool IsCookies(string strCookiesName)
    {
        return (HttpContext.Current.Request.Cookies[strCookiesName] == null);
    }

    public static string ReadCookies(string strCookieName, string strName)
    {
        if (HttpContext.Current.Request.Cookies[strCookieName] != null)
        {
            return HttpContext.Current.Request.Cookies[strCookieName][strName];
        }
        return "";
    }

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
        cookie.Expires = DateTime.Now.AddDays((double) expiresDays);
        HttpContext.Current.Response.AppendCookie(cookie);
    }

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
                cookie.Expires = DateTime.Now.AddMinutes((double) expires);
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

 
 
