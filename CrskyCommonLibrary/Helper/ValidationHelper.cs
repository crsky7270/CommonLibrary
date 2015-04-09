using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crsky.Utility.Helper
{
   public class ValidationHelper
   {
      #region 检测对象是否为空

      #region 重载1
      /// <summary>
      /// 检测对象是否为空，为空返回true
      /// </summary>
      /// <typeparam name="T">要验证的对象的类型</typeparam>
      /// <param name="data">要验证的对象</param>        
      public static bool IsNullOrEmpty<T>(T data)
      {
         //如果为null
         if (data == null)
         {
            return true;
         }

         //如果为""
         if (data.GetType() == typeof(String))
         {
            if (string.IsNullOrEmpty(data.ToString().Trim()))
            {
               return true;
            }
            else
            {
               return false;
            }
         }

         //如果为DBNull
         if (data.GetType() == typeof(DBNull))
         {
            return true;
         }

         //不为空
         return false;
      }
      #endregion

      #region 重载2
      /// <summary>
      /// 检测对象是否为空，为空返回true
      /// </summary>
      /// <param name="data">要验证的对象</param>
      public static bool IsNullOrEmpty(object data)
      {
         return IsNullOrEmpty<object>(data);
      }
      #endregion

      #region 重载3
      /// <summary>
      /// 检测字符串是否为空，为空返回true
      /// </summary>
      /// <param name="text">要检测的字符串</param>
      public static bool IsNullOrEmpty(string text)
      {
         //检测是否为null
         if (text == null)
         {
            return true;
         }

         //检测字符串空值
         if (string.IsNullOrEmpty(text.ToString().Trim()))
         {
            return true;
         }
         else
         {
            return false;
         }
      }

      #endregion

      #endregion

      #region 验证IP地址是否合法
      /// <summary>
      /// 验证IP地址是否合法
      /// </summary>
      /// <param name="ip">要验证的IP地址</param>        
      public static bool IsIPLegality(string ip)
      {
         return IpHelper.IsIPLegality(ip);
      }
      #endregion

      #region  验证EMail是否合法
      /// <summary>
      /// 验证EMail是否合法
      /// </summary>
      /// <param name="email">要验证的Email</param>
      /// <returns>true为合法</returns>
      public static bool IsEmail(string email)
      {
         if (IsNullOrEmpty(email))
         {
            return false;
         }

         //清除要验证字符串中的空格
         email = email.Trim();

         //模式字符串
         string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

         //验证
         return RegexHelper.IsMatch(email, pattern);
      }
      #endregion

      #region  验证是否是手机号
      /// <summary>
      /// 验证是否是手机号
      /// </summary>
      /// <param name="mobile">要验证的mobile</param>
      /// <returns>true为合法</returns>
      public static bool IsMobile(string mobile)
      {
         if (IsNullOrEmpty(mobile))
         {
            return false;
         }

         //清除要验证字符串中的空格
         mobile = mobile.Trim();

         //模式字符串
         string pattern = @"^((\(\d{3}\))|(\d{3}\-))?(13|15|18)\d{9}$";

         //验证
         return RegexHelper.IsMatch(mobile, pattern);
      }
      #endregion

      #region  验证是否是座机号
      /// <summary>
      /// 验证是否是座机号
      /// </summary>
      /// <param name="phone">要验证的phone</param>
      /// <returns>true为合法</returns>
      public static bool IsPhone(string phone)
      {
         if (IsNullOrEmpty(phone))
         {
            return false;
         }

         //清除要验证字符串中的空格
         phone = phone.Trim();

         //模式字符串
         string pattern = @"^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$";

         //验证
         return RegexHelper.IsMatch(phone, pattern);
      }
      #endregion

      #region 验证是否为整数
      /// <summary>
      /// 验证是否为整数
      /// </summary>
      /// <param name="number">要验证的整数</param>        
      public static bool IsInt(string number)
      {
         //如果为空，认为验证合格
         if (IsNullOrEmpty(number))
         {
            return false;
         }

         //清除要验证字符串中的空格
         number = number.Trim();

         //模式字符串
         string pattern = @"^[1-9]+[0-9]*$";

         //验证
         return RegexHelper.IsMatch(number, pattern);
      }
      #endregion

      #region 验证是否为数字
      /// <summary>
      /// 验证是否为数字
      /// </summary>
      /// <param name="number">要验证的数字</param>        
      public static bool IsNumber(string number)
      {
         //如果为空，认为验证合格
         if (IsNullOrEmpty(number))
         {
            return false;
         }

         //清除要验证字符串中的空格
         number = number.Trim();

         //模式字符串
         string pattern = @"^[-]?[0-9]*[.]?[0-9]*$";

         //验证
         return RegexHelper.IsMatch(number, pattern);
      }
      #endregion

      #region 验证日期是否合法
      /// <summary>
      /// 验证日期是否合法,对不规则的作了简单处理
      /// </summary>
      /// <param name="date">日期</param>
      public static bool IsDate(ref string date)
      {
         //如果为空，认为验证合格
         if (IsNullOrEmpty(date))
         {
            return true;
         }

         //清除要验证字符串中的空格
         date = date.Trim();

         //替换\
         date = date.Replace(@"\", "-");
         //替换/
         date = date.Replace(@"/", "-");

         //如果查找到汉字"今",则认为是当前日期
         if (date.IndexOf("今") != -1)
         {
            date = DateTime.Now.ToString();
         }

         try
         {
            //用转换测试是否为规则的日期字符
            date = Convert.ToDateTime(date).ToString("d");
            return true;
         }
         catch
         {
            //如果日期字符串中存在非数字，则返回false
            if (!IsInt(date))
            {
               return false;
            }

            #region 对纯数字进行解析
            //对8位纯数字进行解析
            if (date.Length == 8)
            {
               //获取年月日
               string year = date.Substring(0, 4);
               string month = date.Substring(4, 2);
               string day = date.Substring(6, 2);

               //验证合法性
               if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
               {
                  return false;
               }
               if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31)
               {
                  return false;
               }

               //拼接日期
               date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
               return true;
            }

            //对6位纯数字进行解析
            if (date.Length == 6)
            {
               //获取年月
               string year = date.Substring(0, 4);
               string month = date.Substring(4, 2);

               //验证合法性
               if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
               {
                  return false;
               }
               if (Convert.ToInt32(month) > 12)
               {
                  return false;
               }

               //拼接日期
               date = Convert.ToDateTime(year + "-" + month).ToString("d");
               return true;
            }

            //对5位纯数字进行解析
            if (date.Length == 5)
            {
               //获取年月
               string year = date.Substring(0, 4);
               string month = date.Substring(4, 1);

               //验证合法性
               if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
               {
                  return false;
               }

               //拼接日期
               date = year + "-" + month;
               return true;
            }

            //对4位纯数字进行解析
            if (date.Length == 4)
            {
               //获取年
               string year = date.Substring(0, 4);

               //验证合法性
               if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
               {
                  return false;
               }

               //拼接日期
               date = Convert.ToDateTime(year).ToString("d");
               return true;
            }
            #endregion

            return false;
         }
      }
      #endregion

      #region 验证身份证是否合法
      /// <summary>
      /// 验证身份证是否合法
      /// </summary>
      /// <param name="idCard">要验证的身份证</param>        
      public static bool IsIdCard(string idCard)
      {
         //如果为空，认为验证合格
         if (IsNullOrEmpty(idCard))
         {
            return true;
         }

         //清除要验证字符串中的空格
         idCard = idCard.Trim();

         //模式字符串
         StringBuilder pattern = new StringBuilder();
         pattern.Append(@"^(11|12|13|14|15|21|22|23|31|32|33|34|35|36|37|41|42|43|44|45|46|");
         pattern.Append(@"50|51|52|53|54|61|62|63|64|65|71|81|82|91)");
         pattern.Append(@"(\d{13}|\d{15}[\dx])$");

         //验证
         return RegexHelper.IsMatch(idCard, pattern.ToString());
      }
      #endregion

      #region 检测客户的输入中是否有危险字符串
      /// <summary>
      /// 检测客户输入的字符串是否有效,并将原始字符串修改为有效字符串或空字符串。
      /// 当检测到客户的输入中有攻击性危险字符串,则返回false,有效返回true。
      /// </summary>
      /// <param name="input">要检测的字符串</param>
      public static bool IsValidInput(ref string input)
      {
         try
         {
            if (IsNullOrEmpty(input))
            {
               //如果是空值,则跳出
               return true;
            }
            else
            {
               //替换单引号
               input = input.Replace("'", "''").Trim();

               //检测攻击性危险字符串
               string testString = "and |or |exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare ";
               string[] testArray = testString.Split('|');
               foreach (string testStr in testArray)
               {
                  if (input.ToLower().IndexOf(testStr) != -1)
                  {
                     //检测到攻击字符串,清空传入的值
                     input = string.Empty;
                     return false;
                  }
               }

               //未检测到攻击字符串
               return true;
            }
         }
         catch
         {
            return false;
         }
      }
      #endregion

      #region  验证用户是否含有非法字符[^<>'~`·!@#$%^&*()]+$
      /// <summary>
      /// 验证用户是否含有非法字符
      /// </summary>
      /// <param name="input">要验证的字符串</param>
      /// <returns>true为不包含</returns>
      public static bool NotHasInvalidChar(string input)
      {
         if (IsNullOrEmpty(input))
         {
            return false;
         }

         //清除要验证字符串中的空格
         input = input.Trim();

         //模式字符串
         string pattern = @"^[^<>'~`·!@#$%^&*()]+$";

         //验证
         return RegexHelper.IsMatch(input, pattern);
      }
      #endregion

      #region 验证指定字符串在指定字符串数组中的位置

      /// <summary>
      /// 验证指定字符串是否属于指定字符串数组中的一个元素
      /// </summary>
      /// <param name="searchStr">指定字符串</param>
      /// <param name="arrStr">指定字符串数组</param>
      /// <returns>字符串在指定字符串数组中的位置</returns>
      public static bool IsInArray(string searchStr, string[] arrStr)
      {
         return IsInArray(searchStr, arrStr, true);
      }
      /// <summary>
      /// 验证指定字符串是否属于指定字符串数组中的一个元素
      /// </summary>
      /// <param name="searchStr">指定字符串</param>
      /// <param name="arrStr">指定字符串数组</param>
      /// <param name="caseInsensetive">是否不区分大小写, true为区分, false为不区分</param>
      /// <returns>字符串在指定字符串数组中如不存在则返回false</returns>
      public static bool IsInArray(string searchStr, string[] arrStr, bool caseInsensetive)
      {
         return CommonHelper.GetIndexInArray(searchStr, arrStr, caseInsensetive) >= 0;
      }
      /// <summary>
      /// 判断指定字符串是否属于指定字符串数组中的一个元素
      /// </summary>
      /// <param name="searchStr">字符串</param>
      /// <param name="arrStr">内部以逗号分割单词的字符串</param>
      /// <returns>判断结果</returns>
      public static bool IsInArray(string searchStr, string arrStr)
      {
         return IsInArray(searchStr, CommonHelper.Split(arrStr, ","), false);
      }
      /// <summary>
      /// 验证指定字符串是否属于指定字符串数组中的一个元素
      /// </summary>
      /// <param name="searchStr">指定字符串</param>
      /// <param name="arrStr">指定内部以特殊符号分割的字符串</param>
      /// <param name="splitStr">特殊分割符号</param>
      /// <returns>字符串在指定字符串数组中如不存在则返回false</returns>
      public static bool IsInArray(string searchStr, string arrStr, string splitStr)
      {
         return IsInArray(searchStr, CommonHelper.Split(arrStr, splitStr), true);
      }
      /// <summary>
      /// 验证指定字符串是否属于指定字符串数组中的一个元素
      /// </summary>
      /// <param name="searchStr">字符串</param>
      /// <param name="arrStr">指定内部以特殊符号分割的字符串, true为区分, false为不区分</param>
      /// <param name="splitStr">分割字符串</param>
      /// <param name="caseInsensetive">是否不区分大小写, true为区分, false为不区分</param>
      /// <returns>验证结果</returns>
      public static bool IsInArray(string searchStr, string arrStr, string splitStr, bool caseInsensetive)
      {
         return IsInArray(searchStr, CommonHelper.Split(arrStr, splitStr), caseInsensetive);
      }
      #endregion

      #region 验证字符串是否超过指定字节长度
      /// <summary>
      /// 验证字符串是否超过指定字节长度
      /// </summary>
      /// <param name="str">字符串</param>
      /// <param name="maxLength">指定的最大字节长度(不包括)</param>
      /// <returns>true为超出字符串限定的最大长度</returns>
      public static bool IsExceedMaxLength(string str, int maxLength)
      {
         int length = CommonHelper.GetLength(str);
         if (length >= maxLength)
            return true;

         return false;
      }
      #endregion

      #region 验证字符串的字节长度区间
      /// <summary>
      /// 验证字符串的字节长度区间
      /// </summary>
      /// <param name="str">字符串</param>
      /// <param name="minLength">最小长度(包括)</param>
      /// <param name="maxLength">最大长度(不包括)</param>
      /// <returns>true为字符串的长度在指定的长度区间内</returns>
      public static bool IsLengthRange(string str, int minLength, int maxLength)
      {
         int strLength = CommonHelper.GetLength(str);
         if (strLength >= minLength && strLength < maxLength)
            return true;

         return false;
      }
      #endregion

      /// <summary>
      /// 判断字符是否英文半角字符或标点
      /// </summary>
      /// <remarks>
      /// 32    空格
      /// 33-47    标点
      /// 48-57    0~9
      /// 58-64    标点
      /// 65-90    A~Z
      /// 91-96    标点
      /// 97-122    a~z
      /// 123-126  标点
      /// </remarks>
      public static bool IsBjChar(char ctr)
      {
         int i = (int)ctr;
         return i >= 32 && i <= 126;
      }

      /// <summary>
      /// 判断字符是否全角字符或标点
      /// </summary>
      /// <param name="ctr">全角字符 - 65248 = 半角字符</param>
      /// <returns></returns>
      /// <remarks>全角空格例外</remarks>
      public static bool IsQjChar(char ctr)
      {
         if (ctr == '\u3000') return true;

         int i = (int)ctr - 65248;
         if (i < 32) return false;
         return IsBjChar((char)i);
      }

      /// <summary>
      /// 是否是奇数
      /// </summary>
      /// <param name="n">数字</param>
      /// <returns></returns>
      public static bool IsOdd(int n)
      {
         if (n % 2 != 0)
            return true;
         return false;
      }
   }
}
