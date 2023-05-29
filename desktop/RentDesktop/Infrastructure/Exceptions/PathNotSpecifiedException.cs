using System;

namespace RentDesktop.Infrastructure.Exceptions
{
    internal class PathNotSpecifiedException : ApplicationException
    {
        public PathNotSpecifiedException(string? message = null, Exception? innerException = null)
            : base(message ?? "The path to the file is not specified.", innerException)
        {
        }
    }
}
