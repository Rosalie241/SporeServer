using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SporeServer.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SporeServer.Middleware
{
    public class SporeClientMiddleware
    {
        private readonly RequestDelegate _next;

        public SporeClientMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            
            await _next(context);
        }
    }
}
