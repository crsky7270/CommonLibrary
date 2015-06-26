/*----------------------------------------------------------------
//
// 文件名：         SerializeHelper.cs
// 文件功能描述：   序列化和反序列化集合
//
// 修改标识：
// 修改描述：
// 
// 备注：
 * 几种序列化方式的比较：   
  BinaryFormatter对象： 提供了一种高效的办法，能够以缩略型二进制格式持久化一个对象。所以序列化和反序列化操作的速度都非常快。   
  SoapFormatter对象：   以可读的XML格式持久化数据。可以很容易的通过Http发送到另一个应用程序。(此序列化方式的效率较低)   
  XML序列化：           可以在XML流中持久化一个对象的状态。但局限于：仅使用于公共类。只有公共字段和属性可以被序列化。若一个
                        对象图包含循环引用，它将不能被序列化。
// 				
//----------------------------------------------------------------*/
using System;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using Crsky.Utility.Helper;

namespace Crsky.Utility.Helper
{
    /// <summary>
    /// 对象序列化操作类
    /// </summary>    
    public class SerializeHelper
    {
        #region 文件流中序列化[永久存储]

        #region 将对象序列化到XML文件中
        /// <summary>
        /// 将指定类型的对象序列化为XML文档并保存到本地磁盘文件中
        /// </summary>
        /// <typeparam name="T">要序列化的类，即obj的类名</typeparam>
        /// <param name="instance">要序列化的对象</param>
        /// <param name="xmlFile">Xml文件名，表示保存序列化数据的位置</param>
        public static void XmlSerializeToXML<T>(object instance, string xmlFile)
        {
            try
            {
                //创建XML序列化对象
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                //创建文件流
                using (FileStream fs = new FileStream(xmlFile, FileMode.Create))
                {
                    //开始序列化对象
                    serializer.Serialize(fs, instance);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 将XML文件反序列化为对象
        /// <summary>
        /// 将XML文件反序列化为对象
        /// </summary>
        /// <typeparam name="T">要获取的类</typeparam>
        /// <param name="xmlFile">Xml文件名，即保存序列化数据的位置</param>        
        public static T DeSerializeFromXML<T>(string xmlFile)
        {
            try
            {
                //创建XML序列化对象
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                //创建文件流
                using (FileStream fs = new FileStream(xmlFile, FileMode.Open))
                {
                    //开始反序列化对象
                    return ConvertHelper.ConvertTo<T>(serializer.Deserialize(fs));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 将对象序列化到二进制文件中
        /// <summary>
        /// 将对象序列化到二进制文件中
        /// </summary>
        /// <param name="instance">要序列化的对象</param>
        /// <param name="fileName">文件名，保存二进制序列化数据的位置,后缀一般为.bin</param>
        public static void SerializeToBinary(object instance, string fileName)
        {
            try
            {
                //创建二进制序列化对象
                BinaryFormatter serializer = new BinaryFormatter();

                //创建文件流
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    //开始序列化对象
                    serializer.Serialize(fs, instance);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 将二进制文件反序列化为对象
        /// <summary>
        /// 将二进制文件反序列化为对象
        /// </summary>
        /// <typeparam name="T">要获取的类</typeparam>
        /// <param name="fileName">文件名，保存二进制序列化数据的位置</param>        
        public static T DeSerializeFromBinary<T>(string fileName)
        {
            try
            {
                //创建二进制序列化对象
                BinaryFormatter serializer = new BinaryFormatter();

                //创建文件流
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    //开始反序列化对象
                    return ConvertHelper.ConvertTo<T>(serializer.Deserialize(fs));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 将对象序列化到Soap格式的XML文件中
        /// <summary>
        /// 将对象序列化到Soap格式的XML文件中
        /// <remarks>
        /// 此序列化方式的效率相对较低，请优先使用二进制序列化与XML序列化方式处理
        /// </remarks>
        /// </summary>        
        /// <param name="instance">要序列化的对象</param>
        /// <param name="xmlFile">Xml文件名，表示保存序列化数据的位置</param>
        public static void SerializeToSoapXML(object instance, string xmlFile)
        {
            try
            {
                //创建Soap序列化对象
                SoapFormatter serializer = new SoapFormatter();

                //创建文件流
                using (FileStream fs = new FileStream(xmlFile, FileMode.Create))
                {
                    //开始序列化对象
                    serializer.Serialize(fs, instance);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 将Soap格式的XML文件反序列化为对象
        /// <summary>
        /// 将Soap格式的XML文件反序列化为对象
        /// <remarks>
        /// 此序列化方式的效率相对较低，请优先使用二进制序列化与XML序列化方式处理
        /// </remarks>
        /// </summary>
        /// <typeparam name="T">要获取的类</typeparam>
        /// <param name="xmlFile">Xml文件名，即保存序列化数据的位置</param>        
        public static T DeSerializeFromSoapXML<T>(string xmlFile)
        {
            try
            {
                //创建Soap序列化对象
                SoapFormatter serializer = new SoapFormatter();

                //创建文件流
                using (FileStream fs = new FileStream(xmlFile, FileMode.Open))
                {
                    //开始反序列化对象
                    return ConvertHelper.ConvertTo<T>(serializer.Deserialize(fs));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #endregion

        #region 内存流中序列化

        #region 将对象序列化到字节数组中后直接输出序列化后的字节数组
        /// <summary>
        /// 将对象序列化到字节数组中后直接输出序列化后的字节数组
        /// </summary>
        /// <param name="instance">要序列化的对象</param>        
        public static byte[] SerializeToBytes(object instance)
        {
            try
            {
                //序列化后的字节数组
                byte[] buffer;

                //创建二进制序列化对象
                BinaryFormatter serializer = new BinaryFormatter();

                //创建一个内存流
                using (MemoryStream ms = new MemoryStream())
                {
                    //将对象序列化到内存流中
                    serializer.Serialize(ms, instance);

                    //重置内存流的当前位置
                    ms.Position = 0;

                    //初始化缓冲区
                    buffer = new byte[ms.Length];

                    //读取内存流数据到缓冲区中
                    ms.Read(buffer, 0, buffer.Length);

                    return buffer;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 将字节数组反序列化为对象
        /// <summary>
        /// 将字节数组反序列化为对象
        /// </summary>
        /// <param name="buffer">要反序列化的字节数组</param>        
        public static object DeSerializeFromBytes(byte[] buffer)
        {
            try
            {
                //创建二进制序列化对象
                BinaryFormatter serializer = new BinaryFormatter();

                //创建一个内存流
                using (MemoryStream ms = new MemoryStream())
                {
                    //将缓冲区数据写入内存流
                    ms.Write(buffer, 0, buffer.Length);

                    //重置内存流的当前位置
                    ms.Position = 0;

                    //开始反序列化对象
                    return serializer.Deserialize(ms);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 将字节数组反序列化为对象
        /// </summary>
        /// <typeparam name="T">要获取的类</typeparam>
        /// <param name="buffer">要反序列化的字节数组</param>        
        public static T DeSerializeFromBytes<T>(byte[] buffer)
        {
            return ConvertHelper.ConvertTo<T>(DeSerializeFromBytes(buffer));
        }
        #endregion

        #region 将指定类型的对象序列化为XML文档后直接输出序列化后的XML文档
        /// <summary>
        /// 将指定类型的对象序列化为XML文档后直接输出序列化后的XML文档
        /// </summary>
        /// <param name="obj">可序列化的对象</param>
        /// <returns>序列化后的XML实例文档</returns>
        public static string SerializeToXml(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                stream.Close();

#warning 待测试"Encoding.UTF8.GetString(stream.ToArray())"有无问题

                //return Encoding.UTF8.GetString(stream.ToArray());
                return Convert.ToBase64String(stream.ToArray());
            }
        }
        #endregion

        #region 将XML文档反序列化为指定类型的对象
        /// <summary>
        /// 将XML文档反序列化为指定类型的对象
        /// </summary>
        /// <param name="xml">XML实例文档</param>
        /// <param name="type">XmlSerializer可序列化的对象的类型</param>
        /// <returns>反序列化后的指定类型的对象</returns>
        /// <remarks>
        /// 调用示例：XmlDeserialize(xml,typeof(ClassA));
        /// </remarks>
        public static T DeserializeFromXml<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

#warning 待测试"Encoding.UTF8.GetBytes(xml)"有无问题
            //using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(xml)))
            {
                return ConvertHelper.ConvertTo<T>(serializer.Deserialize(stream));
            }
        }
        #endregion

        #endregion
    }
}
