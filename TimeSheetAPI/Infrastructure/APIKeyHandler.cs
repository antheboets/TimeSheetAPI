using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetAPI.Infrastructure
{
    public class APIKeyHandler
    {
        private readonly RequestDelegate requestDelegate;
        private readonly IConfiguration Config;
        private readonly string APIKeyToCheck;
        private const string headerKey = "APIKey";
        public APIKeyHandler(RequestDelegate requestDelegate, IConfiguration Config)
        {
            this.requestDelegate = requestDelegate;
            this.Config = Config;
            APIKeyToCheck = Config.GetSection("APIKey:key").Value;
  
        }
        public async Task Invoke(HttpContext context)
        {
            bool validKey = false;
            if (context.Request.Headers.ContainsKey(headerKey))
            {
                if (APIKeyToCheck.Contains(context.Request.Headers[headerKey].FirstOrDefault()))
                {
                    validKey = true;
                }
            }
            if (validKey)
            {
                await requestDelegate.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Invalid API key");
            }
        }
    }
    public static class MyHandlerExtensions
    {
        public static IApplicationBuilder UseAPIKeyHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<APIKeyHandler>();
        }
    }
}
