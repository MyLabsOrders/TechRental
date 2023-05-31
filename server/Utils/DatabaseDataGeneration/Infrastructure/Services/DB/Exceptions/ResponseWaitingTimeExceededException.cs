namespace RentDesktop.Infrastructure.Services.DB.Exceptions
{
    internal class ResponseWaitingTimeExceededException : ApplicationException
    {
        public ResponseWaitingTimeExceededException(int waitingTime, string? message = null, Exception? innerException = null)
            : base(message ?? "The maximum response waiting time has been exceeded.", innerException)
        {
            WaitingTime = waitingTime;
        }

        public int WaitingTime { get; }
    }
}
