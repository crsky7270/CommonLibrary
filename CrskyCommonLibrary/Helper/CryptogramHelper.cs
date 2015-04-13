using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crsky.Utility.Helper
{
   /// <summary>
   /// 加解密集合
   /// </summary>
   public sealed class CryptogramHelper
   {
      #region 静态常量
      /// <summary>
      /// 全局密钥(64位)
      /// </summary>
      private static readonly string Key = "lQa9_&skzly%!9fs2@*UNA($ck_^:)'aI9e.^2Lbx9,5lf!j+~Hz@^hakuJ^crOb";
      #endregion

      #region MD5加密字符串
      /// <summary>
      /// MD5加密字符串
      /// </summary>
      /// <param name="sourceStr">要加密的字符串</param>
      /// <returns>MD5加密后的字符串</returns>
      public static string MD5(string sourceStr)
      {
         string sRet = string.Empty;
         System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create();
         byte[] btRet;
         System.IO.MemoryStream ms = new System.IO.MemoryStream();
         System.IO.StreamWriter sw = new System.IO.StreamWriter(ms);
         System.IO.StreamReader sr = new System.IO.StreamReader(ms);

         if (sourceStr == null)
         {
            sourceStr = string.Empty;
         }

         sw.Write(sourceStr);
         sw.Flush();
         ms.Seek(0, System.IO.SeekOrigin.Begin);

         btRet = md5.ComputeHash(ms);
         ms.SetLength(0);
         sw.Flush();

         for (int i = 0; i < btRet.Length; i++)
         {
            sw.Write("{0:X2}", btRet[i]);
         }
         sw.Flush();
         ms.Seek(0, System.IO.SeekOrigin.Begin);
         sRet = sr.ReadToEnd();

         sw.Close();
         sw.Dispose();
         sr.Close();
         sr.Dispose();
         ms.Close();
         ms.Dispose();

         return sRet;
      }
      #endregion

      #region SHA256加密字符串
      /// <summary>
      /// SHA256加密字符串
      /// </summary>
      /// <param name="sourceStr">要加密的字符串</param>
      /// <returns>SHA256加密后的字符串</returns>
      public static string SHA256(string sourceStr)
      {
         byte[] SHA256Data = Encoding.UTF8.GetBytes(sourceStr);
         SHA256Managed Sha256 = new SHA256Managed();
         byte[] Result = Sha256.ComputeHash(SHA256Data);
         Sha256.Clear();
         return Convert.ToBase64String(Result);  //返回长度为44字节的字符串
      }
      #endregion

      #region TripleDES加解密

      #region 3DES加密
      /// <summary>
      /// 对字符串用默认密钥进行3DES加密
      /// </summary>
      /// <param name="sourceStr">要加密的字符串</param>
      /// <returns>加密后并经base64编码的字符串</returns>
      public static string Encrypt3DES(string sourceStr)
      {
         TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
         MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();

         DES.Key = hashMD5.ComputeHash(Encoding.UTF8.GetBytes(Key));
         DES.Mode = CipherMode.ECB;

         ICryptoTransform DESEncrypt = DES.CreateEncryptor();

         byte[] Buffer = Encoding.UTF8.GetBytes(sourceStr);
         return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
      }
      /// <summary>
      /// 对字符串用指定密钥进行3DES加密
      /// </summary>
      /// <param name="sourceStr">要加密的字符串</param>
      /// <param name="key">密钥</param>
      /// <returns>加密后并经base64编码的字符串</returns>
      public static string Encrypt3DES(string sourceStr, string key)
      {
         TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
         MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();

         DES.Key = hashMD5.ComputeHash(Encoding.UTF8.GetBytes(key));
         DES.Mode = CipherMode.ECB;

         ICryptoTransform DESEncrypt = DES.CreateEncryptor();

         byte[] Buffer = Encoding.UTF8.GetBytes(sourceStr);
         return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
      }
      /// <summary>
      /// 对字符串用指定密钥与指定编码方式加密
      /// </summary>
      /// <param name="sourceStr">要加密的字符串</param>
      /// <param name="key">密钥</param>
      /// <param name="encoding">编码方式</param>
      /// <returns>加密后并经base64编码的字符串</returns>
      public static string Encrypt3DES(string sourceStr, string key, Encoding encoding)
      {
         TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
         MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();

         DES.Key = hashMD5.ComputeHash(encoding.GetBytes(key));
         DES.Mode = CipherMode.ECB;

         ICryptoTransform DESEncrypt = DES.CreateEncryptor();

         byte[] Buffer = encoding.GetBytes(sourceStr);
         return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
      }
      #endregion

      #region 3DES解密字符串
      /// <summary>
      /// 对加密字符串用默认密钥进行3DES解密
      /// </summary>
      /// <param name="sourceStr">要解密的字符串</param>
      /// <returns>解密后的字符串</returns>
      public static string Decrypt3DES(string sourceStr)
      {
         TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
         MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();

         DES.Key = hashMD5.ComputeHash(Encoding.UTF8.GetBytes(Key));
         DES.Mode = CipherMode.ECB;

         ICryptoTransform DESDecrypt = DES.CreateDecryptor();

         string result = "";
         try
         {
            byte[] Buffer = Convert.FromBase64String(sourceStr);
            result = Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
         }
         catch
         {
            ;
         }
         return result;
      }
      /// <summary>
      /// 对加密字符串用指定密钥进行3DES解密
      /// </summary>
      /// <param name="sourceStr">要解密的字符串</param>
      /// <param name="key">密钥</param>
      /// <returns>解密后的字符串</returns>
      public static string Decrypt3DES(string sourceStr, string key)
      {
         TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
         MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();

         DES.Key = hashMD5.ComputeHash(Encoding.UTF8.GetBytes(key));
         DES.Mode = CipherMode.ECB;

         ICryptoTransform DESDecrypt = DES.CreateDecryptor();

         string result = "";
         try
         {
            byte[] Buffer = Convert.FromBase64String(sourceStr);
            result = Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
         }
         catch
         {
            ;
         }
         return result;
      }
      /// <summary>
      /// 对加密字符串用指定密钥与指定编码方式进行3DES解密
      /// </summary>
      /// <param name="sourceStr">要解密的字符串</param>
      /// <param name="key">密钥</param>
      /// <param name="encoding">编码方式</param>
      /// <returns>解密后的字符串</returns>
      public static string Decrypt3DES(string sourceStr, string key, Encoding encoding)
      {
         TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
         MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();

         DES.Key = hashMD5.ComputeHash(encoding.GetBytes(key));
         DES.Mode = CipherMode.ECB;

         ICryptoTransform DESDecrypt = DES.CreateDecryptor();

         string result = "";
         try
         {
            byte[] Buffer = Convert.FromBase64String(sourceStr);
            result = encoding.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
         }
         catch
         {
            ;
         }
         return result;
      }
      #endregion

      #endregion

      #region Base64加解密

      #region Base64加密
      /// <summary>
      /// 对字符串用Base64加密
      /// </summary>
      /// <param name="sourceStr">要加密的字符串</param>
      /// <returns>加密后的字符串</returns>
      public static string EncryptBase64(string sourceStr)
      {
         return Convert.ToBase64String(Encoding.UTF8.GetBytes(sourceStr));
      }
      /// <summary>
      /// 对字符串用指定密钥与指定编码方式进行Base64加密
      /// </summary>
      /// <param name="sourceStr">要加密的字符串</param>
      /// <param name="encoding">字符编码方式(Encoding)</param>
      /// <returns>加密后的字符串</returns>
      public static string EncryptBase64(string sourceStr, Encoding encoding)
      {
         return Convert.ToBase64String(encoding.GetBytes(sourceStr));
      }
      #endregion

      #region Base64解密
      /// <summary>
      /// 对字符串用Base64解密
      /// </summary>
      /// <param name="sourceStr">已加密字符串</param>
      /// <returns>解密后的字符串</returns>
      public static string DecryptBase64(string sourceStr)
      {
         Byte[] bytes = Convert.FromBase64String(sourceStr);
         return Encoding.UTF8.GetString(bytes);
      }
      /// <summary>
      /// 对字符串用指定密钥与指定编码方式进行Base64解密
      /// </summary>
      /// <param name="sourceStr">已加密字符串</param>
      /// <param name="encoding">字符编码方式(Encoding)</param>
      /// <returns>解密后的字符串</returns>
      public static string DecryptBase64(string sourceStr, Encoding encoding)
      {
         Byte[] bytes = Convert.FromBase64String(sourceStr);
         return encoding.GetString(bytes);
      }
      #endregion

      #endregion
   }
}
