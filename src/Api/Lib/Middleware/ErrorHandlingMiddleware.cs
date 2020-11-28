using NetCoreExample.Server.Lib.Extensions;
using NetCoreExample.Server.Models.DTOs.Error;
using NetCoreExample.Server.Models.DTOs.Error.Response;
using Lib.Exceptions;
using Lib.Exceptions.Enums;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetCoreExample.Server.Api.Lib.Middleware {
    public class ErrorHandlingMiddleware {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next) {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, TelemetryClient telemetryClient, IHttpContextAccessor httpContentAccessor) {
            try {
                await next(context);
            } catch (Exception ex) {
                telemetryClient.TrackException(ex, new Dictionary<string, string> {
                    { "ExceptionMessage", ex.Message },
                    { "UserId", httpContentAccessor.HttpContext.User?.GetId().ToString() }
                });
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception) {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (exception is EntityNotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }
            else if (exception is InvalidOperationException 
                     || exception is EmailAlreadyExistsException 
                     || exception is EntityCreationException 
                     || exception is InvalidEntityException
                     || exception is InvalidCredentialException
                     || exception is InvalidDataException)
            {
                code = HttpStatusCode.BadRequest;
            } 

            var reason = ErrorReason.InvalidData;
            switch (exception) {
                case NotAllowedException e:
                    reason = ErrorReason.Unauthorized;
                    break;
                case NetCoreExampleException e:
                    reason = ErrorReason.ServerError;
                    break;

            }
            if (exception is NotAllowedException)
                reason = ErrorReason.Unauthorized;

            var errorResponse = new ErrorResponse(exception.Message, reason);

            if (exception is DbUpdateException)
            {
                errorResponse.Reason = ErrorReason.ValidationError;
                errorResponse.Details = exception.InnerException.Message;
                code = HttpStatusCode.BadRequest;
            }
            

            var result = JsonConvert.SerializeObject(errorResponse, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
