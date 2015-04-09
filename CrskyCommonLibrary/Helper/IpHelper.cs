using System;
using System.Web;
using System.Net;

namespace Crsky.Utility.Helper
{
   /// <summary>
   /// IP查询与转换相关
   /// </summary>
   public class IpHelper
   {
      #region 客户端IP变量枚举
      /// <summary>
      /// 客户端IP变量枚举
      /// </summary>
      private enum ClientIPVariable
      {
         None,
         Remote_Addr,
         Http_X_Forwarded_For,
         Http_Client_IP,
         X_Client_IP
      }
      #endregion

      #region 获取客户端IP真实地址
      /// <summary>
      /// 获取客户端IP真实地址
      /// </summary>
      /// <returns>返回客户端IP真实地址</returns>
      public static string GetClientIPAddress()
      {
         ClientIPVariable ipVar;

         return GetClientIPAddress(out ipVar);
      }

      /// <summary>
      /// 获取客户端IP真实地址
      /// </summary>
      /// <param name="ipVar">IP变量的来源 (out 接收)</param>
      /// <returns>客户端IP真实地址</returns>
      private static string GetClientIPAddress(out ClientIPVariable ipVar)
      {
         string retValue = null;

         ipVar = ClientIPVariable.None;

         // 非加密页就检查 Proxy IP 变数
         if (String.Compare(HttpContext.Current.Request.Url.Scheme, "https", true) != 0)
         {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
               ipVar = ClientIPVariable.Http_X_Forwarded_For;
               retValue = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(",;".ToCharArray())[0];
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"]))
            {
               ipVar = ClientIPVariable.Http_Client_IP;
               retValue = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["X_CLIENT_IP"]))
            {
               ipVar = ClientIPVariable.X_Client_IP;
               retValue = HttpContext.Current.Request.ServerVariables["X_CLIENT_IP"];
            }
         }

         // 若还没取到 IP
         if (retValue == null)
         {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]))
            {
               ipVar = ClientIPVariable.Remote_Addr;
               retValue = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
         }
         return retValue;
      }

      #endregion

      #region 获取服务端IP真实地址
      /// <summary>
      /// 获取服务端IP真实地址
      /// <remarks>
      /// 如果取不到，则返回127.0.0.1
      /// </remarks>
      /// </summary>
      /// <returns>服务端IP真实地址</returns>
      public static string GetRemoteIPAddress()
      {
         IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
         if (addressList.Length > 0)
         {
            return addressList[0].ToString();
         }
         return "127.0.0.1";
      }
      #endregion

      #region 将数字IP转化成点分IP
      /// <summary>
      /// 将数字IP转化成点分IP
      /// </summary>
      /// <param name="ip">数字IP字符串</param>
      /// <returns>转化后的点分IP字符串</returns>
      public static string DigitalIPIntoDottedIP(Int64 ip)
      {
         //将数字IP转换为十六进制字符串
         string sHex = ip.ToString("X");
         string HexUse = sHex;
         while (HexUse.Length < 8)
            HexUse = "0" + HexUse;

         //将截取的字符串以俩个为单位分组
         string str1 = HexUse.Substring(6, 2);
         string str2 = HexUse.Substring(4, 2);
         string str3 = HexUse.Substring(2, 2);
         string str4 = HexUse.Substring(0, 2);

         //将分组的字串转换为十进制字串
         string IP1 = int.Parse(str1, System.Globalization.NumberStyles.HexNumber).ToString();
         string IP2 = int.Parse(str2, System.Globalization.NumberStyles.HexNumber).ToString();
         string IP3 = int.Parse(str3, System.Globalization.NumberStyles.HexNumber).ToString();
         string IP4 = int.Parse(str4, System.Globalization.NumberStyles.HexNumber).ToString();
         switch (IP1.Length)
         {
            case 1:
               IP1 = "00" + IP1;
               break;
            case 2:
               IP1 = "0" + IP1;
               break;
            default:
               break;
         }
         switch (IP2.Length)
         {
            case 1:
               IP2 = "00" + IP2;
               break;
            case 2:
               IP2 = "0" + IP2;
               break;
            default:
               break;
         }
         switch (IP3.Length)
         {
            case 1:
               IP3 = "00" + IP3;
               break;
            case 2:
               IP3 = "0" + IP3;
               break;
            default:
               break;
         }
         switch (IP4.Length)
         {
            case 1:
               IP4 = "00" + IP4;
               break;
            case 2:
               IP4 = "0" + IP4;
               break;
            default:
               break;
         }
         string lastIP = IP4 + "." + IP3 + "." + IP2 + "." + IP1;
         return lastIP;
      }
      #endregion

      #region 将点分IP转化成数字IP
      /// <summary>
      /// 将点分IP转化成数字IP
      /// </summary>
      /// <param name="ip">点分IP字符串</param>
      /// <returns>转化后的数字IP字符串</returns>
      public static long DottedIPIntoDigitalIP(string ip)
      {
         long longResult = 0;
         try
         {
            //取出IP地址去掉‘.’后的string数组
            string[] Ip_List = ip.Split('.');
            string X_Ip = "";
            //循环数组，把数据转换成十六进制数，并合并数组(3dafe81e)
            foreach (string str in Ip_List)
            {
               X_Ip += Convert.ToByte(str).ToString("x").PadLeft(2, '0');
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
      #endregion

      #region 直接获取客户端数字IP
      /// <summary>
      /// 直接获取客户端数字IP
      /// </summary>
      /// <returns>转化后的客户端数字IP字符串</returns>
      public static long GetClientDigitalIP()
      {
         return DottedIPIntoDigitalIP(GetClientIPAddress());
      }
      #endregion

      #region 验证IP地址是否合法
      /// <summary>
      /// 验证IP地址是否合法
      /// </summary>
      /// <param name="ip">要验证的IP地址</param>        
      public static bool IsIPLegality(string ip)
      {
         //如果为空，认为验证合格
         if (ValidationHelper.IsNullOrEmpty(ip))
         {
            return false;
         }

         //清除要验证字符串中的空格
         ip = ip.Trim();

         //模式字符串
         string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";

         //验证
         return RegexHelper.IsMatch(ip, pattern);
      }
      #endregion

   }
}
