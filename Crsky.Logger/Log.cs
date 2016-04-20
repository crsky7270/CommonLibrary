using System;
using log4net;

namespace Crsky.Logger
{
    public class Log
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="messageType">日志类型</param>
        public static void Write(string message, MessageType messageType)
        {
            Write(message, messageType, Type.GetType("System.Object"), null);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="messageType">日志类型</param>
        /// <param name="type">配置类型</param>
        public static void Write(string message, MessageType messageType, Type type)
        {
            Write(message, messageType, type, null);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="messageType">日志类型</param>
        /// <param name="ex">异常</param>
        /// <param name="type">配置类型</param>
        public static void Write(string message, MessageType messageType, Type type, Exception ex)
        {
            ILog log = LogManager.GetLogger(type);
            switch (messageType)
            {
                case MessageType.Debug: if (log.IsDebugEnabled) { log.Debug(message, ex); } break;
                case MessageType.Info:  if (log.IsInfoEnabled)  { log.Info(message, ex);  } break;
                case MessageType.Warn:  if (log.IsWarnEnabled)  { log.Warn(message, ex);  } break;
                case MessageType.Error: if (log.IsErrorEnabled) { log.Error(message, ex); } break;
                case MessageType.Fatal: if (log.IsFatalEnabled) { log.Fatal(message, ex); } break;
            }
        }

        /// <summary>
        /// 断言
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="message">日志信息</param>
        public static void Assert(bool condition, string message)
        {
            Assert(condition, message, Type.GetType("System.Object"));
        }

        /// <summary>
        /// 断言
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="message">日志信息</param>
        /// <param name="type">日志类型</param>
        public static void Assert(bool condition, string message, Type type)
        {
            if (!condition)
            {
                Write(message, MessageType.Info, type, null);
            }
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 调试
        /// </summary>
        Debug,
        /// <summary>
        /// 信息
        /// </summary>
        Info,
        /// <summary>
        /// 警告
        /// </summary>
        Warn,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 致命错误
        /// </summary>
        Fatal
    }
}
