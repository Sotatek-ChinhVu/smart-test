using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Infrastructure.Common
{
    public class GlobalExceptionFilters : IExceptionFilter
    {
        private readonly ILogger _logger;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public GlobalExceptionFilters(ILogger<GlobalExceptionFilters> logger, ITenantProvider tenantProvider)
        {
            _logger = logger;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public void OnException(ExceptionContext context)
        {
            try
            {
                if (!context.ExceptionHandled)
                {
                    var exception = context.Exception;
                    int statusCode;

                    switch (true)
                    {
                        case bool _ when exception is UnauthorizedAccessException:
                            statusCode = (int)HttpStatusCode.Unauthorized;
                            break;
                        case bool _ when exception is InvalidOperationException:
                            statusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        case bool _ when exception is TimeoutException:
                            statusCode = (int)HttpStatusCode.RequestTimeout;
                            break;
                        default:
                            statusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }

                    var message = $"GlobalExceptionFilter: Error in {context.ActionDescriptor.DisplayName}. {exception.Message}. Stack Trace: {exception.StackTrace}. {Environment.NewLine}" +
                                  $"{context.ActionDescriptor.Parameters}. {Environment.NewLine}" +
                                  $"{context.HttpContext}. {exception.Source}. {Environment.NewLine}" +
                                  $"{exception.InnerException?.Message ?? string.Empty}. {Environment.NewLine} {Environment.NewLine}" +
                                  $"{exception.TargetSite}";
                    _logger.LogError(message);

                    _loggingHandler.WriteLogExceptionAsync(context.Exception, message);
                    context.Result = new ObjectResult(exception.Message) { StatusCode = statusCode };
                }
            }
            finally
            {
                _tenantProvider.DisposeDataContext();
                _loggingHandler.Dispose();
            }
        }
    }
}
