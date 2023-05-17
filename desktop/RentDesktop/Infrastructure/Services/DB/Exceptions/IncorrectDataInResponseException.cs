using System;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal class IncorrectDataInResponseException : ApplicationException
    {
        public IncorrectDataInResponseException(string dataName, string? message = null, Exception? innerException = null)
            : base(message ?? $"{dataName} is incorrect", innerException)
        {
            DataName = dataName;
        }

        public string DataName { get; }
    }
}
