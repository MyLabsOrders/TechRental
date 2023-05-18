using System;
using System.Net;
using System.Net.Http;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal class ErrorResponseException : ApplicationException
    {
        public ErrorResponseException(HttpResponseMessage response, string? message = null, Exception? innerException = null)
            : this(response.StatusCode, ResponseAnalyzeService.GetErrorReason(response), message, innerException)
        {
        }

        public ErrorResponseException(HttpStatusCode statusCode, string reason, string? message = null, Exception? innerException = null)
            : base(message ?? $"Status: {statusCode}\n{reason}", innerException)
        {
            StatusCode = statusCode;
            Reason = reason;
        }

        public HttpStatusCode StatusCode { get; }
        public string Reason { get; }
    }
}
