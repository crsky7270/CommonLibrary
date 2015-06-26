using System.IO;

namespace Crsky.Utility.Helper
{
   public static class FileHelper
   {
      /// <summary>
      /// 创建新的文件
      /// </summary>
      /// <param name="dirPath">目录路径</param>
      public static void CreateNewFile(string dirPath)
      {

      }

      /// <summary>
      /// Create And Recover Txt File
      /// </summary>
      /// <param name="filePath">文件的完整路径</param>
      public static void CreateTxtFile(string filePath)
      {
         var dirName = System.IO.Path.GetDirectoryName(filePath);
         DirectoryHelper.CreateDirectory(dirName);
         File.CreateText(filePath);
      }

      #region 获取指定目录中的文件列表
      /// <summary>
      /// 获取指定目录中所有文件列表
      /// </summary>
      /// <param name="directoryPath">指定目录的绝对路径</param>        
      public static string[] GetFileNames(string directoryPath)
      {
         //如果目录不存在，则抛出异常
         if (!DirectoryHelper.IsExistDirectory(directoryPath))
         {
            throw new FileNotFoundException();
         }

         //获取文件列表
         return Directory.GetFiles(directoryPath);
      }

      /// <summary>
      /// 获取指定目录及子目录中所有文件列表
      /// </summary>
      /// <param name="directoryPath">指定目录的绝对路径</param>
      /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
      /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
      /// <param name="isSearchChild">是否搜索子目录</param>
      public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
      {
         //如果目录不存在，则抛出异常
         if (!DirectoryHelper.IsExistDirectory(directoryPath))
         {
            throw new FileNotFoundException();
         }

         try
         {
            return Directory.GetFiles(directoryPath, searchPattern, isSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
         }
         catch (IOException ex)
         {
            throw ex;
         }
      }
      #endregion

      #region 获取一个文件的长度
      /// <summary>
      /// 获取一个文件的长度,单位为Byte
      /// </summary>
      /// <param name="filePath">文件的绝对路径</param>        
      public static int GetFileSize(string filePath)
      {
         //创建一个文件对象
         FileInfo fi = new FileInfo(filePath);

         //获取文件的大小
         return (int)fi.Length;
      }

      /// <summary>
      /// 获取一个文件的长度,单位为KB
      /// </summary>
      /// <param name="filePath">文件的路径</param>        
      public static double GetFileSizeByKB(string filePath)
      {
         //创建一个文件对象
         FileInfo fi = new FileInfo(filePath);

         //获取文件的大小
         return ConvertHelper.ToDouble(ConvertHelper.ToDouble(fi.Length) / 1024, 1);
      }

      /// <summary>
      /// 获取一个文件的长度,单位为MB
      /// </summary>
      /// <param name="filePath">文件的路径</param>        
      public static double GetFileSizeByMB(string filePath)
      {
         //创建一个文件对象
         FileInfo fi = new FileInfo(filePath);

         //获取文件的大小
         return ConvertHelper.ToDouble(ConvertHelper.ToDouble(fi.Length) / 1024 / 1024, 1);
      }
      #endregion
   }
}
