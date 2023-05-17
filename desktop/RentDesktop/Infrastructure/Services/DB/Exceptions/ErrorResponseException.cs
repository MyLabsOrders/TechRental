using System;
using System.Net;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal class ErrorResponseException : ApplicationException
    {
        public ErrorResponseException(HttpStatusCode statusCode, string? message = null, Exception? innerException = null)
            : base(message ?? $"Error with status code {statusCode}", innerException)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; set; }
    }
}
