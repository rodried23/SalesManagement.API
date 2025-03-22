using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SalesManagement.API.Models;
using SalesManagement.Domain.Exceptions;

namespace SalesManagement.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            HttpStatusCode statusCode;
            ErrorResponse errorResponse;

            switch (context.Exception)
            {
                case DomainException domainException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorResponse = new ErrorResponse
                    {
                        Type = "DomainError",
                        Error = "Business rule violation",
                        Detail = domainException.Message
                    };
                    break;

                case UnauthorizedAccessException _:
                    statusCode = HttpStatusCode.Unauthorized;
                    errorResponse = new ErrorResponse
                    {
                        Type = "AuthenticationError",
                        Error = "Unauthorized access",
                        Detail = "You are not authorized to access this resource"
                    };
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    errorResponse = new ErrorResponse
                    {
                        Type = "ServerError",
                        Error = "Internal server error",
                        Detail = "An unexpected error occurred"
                    };
                    _logger.LogError(context.Exception, "Unhandled exception: {Message}", context.Exception.Message);
                    break;
            }

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = (int)statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}