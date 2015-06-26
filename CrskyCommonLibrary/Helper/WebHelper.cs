/*----------------------------------------------------------------
 * 
 * Copyright 
 * 
 * File Name   ��WebRelated.cs
 * Description ��web��ع���
 * 
 * Create  ��
 * Modify  ��
 * History ��
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
        /// ��ȡ�ͻ���Ip
        /// </summary>
        /// <param name="page">ҳ��Page</param>
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
        /// ��ȡ�ͻ���Ip
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
        /// �Ƿ�Ϊip
        /// </summary>
        /// <param name="ip">Ip��ַ</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        }

        /// <summary>
        /// ֱ�ӻ�ȡ�ͻ�������IP
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static long GetClientIPNumber(System.Web.UI.Page page)
        {
            return ConvertIPToLong(GetClientIP(page));
        }

        /// <summary>
        /// ��ipתΪ���ָ�ʽ
        /// </summary>
        /// <param name="Ip"></param>
        /// <returns></returns>
        public static long ConvertIPToLong(string Ip)
        {
            long longResult = 0;
            try
            {
                //ȡ��IP��ַȥ����.�����string����
                string[] Ip_List = Ip.Split('.');
                string X_Ip = "";
                //ѭ�����飬������ת����ʮ�������������ϲ�����(3dafe81e)
                foreach (string ip in Ip_List)
                {
                    X_Ip += Convert.ToByte(ip).ToString("x").PadLeft(2, '0');
                }

                //��ʮ��������ת����ʮ������(1034938398)
                longResult = long.Parse(X_Ip, System.Globalization.NumberStyles.HexNumber);
            }
            catch
            {
                //���õ�IP��Χ�쳣
            }

            return longResult;
        }


        /// <summary>
        /// ��Ҫֱ�������ҳ����ı��е�Html��ǽ��б���
        /// add by ��ά��
        /// һ���ո��滻Ϊ���������Ϊ��������ʾӢ��
        /// </summary>
        /// <param name="text">��Ҫ������ı�</param>
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
        /// �����û������Ƿ�����
        ///  </summary>
        ///  <param name="str">�û��ύ������ </param>
        ///  <returns>�����Ƿ���SQLע��ʽ�������� </returns>
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
        /// ȥ��HTML��ǩ
        /// </summary>
        /// <param name="str">HTML����</param>
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
        /// ɾ���ı��е�html��ǩ
        /// </summary>
        /// <param name="input">����html��ǩ���ı�</param>
        /// <returns>����û��html��ǩ���ı�</returns>
        public static string Html2Txt(string input)
        {
            string sRet;
            System.Text.RegularExpressions.Regex reg =
                new System.Text.RegularExpressions.Regex("<[^>]+>", System.Text.RegularExpressions.RegexOptions.Compiled);
            sRet = reg.Replace(input, string.Empty);
            sRet = sRet.Replace("&nbsp;", " ");
            sRet = sRet.Replace("<", "&lt;");
            sRet = sRet.Replace(">", "&gt;");
            sRet = sRet.Replace("��", " ");
            return sRet;
        }


        /// <summary>
        /// ʵ��JavaScript�ĺ���escape()���ַ���ת��
        /// </summary>
        /// <param name="str">Ҫת�����ַ���</param>
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
                sb.Append(byteArr[i + 1].ToString("X2"));//���ֽ�ת��Ϊʮ�����Ƶ��ַ���������ʽ

                sb.Append(byteArr[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// JavaScript��escape()ת����ȥ���ַ������ͻ���
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string Unescape(string str)
        {
            str = str.Remove(0, 2);//ɾ����ǰ��������%u��
            string[] strArr = str.Split(new string[] { "%u" }, StringSplitOptions.None);//�����ַ�����%u���ָ�
            byte[] byteArr = new byte[strArr.Length * 2];
            for (int i = 0, j = 0; i < strArr.Length; i++, j += 2)
            {
                byteArr[j + 1] = Convert.ToByte(strArr[i].Substring(0, 2), 16);  //��ʮ��������ʽ���ִ�����ת��Ϊ�������ֽ�
                byteArr[j] = Convert.ToByte(strArr[i].Substring(2, 2), 16);
            }
            str = System.Text.Encoding.Unicode.GetString(byteArr);��//���ֽ�תΪunicode����
            return str;
        }

        /// <summary>
        /// ���Request������ֵ
        /// </summary>
        /// <param name="strName">Url��������</param>
        /// <returns>Url������ֵ</returns>
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
        /// ���ָ����������ֵ
        /// </summary>
        /// <param name="strName">����������</param>
        /// <returns>��������ֵ</returns>
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
        /// ҳ��ץȡ
        /// </summary>
        /// <param name="sLocation">��ַ</param>
        /// <param name="htPara">������</param>
        /// <param name="codingType">��������</param>
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
        /// ҳ��ץȡ
        /// </summary>
        /// <param name="sLocation">��ַ</param>
        /// <param name="htPara">������</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string GetPage(string sLocation, Hashtable htPara)
        {
            return GetPage(sLocation, htPara, defaultCodingType);
        }

        /// <summary>
        /// ҳ��ץȡ
        /// </summary>
        /// <param name="sLocation">��ַ</param>
        /// <param name="htPara">����Ϣ</param>
        /// <param name="codingType">��������</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string GetPage(string url, string postPara, string encType)
        {
            return GetPage(url, postPara, (CookieCollection)null, false, encType, "");
        }

        /// <summary>
        /// ҳ��ץȡ������Stream
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
        /// ҳ��ץȡ
        /// </summary>
        /// <param name="url"></param>
        /// <param name="refer">���õ�ַ</param>
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
        /// ҳ��ץȡ
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <param name="postPara">����Ϣ</param>
        /// <param name="cookies">����</param>
        /// <param name="hasCookie">�Ƿ���֤</param>
        /// <param name="encType">��������</param>
        /// <param name="refer">HTTPͷ��Ϣ</param>
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
        /// ҳ��ץȡ
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <param name="postPara">������</param>
        /// <param name="cookies">Cookie����</param>
        /// <param name="hasCookie">�Ƿ���֤</param>
        /// <param name="encType">��������</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static string GetPage(string url, Hashtable postPara, CookieCollection cookies, bool hasCookie, string encType)
        {
            return GetPage(url, ColToStr(postPara, "utf-8"), cookies, hasCookie, encType, "");
        }

        /// <summary>
        /// ҳ��ץȡ
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <param name="cookies">Cookie����</param>
        /// <param name="hasCookie">�Ƿ���֤</param>
        /// <param name="encType">��������</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        [Obsolete("�˷����ѹ�ʱ,�벻Ҫ��ʹ��")]
        public static string GetPage(string url, CookieCollection cookies, bool hasCookie, string encType)
        {
            return GetPage(url, "", cookies, hasCookie, encType, "");
        }

        /// <summary>
        /// �ú�������ת�������Ϊ�ַ���
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        /// <remarks>
        /// HttpDriver ת��
        /// </remarks>
        [Obsolete("�˷����ѹ�ʱ,�벻Ҫ��ʹ��")]
        public static string ColToStr(System.Collections.Hashtable ht)
        {
            return ColToStr(ht, defaultCodingType);
        }

        /// <summary>
        /// ת�������Ϊ�ַ���
        /// </summary>
        /// <param name="ht">������</param>
        /// <param name="codingType">��������</param>
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
        /// ����ҳ�����JavaScript
        /// </summary>
        /// <param name="js">ScriptScript�ű�</param>
        /// <param name="page">��ʾ�ĵ�ǰҳ��</param>
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
        /// ��ʾAlert�Ի���
        /// </summary>
        /// <param name="message">Ҫ��ʾ����Ϣ</param>
        /// <param name="page">��ʾ�ĵ�ǰҳ��</param>
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
        /// ��ʾAlert�Ի���,��ת����һ��ҳ��
        /// </summary>
        /// <param name="message">Ҫ��ʾ����Ϣ</param>
        /// <param name="url">Ҫת����URL</param>
        /// <param name="page">��ʾ�ĵ�ǰҳ��</param>
        /// <remarks>
        /// </remarks>
        [Obsolete("�˷����ѹ�ʱ,�벻Ҫ��ʹ��")]
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
        /// ȥ��·������չ���󣬷���ҳ������
        /// </summary>
        /// <param name="page">��ǰҳ��</param>
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
        /// Ϊ��ַ���http://
        /// </summary>
        /// <param name="url">URL��ַ</param>
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
        /// ��ֹ��վ�ű�����,�Ͷ���ű���
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string FilterHtml(string text)
        {
            string sRet;

            if (text != null)
            {
                sRet = text.Replace("<script", "&lt;script");
                sRet = text.Replace("javascript:", "javascript��");
            }
            else
            {
                sRet = string.Empty;
            }

            return sRet;
        }

        /// <summary>
        /// �����ļ�
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