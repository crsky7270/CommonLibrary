using System.Collections.Specialized;
using System.Web;

namespace Crsky.Utility.Helper
{
    /// <summary>
    /// HTTP请求相关
    /// </summary>
    public class RequestHelper
    {
        #region 获取当前HTTP请求变量集合

        /// <summary>
        /// 获取服务器变量集合
        /// </summary>
        /// <returns>存储服务器变量集合的NameValueCollection</returns>
        public static NameValueCollection ServerVariables()
        {
            var request = HttpContext.Current.Request;
            return FaultIn(GetCollection(request.ServerVariables));
        }
        
        /// <summary>
        /// 获取查询参数变量集合
        /// </summary>
        /// <returns>存储参数变量集合的NameValueCollection</returns>
        public static NameValueCollection QueryString()
        {
            var request = HttpContext.Current.Request;
            return FaultIn(GetCollection(request.QueryString));
        }
        
        /// <summary>
        /// 获取表单变量集合
        /// </summary>
        /// <returns>存储表单变量集合的NameValueCollection</returns>
        public static NameValueCollection Form()
        {
            var request = HttpContext.Current.Request;
            return FaultIn(GetCollection(request.Form));
        }
        
        /// <summary>
        /// 获取客户端Cookies集合
        /// <remarks>出于简单考虑，去掉了cookie的Path和Domain属性
        /// </remarks>
        /// </summary>
        /// <returns>存储客户端Cookies集合的NameValueCollection</returns>
        public static NameValueCollection Cookies()
        {
            var request = HttpContext.Current.Request;
            return FaultIn(GetCollection(request.Cookies));
        }

        private static NameValueCollection GetCollection(NameValueCollection collection)
        {
            if (collection == null || collection.Count == 0)
                return null;

            return new NameValueCollection(collection);
        }

        private static NameValueCollection GetCollection(HttpCookieCollection cookies)
        {
            if (cookies == null || cookies.Count == 0)
                return null;

            NameValueCollection col = new NameValueCollection(cookies.Count);
            for (int i = 0; i < cookies.Count; i++)
            {
                HttpCookie cookie = cookies[i];
                //出于简单考虑，去掉了cookie的Path和Domain属性
                col.Add(cookie.Name, cookie.Value);
            }
            return col;
        }
        
        private static NameValueCollection FaultIn(NameValueCollection collection)
        {
            if (collection == null)
                collection = new NameValueCollection();

            return collection;
        }

        #endregion

        #region 获取[服务器变量/查询参数/表单/Cookie]中指定参数名的值

        /// <summary>
        /// 获取[服务器变量/查询参数/表单/Cookie]中指定参数名的值
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <returns>指定参数名的值，缺省值为string.Empty</returns>
        public static string Params(string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
                return string.Empty;

            return Params(paramName, string.Empty);
        }
        /// <summary>
        /// 获取[服务器变量/查询参数/表单/Cookie]中指定参数名的值
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>指定参数名的值</returns>
        public static string Params(string paramName, string defaultValue)
        {
            var request = HttpContext.Current.Request;
            return Params(request, paramName, defaultValue);
        }
        /// <summary>
        /// 获取[服务器变量/查询参数/表单/Cookie]中指定参数名的值
        /// </summary>
        /// <param name="request">上下文请求</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>指定参数名的值</returns>
        public static string Params(HttpRequest request, string paramName, string defaultValue)
        {
            if (string.IsNullOrEmpty(request.Params[paramName]))
                return defaultValue;

            return request.Params[paramName].Trim();
        }

        #endregion

        #region 获取服务器变量中指定名称的值

        /// <summary>
        /// 获取服务器变量中指定名称的值
        /// </summary>
        /// <param name="paramName">服务器变量名称</param>
        /// <returns>指定服务器变量名的值，缺省值为string.Empty</returns>
        public static string ServerVariables(string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
                return string.Empty;

            return ServerVariables(paramName, string.Empty);
        }
        /// <summary>
        /// 获取服务器变量中指定名称的值
        /// </summary>
        /// <param name="paramName">服务器变量名称</param>
        /// <returns>指定服务器变量名的值，缺省值为string.Empty</returns>
        public static string ServerVariables(string paramName, string defaultValue)
        {
            var request = HttpContext.Current.Request;
            return ServerVariables(request, paramName, defaultValue);
        }
        /// <summary>
        /// 获取服务器变量中指定名称的值
        /// </summary>
        /// <param name="paramName">服务器变量名称</param>
        /// <returns>指定服务器变量名的值，缺省值为string.Empty</returns>
        public static string ServerVariables(HttpRequest request, string paramName, string defaultValue)
        {
            if (request.ServerVariables[paramName] == null)
            {
                return defaultValue;
            }
            return request.ServerVariables[paramName].ToString().Trim();
        }

        #endregion

        #region 获取查询参数中指定名称的值

        /// <summary>
        /// 获取查询参数中指定名称的值
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <returns>指定参数名的值，缺省值为string.Empty</returns>
        public static string QueryString(string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
                return string.Empty;

            return QueryString(paramName, string.Empty);
        }
        /// <summary>
        /// 获取查询参数中指定名称的值
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>指定参数名的值</returns>
        public static string QueryString(string paramName, string defaultValue)
        {
            var request = HttpContext.Current.Request;
            return QueryString(request, paramName, defaultValue);

        }
        /// <summary>
        /// 获取查询参数中指定名称的值
        /// </summary>
        /// <param name="request">上下文请求</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>指定参数名的值</returns>
        public static string QueryString(HttpRequest request, string paramName, string defaultValue)
        {
            if (request.QueryString[paramName] == null)
            {
                return defaultValue;
            }
            return request.QueryString[paramName];
        }

        #endregion

        #region 获取表单变量中指定名称的值

        /// <summary>
        /// 获取表单变量中指定名称的值
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <returns>指定表单变量名的值，缺省值为string.Empty</returns>
        public static string Form(string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
                return string.Empty;

            return Form(paramName, string.Empty);
        }
        /// <summary>
        /// 获取表单变量中指定名称的值
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>指定表单变量名的值</returns>
        public static string Form(string paramName, string defaultValue)
        {
            var request = HttpContext.Current.Request;
            return Form(request, paramName, defaultValue);

        }
        /// <summary>
        /// 获取表单变量中指定名称的值
        /// </summary>
        /// <param name="request">上下文请求</param>
        /// <param name="paramName">表单变量名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>指定表单变量名的值</returns>
        public static string Form(HttpRequest request, string paramName, string defaultValue)
        {
            if (request.Form[paramName] == null)
            {
                return defaultValue;
            }
            return request.Form[paramName];
        }

        #endregion

        #region 获取客户端Cookies中指定名称的值

        /// <summary>
        /// 获取客户端Cookies集合中指定名称的Cookie数据
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="key">指定Cookie名称下Cookie键名(为空则返回以＆分隔的Cookie集合)</param>
        /// <returns>指定Cookie名称下Cookie键名的值，缺省值为string.Empty</returns>
        public static string Cookies(string name, string key)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            var request = HttpContext.Current.Request;
            NameValueCollection col = Cookies();
            string value = col.Get(name);

            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (string.IsNullOrEmpty(key))
                return value;

            return GetCookieParam(value, key);
        }
        /// <summary>
        /// 获取客户端Cookie中指定键名的值
        /// </summary>
        /// <param name="name">Cookie集合(以&分隔)</param>
        /// <param name="key">Cookie键名</param>
        /// <returns>指定Cookie键名的值，缺省值为string.Empty</returns>
        private static string GetCookieParam(string name, string key)
        {
            string paramValue = string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                string[] cookieParams = name.Split('&');
                if (cookieParams.Length > 0)
                {
                    foreach (string param in cookieParams)
                    {
                        int index = param.IndexOf('=');
                        if (index != -1)
                        {
                            string newName = param.Substring(0, index).Trim();
                            if (newName.Equals(key))
                            {
                                if (param.Length > index + 1)
                                {
                                    paramValue = param.Substring(index + 1, param.Length - index - 1).Trim();
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return paramValue;
        }

        #endregion

        #region 判断当前页面是否接收到了 Post/Get 请求

        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }
        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }

        #endregion
    }
}
