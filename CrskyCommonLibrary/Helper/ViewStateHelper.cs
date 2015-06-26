/*----------------------------------------------------------------
//
// 文件名：         ViewStateHelper.cs
// 文件功能描述：   ViewState压缩与解压缩
//
// 
// 创建标识：       
//
// 修改标识：       
// 修改描述：       
// 
// 备注：           此类只有在服务器未启用GZIP压缩及未对网页进行任何压缩处理时方可使用
// 				
//----------------------------------------------------------------*/
using System.IO;
using System.IO.Compression;

namespace Crsky.Utility.Helper
{
    /// <summary>
    /// ViewState压缩与解压缩
    /// </summary>
    /// <remarks>
    /// 此类只有在服务器未启用GZIP压缩及未对网页进行任何压缩处理时方可使用
    /// </remarks>
    public class ViewStateHelper
    {
        /// <summary>
        /// ViewState压缩
        /// </summary>
        /// <param name="data">要压缩的数据</param>
        /// <returns>GZip压缩后的数据</returns>
        public static byte[] Compress(byte[] data)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream stream = new GZipStream(ms, CompressionMode.Compress);
            stream.Write(data, 0, data.Length);
            stream.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// ViewState解压缩
        /// </summary>
        /// <param name="data">要解压缩的数据</param>
        /// <returns>GZip解压缩后的数据</returns>
        public static byte[] Decompress(byte[] data)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, data.Length);
            ms.Position = 0;
            GZipStream stream = new GZipStream(ms, CompressionMode.Decompress);
            MemoryStream temp = new MemoryStream();
            byte[] buffer = new byte[1024];
            while (true)
            {
                int read = stream.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    break;
                }
                else
                {
                    temp.Write(buffer, 0, buffer.Length);
                }
            }
            stream.Close();
            return temp.ToArray();
        }
    }
}
