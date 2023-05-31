using RentDesktop.Infrastructure.Services.DB.Exceptions;
using RentDesktop.Models;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class FileDownloadService
    {
        private const int RESPONSE_WAIT_TIME_MILLISECONDS = 3000;

        public static MemoryStream DownloadCheque(IOrder order)
        {
            using var db = new DatabaseConnectionService();

            string getChequeHandle = $"/api/Order/cheque?orderTime={order.DateOfCreationStamp}";
            Task<HttpResponseMessage> getChequeTask = db.GetAsync(getChequeHandle);

            if (!getChequeTask.Wait(RESPONSE_WAIT_TIME_MILLISECONDS))
                throw new ResponseWaitingTimeExceededException(RESPONSE_WAIT_TIME_MILLISECONDS);

            using HttpResponseMessage getChequeResponse = getChequeTask.Result;

            if (!getChequeResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(getChequeResponse);

            var chequeBytes = getChequeResponse.Content.ReadAsByteArrayAsync().Result;
            return new MemoryStream(chequeBytes);
        }

        public static MemoryStream DownloadInvoice(IOrder order)
        {
            using var db = new DatabaseConnectionService();

            string getInvoiceHandle = $"/api/Order/invoice?orderTime={order.DateOfCreationStamp}";
            Task<HttpResponseMessage> getInvoiceTask = db.GetAsync(getInvoiceHandle);

            if (!getInvoiceTask.Wait(RESPONSE_WAIT_TIME_MILLISECONDS))
                throw new ResponseWaitingTimeExceededException(RESPONSE_WAIT_TIME_MILLISECONDS);

            using HttpResponseMessage getInvoiceResponse = getInvoiceTask.Result;

            if (!getInvoiceResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(getInvoiceResponse);

            var invoiceBytes = getInvoiceResponse.Content.ReadAsByteArrayAsync().Result;
            return new MemoryStream(invoiceBytes);
        }
    }
}
