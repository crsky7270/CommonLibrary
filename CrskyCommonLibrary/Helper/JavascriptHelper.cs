using System;
using System.Web;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using Crsky.Utility.Helper;

namespace Crsky.Utility.Helper
{
    /// <summary>
    /// Javascript函数相关
    /// </summary>
    public class JavascriptHelper
    {
        #region 跳转到404页面
        /// <summary>
        /// 跳转到404页面
        /// </summary>
        /// <param name="NavigateHref">错误站点(ex:http://www.webcars.com.cn/)</param>
        public static void NavigateTo404(string NavigateHref)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type='text/javascript' language='javascript'>");
            sb.AppendFormat(" window.location.href='{0}' ", NavigateHref + "404.html");
            sb.AppendLine("</script>");
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }
        #endregion

        #region 弹出Alter对话框并可指定后续操作
        /// <summary>
        /// 弹出Alter对话框
        /// </summary>
        /// <param name="message">消息字符串</param>
        public static void Alert(string message)
        {
            Alert(message, null, false);
        }
        /// <summary>
        /// 弹出Alter对话框，控制是否结束页面执行
        /// </summary>
        /// <param name="message">消息字符串</param>
        /// <param name="pageEnd">是否结束页面执行</param>
        public static void Alert(string message, bool pageEnd)
        {
            Alert(message, null, pageEnd);
        }
        /// <summary>
        /// 弹出Alter对话框，指定页面转向并结束页面执行
        /// </summary>
        /// <param name="message">消息字符串</param>
        /// <param name="url">重定向URL</param>
        public static void Alert(string message, string url)
        {
            Alert(message, url, true);
        }
        /// <summary>
        /// 弹出Alter对话框，指定页面转向并控制是否结束页面执行
        /// </summary>
        /// <param name="message">消息字符串</param>
        /// <param name="url">重定向URL</param>
        /// <param name="pageEnd">是否结束页面执行</param>
        public static void Alert(string message, string url, bool pageEnd)
        {
            HttpContext.Current.Response.Write("<script>");
            HttpContext.Current.Response.Write("alert(\"" + message + "\");");

            if (url != null)
            {
                HttpContext.Current.Response.Write("document.location.href=\"" + url + "\";");
            }
            HttpContext.Current.Response.Write("</script>");

            if (pageEnd)
                HttpContext.Current.Response.End();
        }
        #endregion

        #region 弹出Alert对话框并返回前一页
        /// <summary>
        /// 弹出Alert对话框并返回前一页
        /// </summary>
        /// <param name="message">消息字符串</param>
        public static void AlertWithBack(string message)
        {
            AlertWithBack(message, false);
        }

        /// <summary>
        /// 弹出Alert对话框并返回前一页
        /// </summary>
        /// <param name="message">消息字符串</param>
        /// <param name="pageEnd">是否结束页面执行</param>
        public static void AlertWithBack(string message, bool pageEnd)
        {
            HttpContext.Current.Response.Write("<script>");
            HttpContext.Current.Response.Write("alert(\"" + message + "\");");
            HttpContext.Current.Response.Write("history.go(-1);");
            HttpContext.Current.Response.Write("</script>");

            if (pageEnd)
                HttpContext.Current.Response.End();
        }
        #endregion

        #region 注册一个客户端启动脚本
        /// <summary>
        /// 注册一个客户端启动脚本
        /// </summary>     
        /// <param name="key">要注册脚本的键名</param>
        /// <param name="scriptText">要注册的脚本块，该脚本不用包含script标记对</param>
        public static void RegisterStartupScript(string key, string scriptText)
        {
            RegisterStartupScript(key, scriptText, true);
        }

        /// <summary>
        ///  注册一个客户端启动脚本
        /// </summary>
        /// <param name="key">要注册脚本的键名</param>
        /// <param name="scriptText">要注册的脚本块</param>
        /// <param name="isScriptTag">脚本标记添加标识，指示是否对脚本块自动添加script标记对，
        /// 如果设置为true,则在脚本块中不用包含script标记对</param>
        public static void RegisterStartupScript(string key, string scriptText, bool isScriptTag)
        {
            try
            {
                //获取Page对象
                Page page = HttpContext.Current.Handler as Page;

                if (!ValidationHelper.IsNullOrEmpty(page))
                {

                    //获取ClientScriptManager对象
                    ClientScriptManager csm = page.ClientScript;

                    //获取要注册的启动脚本的类型
                    Type type = page.GetType();

                    //判断客户端启动脚本是否注册
                    if (!csm.IsStartupScriptRegistered(type, key))
                    {
                        //如果未注册，则进行注册
                        csm.RegisterStartupScript(type, key, scriptText, isScriptTag);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 注册一个客户端脚本块
        /// <summary>
        /// 注册一个客户端脚本块
        /// </summary>    
        /// <param name="key">要注册脚本的键名</param>
        /// <param name="scriptText">要注册的脚本块，该脚本不用包含script标记对</param>
        public static void RegisterClientScriptBlock(string key, string scriptText)
        {
            RegisterClientScriptBlock(key, scriptText, true);
        }

        /// <summary>
        ///  注册一个客户端脚本块
        /// </summary>
        /// <param name="key">要注册脚本的键名</param>
        /// <param name="scriptText">要注册的脚本块</param>
        /// <param name="isScriptTag">脚本标记添加标识，指示是否对脚本块自动添加script标记对，
        /// 如果设置为true,则在脚本块中不用包含script标记对</param>
        public static void RegisterClientScriptBlock(string key, string scriptText, bool isScriptTag)
        {
            try
            {
                //获取Page对象
                Page page = HttpContext.Current.Handler as Page;

                if (!ValidationHelper.IsNullOrEmpty(page))
                {
                    //获取ClientScriptManager对象
                    ClientScriptManager csm = page.ClientScript;

                    //获取要注册脚本块的类型
                    Type type = page.GetType();

                    //判断客户端脚本块是否注册
                    if (!csm.IsClientScriptBlockRegistered(type, key))
                    {
                        //如果未注册，则进行注册
                        csm.RegisterClientScriptBlock(type, key, scriptText, isScriptTag);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 设置控件的焦点
        /// <summary>
        /// 设置控件的焦点。如果要设置母版页中的控件焦点，应使用该方法的泛型版本.
        /// </summary>
        /// <param name="controlID">控件的ID</param>
        public static void SetFocus(string controlID)
        {
            //编写脚本
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("document.getElementById('{0}').focus();", controlID);

            //执行脚本
            RegisterStartupScript("focus", sb.ToString());
        }

        /// <summary>
        /// 设置控件的焦点,该版本用于设置母版页中的控件焦点.
        /// 只允许设置TextBox,DropDownList,RadioButton,CheckBox,Button的焦点。
        /// </summary>
        /// <typeparam name="T">要设置焦点的控件类</typeparam>
        /// <param name="controlID">要设置焦点的控件ID</param>
        public static void SetFocus<T>(T controlID)
        {
            //定义脚本字符串
            StringBuilder sb = new StringBuilder();

            //设置TextBox的焦点
            if (controlID is TextBox)
            {
                //编写脚本
                sb.AppendFormat("document.getElementById('{0}').focus();", (controlID as TextBox).ClientID);
                //执行脚本
                RegisterStartupScript("focus", sb.ToString());
                return;
            }
            //设置DropDownList的焦点
            if (controlID is DropDownList)
            {
                //编写脚本
                sb.AppendFormat("document.getElementById('{0}').focus();", (controlID as DropDownList).ClientID);
                //执行脚本
                RegisterStartupScript("focus", sb.ToString());
                return;
            }
            //设置RadioButton的焦点
            if (controlID is RadioButton)
            {
                //编写脚本
                sb.AppendFormat("document.getElementById('{0}').focus();", (controlID as RadioButton).ClientID);
                //执行脚本
                RegisterStartupScript("focus", sb.ToString());
                return;
            }
            //设置CheckBox的焦点
            if (controlID is CheckBox)
            {
                //编写脚本
                sb.AppendFormat("document.getElementById('{0}').focus();", (controlID as CheckBox).ClientID);
                //执行脚本
                RegisterStartupScript("focus", sb.ToString());
                return;
            }
            //设置Button的焦点
            if (controlID is Button)
            {
                //编写脚本
                sb.AppendFormat("document.getElementById('{0}').focus();", (controlID as Button).ClientID);
                //执行脚本
                RegisterStartupScript("focus", sb.ToString());
                return;
            }
        }
        #endregion

        #region 重定向到其它页面
        /// <summary>
        /// 重定向到其它页面
        /// </summary>
        /// <param name="url">要转向的页面地址</param>
        public static void Redirect(string url)
        {
            //执行脚本
            RegisterStartupScript("Redirect", string.Format("window.location.href='{0}';", url));
        }
        #endregion

        #region 返回上一页
        /// <summary>
        /// 返回上一页
        /// </summary>
        public static void ReturnBack()
        {
            //执行脚本
            RegisterStartupScript("ReturnBack", "history.go(-2);");
        }
        #endregion

        #region 弹出窗口
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="url">窗口的URL</param>
        public static void Open(string url)
        {
            //执行脚本
            RegisterStartupScript("Open", string.Format("window.open('{0}');", url));
        }
        #endregion

        #region 关闭窗口
        /// <summary>
        /// 关闭窗口
        /// </summary>
        public static void Close()
        {
            //执行脚本
            RegisterStartupScript("Close", "window.opener=null;window.close();");
        }
        #endregion

        #region 实现JavaScript的函数escape()的字符串转换
        /// <summary>
        /// 实现JavaScript的函数escape()的字符串转换
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string Escape(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byteArr = Encoding.UTF8.GetBytes(str);

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
        /// <param name="s">要反转的字符串</param>
        /// <returns>反转后的字符串</returns>
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
            str = Encoding.UTF8.GetString(byteArr);　//把字节转为unicode编码
            return str;
        }
        #endregion
    }
}
