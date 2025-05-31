using Ecommorce.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecommorce.API.Middleware
{
    public class ExecptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);


        public ExecptionsMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache cache)
        {
            _next = next;
            _environment = environment;
            _cache = cache;

        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplySecurity(context);


                if (IsRequestAllowed(context) == false)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";

                    var response = new ApiExecption((int)HttpStatusCode.TooManyRequests, "To Many Requests .Please Try Again Later");
                    await context.Response.WriteAsJsonAsync(response);
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = _environment.IsDevelopment() ?
                    new ApiExecption((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new ApiExecption((int)HttpStatusCode.InternalServerError, ex.Message);
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);

            }

        }
        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();
            var cachKey = $"Rate:{ip}";
            var dateNow = DateTime.Now;
            var (timesTamp, count) = _cache.GetOrCreate(cachKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (timesTamp: dateNow, count: 0);
            });

            if (dateNow - timesTamp < _rateLimitWindow)
            {
                if (count >= 20)
                {
                    return false;
                }
                _cache.Set(cachKey, (timesTamp, count += 1), _rateLimitWindow);
            }
            else
            {
                _cache.Set(cachKey, (timesTamp: dateNow, count), _rateLimitWindow);

            }
            return true;
        }
        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Protection"] = "1;mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";
        }


    }
}

