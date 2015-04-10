using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Crsky.Utility.Helper;

namespace Crsky.Utility.Helper
{
   /// <summary>
   /// 通用函数集合[凡无法归入专项集合中的通用函数，均放入此处]
   /// 该类已经经过优化。时间:2015-04-10
   /// </summary>
   public class CommonHelper
   {
      #region 根据UTF-8编码获取字符串的字节长度
      /// <summary>
      /// 根据Unicode编码获取字符串的字节长度(1个汉字为2字节)
      /// </summary>
      /// <param name="str">字符串</param>
      /// <returns>字符串的字节长度(1个汉字为2字节)，输入字符串IsNullOrEmpty，则返回0</returns>
      public static int GetLength(string str)
      {
         return ValidationHelper.IsNullOrEmpty(str) ? 0 : Encoding.Unicode.GetByteCount(str);
      }

      #endregion

      #region 从字符串的相应位置截取指定字节长度的字符串
      /// <summary>
      /// 从字符串的起始位置截取指定字节长度的字符串
      /// </summary>
      /// <param name="str">原字符串</param>
      /// <param name="length">字节长度</param>
      /// <returns>指定字节长度的字符串</returns>
      public static string Substring(string str, int length)
      {
         if (length < 1) return string.Empty;
         var tempStr = Encoding.UTF8.GetBytes(str);
         return tempStr.Length > length ? Encoding.UTF8.GetString(tempStr, 0, length) : str;
      }

      /// <summary>
      /// 从字符串的指定位置截取指定长度的子字符串
      /// </summary>
      /// <param name="str">原字符串</param>
      /// <param name="startIndex">子字符串的起始位置</param>
      /// <param name="length">要返回的字符串的字节长度</param>
      /// <returns>指定字节长度的字符串</returns>
      public static string Substring(string str, int startIndex, int length)
      {
         Byte[] tempStr = Encoding.UTF8.GetBytes(str);

         if (startIndex >= 0)
         {
            if (length < 0)
            {
               length = length * -1;
               if (startIndex - length < 0)
               {
                  length = startIndex;
                  startIndex = 0;
               }
               else
               {
                  startIndex = startIndex - length;
               }
            }
            if (startIndex > tempStr.Length)
            {
               return "";
            }
         }
         else
         {
            if (length < 0)
            {
               return "";
            }
            if (length + startIndex > 0)
            {
               length = length + startIndex;
               startIndex = 0;
            }
            else
            {
               return "";
            }
         }
         if (tempStr.Length - startIndex < length)
         {
            length = tempStr.Length - startIndex;
         }
         return Encoding.UTF8.GetString(tempStr, startIndex, length);
      }
      #endregion

      #region 验证指定字符串在指定字符串数组中的位置

      /// <summary>
      /// 验证指定字符串在指定字符串数组中的位置
      /// </summary>
      /// <param name="searchStr">指定字符串</param>
      /// <param name="arrStr">指定字符串数组</param>
      /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
      public static int GetIndexInArray(string searchStr, string[] arrStr)
      {
         return GetIndexInArray(searchStr, arrStr, true);
      }
      /// <summary>
      /// 验证指定字符串在指定字符串数组中的位置
      /// </summary>
      /// <param name="searchStr">指定字符串</param>
      /// <param name="arrStr">指定字符串数组</param>
      /// <param name="caseInsensetive">是否区分大小写, true为区分, false为不区分</param>
      /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
      public static int GetIndexInArray(string searchStr, string[] arrStr, bool caseInsensetive)
      {
         var retValue = -1;
         if (string.IsNullOrEmpty(searchStr) || arrStr.Length <= 0) return retValue;
         for (var i = 0; i < arrStr.Length; i++)
         {
            if (caseInsensetive)
            {
               if (searchStr == arrStr[i])
                  retValue = i;
            }
            else
            {
               if (String.Equals(searchStr, arrStr[i], StringComparison.CurrentCultureIgnoreCase))
                  retValue = i;
            }
         }
         return retValue;
      }

      #endregion

      #region 分割字符串数组
      /// <summary>
      /// 分割字符串数组
      /// </summary>
      /// <param name="sourceStr">要分割字符串</param>
      /// <param name="splitStr">分割字符</param>
      /// <returns>分割后的字符串</returns>
      public static string[] Split(string sourceStr, string splitStr)
      {
         if (!string.IsNullOrEmpty(sourceStr))
         {
            if (sourceStr.IndexOf(splitStr, StringComparison.Ordinal) < 0)
            {
               string[] tmp = { sourceStr };
               return tmp;
            }
            return Regex.Split(sourceStr, Regex.Escape(splitStr), RegexOptions.IgnoreCase);
         }
         return new string[0] { };
      }

      /// <summary>
      /// 分割字符串数组
      /// </summary>
      /// <param name="sourceStr">要分割字符串</param>
      /// <param name="splitStr">分割字符</param>
      /// <param name="count">将从要分割字符串中分割出的字符数组最大索引数</param>
      /// <returns></returns>
      public static string[] Split(string sourceStr, string splitStr, int count)
      {
         var result = new string[count];
         var splited = Split(sourceStr, splitStr);
         for (var i = 0; i < count; i++)
         {
            if (i < splited.Length)
               result[i] = splited[i];
            else
               result[i] = string.Empty;
         }
         return result;
      }
      #endregion

      #region 过滤/还原HTML重要标记
      /// <summary>
      /// 将字符串转换为 HTML 编码的字符串
      /// </summary>
      /// <param name="str">HTML字符串</param>
      /// <returns>编码后的字符串</returns>
      public static string FormatHtmlTag(string str)
      {
         //为避免在HTTP中传递空白和标点之类的字符，造成接收端错误地解释，须先编码
         return HttpUtility.HtmlEncode(str);
      }

      /// <summary>
      /// 将已经为 HTTP 传输进行过 HTML 编码的字符串转换为已解码的字符串
      /// </summary>
      /// <param name="str">HTML字符串</param>
      /// <returns>解码后的字符串</returns>
      public static string UnFormatHtmlTag(string str)
      {
         return HttpUtility.HtmlDecode(str);
      }
      #endregion

   }
}
