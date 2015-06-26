using System;
using System.Text;
using log4net;

namespace Crsky.IoC
{
   /// <summary>
   /// IOC Mapping Class
   /// </summary>
    public class BootstrapLogger
    {
        public string Output { get { return Logger.ToString(); } }
        protected StringBuilder Logger { get; private set; }
        protected static ILog Log4Net = LogManager.GetLogger(typeof(BootstrapLogger));
        public BootstrapLogger(StringBuilder stringBuilder)
        {
            Logger = stringBuilder;
        }
        private string FormattedUtcNow { get { return DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss.fff UTC"); } }
        public void Log(string messageFormat, params object[] pReplacements)
        {
            Logger.AppendLine(FormattedUtcNow + ": " + string.Format(messageFormat, pReplacements));
            Log4Net.InfoFormat(messageFormat, pReplacements);
        }
        public void Error(Exception ex, string messageFormat, params object[] pReplacements)
        {
            Log4Net.Error(string.Format(messageFormat, pReplacements), ex);
            Error(ex);   
            Log(messageFormat, pReplacements);
        }

        private const int MaxExceptions = 10;
        public void Error(Exception ex)
        {
            Logger.AppendLine(FormattedUtcNow + ": " + ex.Message);
            var lastMessage = ex.Message;
            var innerException = ex.InnerException;
            var exceptions = 0;
            while (innerException != null && exceptions < MaxExceptions)
            {
                if (!innerException.Message.Equals(lastMessage))
                {
                    Logger.AppendLine(FormattedUtcNow + " InnerException: " + innerException.Message);
                }
                lastMessage = innerException.Message;
                innerException = innerException.InnerException;
                exceptions++;
            }
        }
    }
}