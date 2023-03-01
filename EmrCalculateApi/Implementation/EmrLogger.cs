using EmrCalculateApi.Interface;
using Infrastructure.Interfaces;
using System;

namespace EmrCalculateApi.Implementation
{
    public class EmrLogger : IEmrLogger
    {
        private readonly ILogger<EmrLogger> _logger;
        private readonly string _tenantInfo;
        public EmrLogger(ILogger<EmrLogger> logger, ITenantProvider tenantProvider)
        {
            _logger = logger;
            _tenantInfo = tenantProvider.GetTenantInfo();
        }

        public void WriteLogEnd(object className, string functionName, string message)
        {
            string prefix = "<E";
            string messageType = "[INFO]";
            WriteLog(messageType, prefix, className.GetType().ToString(), functionName, message);
        }

        public void WriteLogError(object className, string functionName, Exception exception)
        {
            string prefix = "";
            string messageType = "[ERROR]";
            WriteLog(messageType, prefix, className.GetType().ToString(), functionName, exception.Message);
        }

        public void WriteLogStart(object className, string functionName, string message)
        {
            string prefix = ">S";
            string messageType = "[INFO]";
            WriteLog(messageType, prefix, className.GetType().ToString(), functionName, message);
        }

        public void WriteLogMsg(object className, string functionName, string message)
        {
            string prefix = "";
            string messageType = "[INFO]";
            WriteLog(messageType, prefix, className.GetType().ToString(), functionName, message);
        }

        private void WriteLog(string messageType, string prefix, string className, string functionName, string message)
        {
            string dateTime = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss.fff");
            string logContent = $"{_tenantInfo} {messageType} {dateTime} {prefix} {className}.{functionName} {message}";
            _logger.LogInformation(logContent);
        }
    }
}
