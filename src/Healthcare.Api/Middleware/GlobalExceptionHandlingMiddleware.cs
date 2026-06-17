using FluentValidation;
using Healthcare.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Healthcare.Api.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An exception occurred while processing the request.");

                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is ValidationException validationException)
            {
                await HandleValidationExceptionAsync(context, validationException);
                return;
            }

            var statusCode = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                BadRequestException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };

            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = GetTitle(statusCode),
                Detail = statusCode == HttpStatusCode.InternalServerError
                    ? "An unexpected error occurred."
                    : exception.Message,
                Instance = context.Request.Path
            };

            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problemDetails);
        }

        private static async Task HandleValidationExceptionAsync(
            HttpContext context,
            ValidationException exception)
        {
            var errors = exception.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.ErrorMessage).ToArray());

            var validationProblemDetails = new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed",
                Instance = context.Request.Path
            };

            validationProblemDetails.Extensions["traceId"] = context.TraceIdentifier;

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(validationProblemDetails);
        }

        private static string GetTitle(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => "Bad request",
                HttpStatusCode.Unauthorized => "Unauthorized",
                HttpStatusCode.NotFound => "Resource not found",
                _ => "Server error"
            };
        }
    }
}
