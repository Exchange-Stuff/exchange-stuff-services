using ExchangeStuff.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using StackExchange.Redis;
using System.Net;
using System.Runtime.CompilerServices;

namespace ExchangeStuff.Exceptions
{
    public class ExceptionHandler : IExceptionFilter
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                HandlerException(context);
                context.ExceptionHandled = true;
            }
        }
        private void HandlerException(ExceptionContext context, [CallerMemberName] string methodName = "")
        {
            ResponseResult<string> responseView = new ResponseResult<string>
            {
                Error = new ErrorViewModel
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = context.Exception.Message
                },
                IsSuccess = false,
                Value = null!
            };
            if (context.Exception is ArgumentException ||
                context.Exception is ArgumentNullException ||
                context.Exception is Exception)
            {
                responseView.Error.Code = 400;
            }
            if (context.Exception is UnauthorizedAccessException)
            {
                responseView.Error.Code = 401;
            }
            if (context.Exception is SecurityTokenExpiredException || context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.HttpContext.Response!.Headers!.TryAdd("IS-LOOKEACH-TOKEN-EXPIRED", "true");
                responseView.Error.Message = "The token provided has expired.";
                responseView.Error.Code = 401;
            }
            if (context.Exception is RedisConnectionException)
            {
                responseView.Error.Code = 500;
                responseView.Error.Message = "Redis server down";
            }
            _logger.LogError(context.Exception, "An error occurred in {Controller}.{Action} with RequestId {RequestId}: {Message}", context.RouteData.Values["controller"]?.ToString(), context.RouteData.Values["action"]?.ToString(), context.HttpContext.TraceIdentifier, context.Exception.Message);

            context.Result = new ObjectResult(responseView)
            {
                StatusCode = responseView.Error.Code
            };
        }
    }
    public static class SerilogConfig
    {
        public static void AddLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .Enrich.WithEnvironmentName()
                        .WriteTo.Debug()
                        .WriteTo.Console()
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticConfig:Uri"] + ""))
                        {
                            IndexFormat = $"{builder.Configuration["ApplicationName"]}-logs-{builder.Environment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                            AutoRegisterTemplate = true,
                            NumberOfShards = 2,
                            NumberOfReplicas = 1
                        })
                        .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
                        .ReadFrom.Configuration(builder.Configuration)
                        .CreateLogger();
            builder.Host.UseSerilog();
        }
    }
}
