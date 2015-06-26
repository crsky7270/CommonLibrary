using System.Web;
using System.Text;

namespace Crsky.Utility.Helper
{
    /// <summary>
    /// URL相关
    /// </summary>
    public class UrlHelper
    {
        #region URL相关

        #region 获取前一个页面的URL地址
        /// <summary>
        /// 获取前一个页面的URL地址
        /// </summary>
        /// <returns>前一个页面的URL地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = string.Empty;

            try
            {
                retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch
            {
            }
            return retVal;
        }
        #endregion

        #region 获取主机头
        /// <summary>
        /// 获取主机头
        /// </summary>
        /// <returns>当前主机头</returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }

        /// <summary>
        /// 获取完整主机头
        /// </summary>
        /// <returns>当前完整主机头</returns>
        public static string GetFullHost()
        {
            var request = HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
            {
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
            }
            return request.Url.Host;
        }
        #endregion

        #region 获取当前请求的原始 URL
        /// <summary>
        /// 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询参数(如果存在))
        /// </summary>
        /// <returns>原始URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }
        #endregion

        #region 获得当前URL完整地址
        /// <summary>
        /// 获得当前URL完整地址
        /// </summary>
        /// <returns>当前URL完整地址</returns>
        public static string GetUrl()
        {

            return HttpContext.Current.Request.Url.PathAndQuery;
        }
        #endregion

        #region 获得当前URL的页面名称
        /// <summary>
        /// 获得当前URL的页面名称
        /// </summary>
        /// <returns>当前URL的页面名称</returns>
        public static string GetPageName()
        {
            string[] arrUrl = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return arrUrl[arrUrl.Length - 1].ToLower();
        }
        #endregion

        #region URL 编/解码 
        /// <summary>
        /// 使用UTF-8编码对URL编码，对URL参数编码时请一定使用此函数
        /// <remqrk>
        /// HttpUtility.UrlEncode内部默认使用UTF-8编码，为支持中文，在 Encode 的时候， 
        /// 将空格转换成加号('+'), 在 Decode 的时候将加号转为空格
        /// </remqrk>
        /// </summary>
        /// <param name="str">要编码的URL字符串</param>
        /// <returns>编码后的URL字符串</returns>
        public static string UrlEncode(string str)
        {
            //return HttpUtility.UrlEncode(str).Replace("+", "%20");
            return UrlEncode(str, null);
        }
        /// <summary>
        /// 使用指定字符编码对URL编码
        /// </summary>
        /// <param name="paramName">URL参数</param>
        /// <param name="encoding">字符编码</param>
        /// <returns>编码后的URL参数</returns>
        public static string UrlEncode(string str, Encoding encoding)
        {
            if (encoding == null || encoding == Encoding.UTF8) 
            {
                //return UrlEncode(str);
                return HttpUtility.UrlEncode(str).Replace("+", "%20");
            }
            else
            {
                return HttpUtility.UrlEncode(str, encoding);
            }
        }
        /// <summary>
        /// 使用UTF-8编码对URL解码，对URL参数解码时请一定使用此函数
        /// <remqrk>
        /// HttpUtility.UrlEncode内部默认使用UTF-8编码，为支持中文，在 Encode 的时候，
        /// 将空格转换成加号('+'), 在 Decode 的时候将加号转为空格
        /// </remqrk>
        /// </summary>
        /// <param name="str">要解码的URL字符串</param>
        /// <returns>解码后的URL字符串</returns>
        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }
        /// <summary>
        /// 使用指定字符编码对URL解码
        /// </summary>
        /// <param name="paramName">URL参数</param>
        /// <param name="encoding">字符编码</param>
        /// <returns>解码后的URL参数</returns>
        public static string UrlDecode(string str, Encoding encoding)
        {
            return HttpUtility.UrlDecode(str, encoding);
        }
        #endregion


        #endregion
    }
}
