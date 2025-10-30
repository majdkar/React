using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SchoolV01.Application.Exceptions;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Server.Middlewares
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred: {Message}, Time of occurrence {time}", exception.Message, DateTime.UtcNow);

            var response = httpContext.Response;
            response.ContentType = "application/json";
            var responseModel = Result<string>.Fail(exception.Message);
            response.StatusCode = exception switch
            {
                ApiException => (int)HttpStatusCode.BadRequest,// custom application error
                KeyNotFoundException => (int)HttpStatusCode.NotFound,// not found error
                _ => (int)HttpStatusCode.InternalServerError,// unhandled error
            };
            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result, cancellationToken);
            return true;
        }
    }
}
