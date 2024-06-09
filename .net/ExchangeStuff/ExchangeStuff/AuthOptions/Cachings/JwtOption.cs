using ExchangeStuff.Service.Services.Interfaces;
using System.Net;

namespace ExchangeStuff.AuthOptions.Cachings
{
    public class JwtOption
    {
        private readonly RequestDelegate _next;

        public JwtOption(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ICacheService cacheService)
        {
            var endpoint = httpContext.GetEndpoint();
            var metadata = endpoint?.Metadata;
            if (metadata != null && metadata.FirstOrDefault(x => x.ToString()!.Contains("Authorize Policy")) == null!)
            {
                await _next(httpContext);
                return;
            }
            var token = (httpContext.Request.Headers.Authorization.FirstOrDefault() + "").Split(" ").Last();

            if (string.IsNullOrEmpty(token) || !(await cacheService.GetTokenValid(token)))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            await _next(httpContext);
        }
    }
}
