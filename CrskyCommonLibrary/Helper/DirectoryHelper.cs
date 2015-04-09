using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crsky.Utility.Helper
{
   /// <summary>
   /// 文件夹助手类
   /// </summary>
   public static class DirectoryHelper
   {
      #region 创建目录
      /// <summary>
      /// 创建目录
      /// </summary>
      /// <param name="dirPath"></param>
      public static void CreateDirectory(string dirPath)
      {
         if (!IsExistDirectory(dirPath))
            Directory.CreateDirectory(dirPath);
      }
      #endregion

      #region 检测指定目录是否存在
      /// <summary>
      /// 检测指定目录是否存在
      /// </summary>
      /// <param name="directoryPath">目录的绝对路径</param>        
      public static bool IsExistDirectory(string directoryPath)
      {
         return Directory.Exists(directoryPath);
      }
      #endregion

      #region 检测指定目录是否为空
      /// <summary>
      /// 检测指定目录是否为空
      /// </summary>
      /// <param name="directoryPath">指定目录的绝对路径</param>        
      public static bool IsEmptyDirectory(string directoryPath)
      {
         try
         {
            //判断是否存在文件
            string[] fileNames = FileHelper.GetFileNames(directoryPath);
            if (fileNames.Length > 0)
            {
               return false;
            }

            //判断是否存在文件夹
            string[] directoryNames = GetDirectories(directoryPath);
            if (directoryNames.Length > 0)
            {
               return false;
            }

            return true;
         }
         catch (Exception ex)
         {
            return true;
         }
      }
      #endregion

      #region 获取指定目录中的子目录列表
      /// <summary>
      /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
      /// </summary>
      /// <param name="directoryPath">指定目录的绝对路径</param>        
      public static string[] GetDirectories(string directoryPath)
      {
         try
         {
            return Directory.GetDirectories(directoryPath);
         }
         catch (IOException ex)
         {
            throw ex;
         }
      }
      #endregion
   }
}
