using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Crsky.Utility.Helper
{
    /// <summary>
    /// json扩展方法
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// DataTable转换成Json格式
        /// </summary>
        /// <param name="dt">要转换的DataTable</param>        
        /// <returns>Json字符串</returns>
        public static string DataTableToJson(this DataTable dt)
        {
            if (dt == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"");
            sb.Append(dt.TableName);
            sb.Append("\":[");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("{");
                foreach (DataColumn c in dt.Columns)
                {
                    sb.Append("\"");
                    sb.Append(c.ColumnName);
                    sb.Append("\":\"");
                    sb.Append(r[c].ToString());
                    sb.Append("\",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]}");
            return sb.ToString();
        }

        /// <summary>
        /// DataTable转换成Json格式
        /// </summary>
        /// <param name="dt">要转换的DataTable</param>
        /// <param name="fields">需要的字段</param>
        /// <returns>Json字符串</returns>
        public static string DataTableToJson(this DataTable dt, string[] fields)
        {
            if (dt == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"");
            sb.Append(dt.TableName);
            sb.Append("\":[");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("{");
                foreach (string t in fields)
                {
                    sb.Append("\"" + t + "\":\"" + r[t] + "\",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]}");
            return sb.ToString();
        }

        /// <summary>
        /// 格式化成Json字符串
        /// </summary>
        /// <param name="obj">需要格式化的对象</param>
        /// <returns>Json字符串</returns>
        public static string ObjectToJson(this object obj)
        {
            // 首先，当然是JSON序列化
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());

            // 定义一个stream用来存发序列化之后的内容
            using (Stream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);

                // 从头到尾将stream读取成一个字符串形式的数据，并且返回
                stream.Position = 0;
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }


        public static string ObjectToJson<T>(string jsonName, IList<T> IL)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"" + jsonName + "\":[");
            if (IL.Count > 0)
            {
                for (int i = 0; i < IL.Count; i++)
                {
                    PropertyInfo[] properties = Activator.CreateInstance<T>().GetType().GetProperties();
                    builder.Append("{");
                    for (int j = 0; j < properties.Length; j++)
                    {
                        builder.Append(
                            string.Concat(new object[]
                                {
                                    "\"", properties[j].Name, "\":\"", properties[j].GetValue(IL[i], null) == null ? null: properties[j].GetValue(IL[i], null).ToString().Replace("\"", "\\\"").Replace("\n","\\n").Replace("\t","\\t"), "\""
                                }));
                        if (j < (properties.Length - 1))
                        {
                            builder.Append(",");
                        }
                    }
                    builder.Append("}");
                    if (i < (IL.Count - 1))
                    {
                        builder.Append(",");
                    }
                }
            }
            builder.Append("]}");
            return builder.ToString();
        }


    }
}
