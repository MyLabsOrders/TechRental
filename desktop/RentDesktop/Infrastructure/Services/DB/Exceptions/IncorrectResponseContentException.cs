using System;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal class IncorrectResponseContentException : ApplicationException
    {
        public IncorrectResponseContentException(string contentName, string? message = null, Exception? innerException = null)
            : base(message ?? $"{contentName} is null", innerException)
        {
            ContentName = contentName;
        }

        public string ContentName { get; set; }
    }
}
