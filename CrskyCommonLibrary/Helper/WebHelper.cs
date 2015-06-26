/*----------------------------------------------------------------
 * 
 * Copyright 
 * 
 * File Name   ：WebRelated.cs
 * Description ：web相关工具
 * 
 * Create  ：
 * Modify  ：
 * History ：
 *----------------------------------------------------------------*/

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Net;
using System.IO;

namespace Crsky.Utility.Helper
{
    public static class WebHelper
    {
        private const string defaultCodingType = "utf-8";
        public static int TimeOut = 15;
        private const string PictureVirtualDirectory = "PictureVirtualDirectory";

        /// <summary>
        /// 获取客户端Ip
        /// </summary>
        /// <param name="page">页面Page</param>
        /// <returns></returns>
        public static string GetClientIP(System.Web.UI.Page page)
        {
            string userIp;
            if (page == null)
            {
                return "127.0.0.1"; //throw new WebcarsException();
            }
            userIp = page.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (userIp == null || !userIp.Contains("."))
            {
                userIp = page.Request.ServerVariables["REMOTE_ADDR"];
            }
            else if (userIp.Contains(","))
            {
                userIp = userIp.Substring(0, userIp.IndexOf(","));
            }

            return userIp;
        }

        /// <summary>
        /// 获取客户端Ip
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            string userIp;

            userIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (userIp == null || !userIp.Contains("."))
            {
                userIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else if (userIp.Contains(","))
            {
                userIp = userIp.Substring(0, userIp.IndexOf(","));
            }

            return userIp;
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip">Ip地址</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        }

        /// <summary>
        /// 直接获取客户端数字IP
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static long GetClientIPNumber(System.Web.UI.Page page)
        {
            return ConvertIPToLong(GetClientIP(page));
        }

        /// <summary>
        /// 将ip转为数字格式
        /// </summary>
        /// <param name="Ip"></param>
        /// <returns></returns>
        public static long ConvertIPToLong(string Ip)
        {
            long longResult = 0;
            try
            {
                //取出IP地址去掉‘.’后的string数组
                string[] Ip_List = Ip.Split('.');
                string X_Ip = "";
                //循环数组，把数据转换成十六进制数，并合并数组(3dafe81e)
                foreach (string ip in Ip_List)
                {
                    X_Ip += Convert.ToByte(ip).ToString("x").PadLeft(2, '0');
                }

                //将十六进制数转换成十进制数(1034938398)
                longResult = long.Parse(X_Ip, System.Globalization.NumberStyles.HexNumber);
            }
            catch
            {
                //设置的IP范围异常
            }

            return longResult;
        }


        /// <summary>
        /// 对要直接输出到页面的文本中的Html标记进行编码
        /// add by 岳维航
        /// 一个空格替换为两个标记是为了正常显示英文
        /// </summary>
        /// <param name="text">需要编码的文本</param>
        /// <returns></returns>
        public static string EncodeXmlText(string text)
        {
            if (text == null)
                return "";

            text = text.Replace("&nbsp;", " ");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("\'", "&apos;");
            text = text.Replace("\r", "").Replace("\n", "<br />");
            //text = text.Replace("&", "&amp;");   
            return text;
        }

        ///  <summary>
        /// 分析用户请求是否正常
        ///  </summary>
        ///  <param name="str">用户提交的数据 </param>
        ///  <returns>返回是否含有SQL注入式攻击代码 </returns>
        public static bool ProcessSqlStr(ref string str)
        {
            bool ReturnValue = true;
            try
            {
                if (str.Trim() != "")
                {
                    string SqlStr = "'|and|exec|insert|select|delete|update|count|*|chr|mid|master|truncate|char|declare";

                    string[] anySqlStr = SqlStr.Split('|');
                    foreach (string ss in anySqlStr)
                    {
                        if (str.ToLower().IndexOf(ss) >= 0)
                        {
                            str = str.Replace(ss, "");
                        }
                    }
                }
                str = HttpContext.Current.Server.HtmlEncode(str);
            }
            catch
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }

        /// <summary>
        /// 去掉HTML标签
        /// </summary>
        /// <param name="str">HTML内容</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string RemoveHtmlTag(string str)
        {
            string regExHtmlTag = "(<[^>]+>)";
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(regExHtmlTag, System.Text.RegularExpressions.RegexOptions.Singleline);
            str = reg.Replace(str, " ");
            str = str.Replace("\t", " ");
            str = str.Replace("\r", " ");
            str = str.Replace("\n", " ");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            reg = new System.Text.RegularExpressions.Regex("( )+", System.Text.RegularExpressions.RegexOptions.Singleline);
            str = reg.Replace(str, " ");
            return str;
        }

        /// <summary>
        /// 删除文本中的html标签
        /// </summary>
        /// <param name="input">含有html标签的文本</param>
        /// <returns>返回没有html标签的文本</returns>
        public static string Html2Txt(string input)
        {
            string sRet;
            System.Text.RegularExpressions.Regex reg =
                new System.Text.RegularExpressions.Regex("<[^>]+>", System.Text.RegularExpressions.RegexOptions.Compiled);
            sRet = reg.Replace(input, string.Empty);
            sRet = sRet.Replace("&nbsp;", " ");
            sRet = sRet.Replace("<", "&lt;");
            sRet = sRet.Replace(">", "&gt;");
            sRet = sRet.Replace("　", " ");
            return sRet;
        }


        /// <summary>
        /// 实现JavaScript的函数escape()的字符串转换
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string Escape(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byteArr = System.Text.Encoding.Unicode.GetBytes(str);

            for (int i = 0; i < byteArr.Length; i += 2)
            {
                sb.Append("%u");
                sb.Append(byteArr[i + 1].ToString("X2"));//把字节转换为十六进制的字符串表现形式

                sb.Append(byteArr[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// JavaScript的escape()转换过去的字符串解释回来
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string Unescape(string str)
        {
            str = str.Remove(0, 2);//删除最前面两个＂%u＂
            string[] strArr = str.Split(new string[] { "%u" }, StringSplitOptions.None);//以子字符串＂%u＂分隔
            byte[] byteArr = new byte[strArr.Length * 2];
            for (int i = 0, j = 0; i < strArr.Length; i++, j += 2)
            {
                byteArr[j + 1] = Convert.ToByte(strArr[i].Substring(0, 2), 16);  //把十六进制形式的字串符串转换为二进制字节
                byteArr[j] = Convert.ToByte(strArr[i].Substring(2, 2), 16);
            }
            str = System.Text.Encoding.Unicode.GetString(byteArr);　//把字节转为unicode编码
            return str;
        }

        /// <summary>
        /// 获得Request参数的值
        /// </summary>
        /// <param name="strName">Url参数名称</param>
        /// <returns>Url参数的值</returns>
        /// <remarks>
        /// </remarks>
        public static string GetQueryString(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.QueryString[strName];
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数名称</param>
        /// <returns>表单参数的值</returns>
        /// <remarks>
        /// </remarks>
        public static string GetFormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        /// 页面抓取
        /// </summary>
        /// <param name="sLocation">地址</param>
        /// <param name="htPara">表单集合</param>
        /// <param name="codingType">编码名称</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string GetPage(string sLocation, Hashtable htPara, string codingType)
        {
            WebRequest hr = null;
            string sRet;
            Stream st;
            StreamReader sr;
            Uri url;

            if (htPara != null && htPara.Count > 0)
            {
                url = new Uri(sLocation + "?" + ColToStr(htPara, codingType));
            }
            else
            {
                url = new Uri(sLocation);
            }

            hr = HttpWebRequest.Create(url);
            hr.Timeout = 240 * 1000;
            st = hr.GetResponse().GetResponseStream();
            sr = new StreamReader(st, System.Text.Encoding.GetEncoding(codingType));

            sRet = sr.ReadToEnd();
            sr.Dispose();
            st.Dispose();
            return sRet;
        }

        /// <summary>
        /// 页面抓取
        /// </summary>
        /// <param name="sLocation">地址</param>
        /// <param name="htPara">表单集合</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string GetPage(string sLocation, Hashtable htPara)
        {
            return GetPage(sLocation, htPara, defaultCodingType);
        }

        /// <summary>
        /// 页面抓取
        /// </summary>
        /// <param name="sLocation">地址</param>
        /// <param name="htPara">表单信息</param>
        /// <param name="codingType">编码名称</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string GetPage(string url, string postPara, string encType)
        {
            return GetPage(url, postPara, (CookieCollection)null, false, encType, "");
        }

        /// <summary>
        /// 页面抓取，返回Stream
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Stream GetPage(string url)
        {
            var uri = new Uri(url);
            var hr = HttpWebRequest.Create(url);
            hr.Timeout = 240 * 1000;
            var st = hr.GetResponse().GetResponseStream();
            return st;
        }

        /// <summary>
        /// 页面抓取
        /// </summary>
        /// <param name="url"></param>
        /// <param name="refer">引用地址</param>
        /// <returns></returns>
        public static Stream GetPage(string url, string refer)
        {
            var uri = new Uri(url);
            var hr = (System.Net.HttpWebRequest)WebRequest.Create(url);
            hr.Referer = refer;
            hr.Timeout = 240 * 1000;
            var st = hr.GetResponse().GetResponseStream();
            return st;
        }

        /// <summary>
        /// 页面抓取
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postPara">表单信息</param>
        /// <param name="cookies">集合</param>
        /// <param name="hasCookie">是否验证</param>
        /// <param name="encType">编码名称</param>
        /// <param name="refer">HTTP头信息</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string GetPage(string url, string postPara, CookieCollection cookies, bool hasCookie, string encType, string refer)
        {
            string returnValue;
            if (url.StartsWith("http://") == false)
            {
                url = "http://" + url;
            }
            HttpWebRequest hRqst = (System.Net.HttpWebRequest)HttpWebRequest.Create(url);
            if (hasCookie == true && (cookies != null))
            {
                hRqst.CookieContainer = new CookieContainer();
                hRqst.CookieContainer.Add(cookies);
            }

            hRqst.ContentType = "application/x-www-form-urlencoded";
            hRqst.Headers.Add("Accept-Language", "zh-cn");
            Stream streamData;
            byte[] bt;
            hRqst.Timeout = TimeOut * 1000;
            hRqst.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
            if (postPara == "")
            {
                hRqst.Method = "GET";
            }
            else
            {
                hRqst.Method = "POST";
                hRqst.AllowWriteStreamBuffering = true;
                bt = System.Text.Encoding.ASCII.GetBytes(postPara);
                hRqst.ContentLength = bt.Length;
                hRqst.Referer = refer;
                hRqst.KeepAlive = false;
                streamData = hRqst.GetRequestStream();
                streamData.Write(bt, 0, bt.Length);
                streamData.Close();
            }
            HttpWebResponse hRsp;
            hRsp = (System.Net.HttpWebResponse)hRqst.GetResponse();
            streamData = hRsp.GetResponseStream();
            if (hasCookie == true && (hRsp.Cookies != null))
            {
                cookies.Add(hRsp.Cookies);
            }
            if (encType == "")
            {
                encType = "utf-8";
            }
            System.IO.StreamReader readStream = new System.IO.StreamReader(streamData, System.Text.Encoding.GetEncoding(encType));
            returnValue = readStream.ReadToEnd();
            streamData.Close();
            return returnValue;
        }

        /// <summary>
        /// 页面抓取
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postPara">表单集合</param>
        /// <param name="cookies">Cookie集合</param>
        /// <param name="hasCookie">是否验证</param>
        /// <param name="encType">编码名称</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string GetPage(string url, Hashtable postPara, CookieCollection cookies, bool hasCookie, string encType)
        {
            return GetPage(url, ColToStr(postPara, "utf-8"), cookies, hasCookie, encType, "");
        }

        /// <summary>
        /// 页面抓取
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="cookies">Cookie集合</param>
        /// <param name="hasCookie">是否验证</param>
        /// <param name="encType">编码名称</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        [Obsolete("此方法已过时,请不要再使用")]
        public static string GetPage(string url, CookieCollection cookies, bool hasCookie, string encType)
        {
            return GetPage(url, "", cookies, hasCookie, encType, "");
        }

        /// <summary>
        /// 该函数用于转换表单项集合为字符串
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        /// <remarks>
        /// HttpDriver 转来
        /// </remarks>
        [Obsolete("此方法已过时,请不要再使用")]
        public static string ColToStr(System.Collections.Hashtable ht)
        {
            return ColToStr(ht, defaultCodingType);
        }

        /// <summary>
        /// 转换表单项集合为字符串
        /// </summary>
        /// <param name="ht">表单集合</param>
        /// <param name="codingType">编码名称</param>
        /// <returns></returns>
        public static string ColToStr(System.Collections.Hashtable ht, string codingType)
        {
            StringBuilder sbRet = new StringBuilder();

            if (ht != null)
            {
                foreach (System.Collections.DictionaryEntry de in ht)
                {
                    sbRet.Append(System.Web.HttpUtility.UrlEncode(de.Key.ToString(), System.Text.Encoding.GetEncoding(codingType)) + "=" +
                        System.Web.HttpUtility.UrlEncode(de.Value.ToString(), System.Text.Encoding.GetEncoding(codingType)) + "&");
                }
                if (sbRet.Length > 0)
                {
                    sbRet.Remove(sbRet.Length - 1, 1);
                }
            }
            return sbRet.ToString();

        }

        /// <summary>
        /// 在网页中添加JavaScript
        /// </summary>
        /// <param name="js">ScriptScript脚本</param>
        /// <param name="page">提示的当前页面</param>
        /// <remarks>
        /// </remarks>
        public static void ShowJavaScript(String js, Page page)
        {
            ClientScriptManager csm = page.ClientScript;
            if (!csm.IsClientScriptIncludeRegistered("clientScript"))
            {
                csm.RegisterStartupScript(page.GetType(), "client_js", js, true);
            }
        }

        /// <summary>
        /// 提示Alert对话框
        /// </summary>
        /// <param name="message">要提示的信息</param>
        /// <param name="page">提示的当前页面</param>
        /// <remarks>
        /// </remarks>
        public static void Alert(String message, Page page)
        {
            String js = "alert(\"" + message + "\");";
            ClientScriptManager csm = page.ClientScript;
            if (!csm.IsClientScriptIncludeRegistered("clientScript"))
            {
                csm.RegisterStartupScript(page.GetType(), "js_alert", js, true);
            }
        }

        /// <summary>
        /// 提示Alert对话框,并转到另一个页面
        /// </summary>
        /// <param name="message">要提示的信息</param>
        /// <param name="url">要转到的URL</param>
        /// <param name="page">提示的当前页面</param>
        /// <remarks>
        /// </remarks>
        [Obsolete("此方法已过时,请不要再使用")]
        public static void Alert(String message, string url, Page page)
        {
            String js = "alert(\"" + message + "\"); \n document.location.href=\"" + url + "\";";
            ClientScriptManager csm = page.ClientScript;
            if (!csm.IsClientScriptIncludeRegistered("clientScript"))
            {
                csm.RegisterStartupScript(page.GetType(), "js_alert_and_location", js, true);
            }
        }

        /// <summary>
        /// 去掉路径和扩展名后，返回页面名称
        /// </summary>
        /// <param name="page">当前页面</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string GetPageNameByPath(System.Web.UI.Page page)
        {
            string pageName;
            pageName = page.Request.Path;
            pageName = pageName.Substring(pageName.LastIndexOf("/") + 1);
            pageName = pageName.Substring(0, pageName.LastIndexOf(".")).ToLower();
            return pageName;
        }

        /// <summary>
        /// 为地址添加http://
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string FormatURL(string url)
        {
            if ((!url.ToLower().StartsWith("http://")) && (url != null) && url.Length > 0)
            {
                url = "http://" + url;
            }
            return url;
        }

        /// <summary>
        /// 防止跨站脚本攻击,和恶意脚本。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string FilterHtml(string text)
        {
            string sRet;

            if (text != null)
            {
                sRet = text.Replace("<script", "&lt;script");
                sRet = text.Replace("javascript:", "javascript：");
            }
            else
            {
                sRet = string.Empty;
            }

            return sRet;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">file path  in service</param>
        /// <param name="local">save path in local</param>
        public static void DownLoadFile(string url, string local)
        {
            WebClient webClient = new WebClient();
            try
            {
                webClient.DownloadFile(url, local);
            }
            catch
            {
                try
                {
                    webClient.DownloadFile(url, local);
                }
                catch
                {
                    webClient.DownloadFile(url, local);
                }
            }
        }
    }
}