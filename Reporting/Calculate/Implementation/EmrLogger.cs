using Reporting.Calculate.Interface;
using Helper.Common;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Infrastructure.Logger;
using System;

namespace Reporting.Calculate.Implementation
{
    public class EmrLogger : IEmrLogger
    {
        private readonly ILogger<EmrLogger> _logger;
        private readonly string _tenantInfo;
        private readonly ILoggingHandler _loggingHandler;
        public EmrLogger(ILogger<EmrLogger> logger, ITenantProvider tenantProvider, ILoggingHandler loggingHandler)
        {
            _logger = logger;
            _tenantInfo = tenantProvider.GetTenantInfo();
            _loggingHandler = loggingHandler;
        }

        public void WriteLogEnd(object className, string functionName, string message)
        {
            string prefix = "<E";
            string messageType = "[INFO]";
            WriteLogToConsole(messageType, prefix, className.GetType().ToString(), functionName, message);
            _loggingHandler.WriteLogEndAsync($"ClassName: {className} FunctionName: {functionName} Message: {message}");
        }

        public void WriteLogError(object className, string functionName, Exception exception)
        {
            string prefix = "";
            string messageType = "[ERROR]";
            WriteLogToConsole(messageType, prefix, className.GetType().ToString(), functionName, exception.Message);
            _loggingHandler.WriteLogExceptionAsync(exception, $"ClassName: {className} FunctionName: {functionName}");
        }

        public void WriteLogStart(object className, string functionName, string message)
        {
            string prefix = ">S";
            string messageType = "[INFO]";
            WriteLogToConsole(messageType, prefix, className.GetType().ToString(), functionName, message);
            _loggingHandler.WriteLogStartAsync($"ClassName: {className} FunctionName: {functionName} Message: {message}");
        }

        public void WriteLogMsg(object className, string functionName, string message)
        {
            string prefix = "";
            string messageType = "[INFO]";
            WriteLogToConsole(messageType, prefix, className.GetType().ToString(), functionName, message);
            _loggingHandler.WriteLogMessageAsync($"ClassName: {className} FunctionName: {functionName} Message: {message}");
        }

        private void WriteLogToConsole(string messageType, string prefix, string className, string functionName, string message)
        {
            string dateTime = CIUtil.GetJapanDateTimeNow().ToString("MM/dd/yyyy HH:mm:ss.fff");
            string logContent = $"{_tenantInfo} {messageType} {dateTime} {prefix} {className}.{functionName} {message}";
            _logger.LogInformation(logContent);
        }
    }
}
