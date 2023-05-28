using RentDesktop.Models;
using System.IO;
using System.Net.Http;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class FileDownloadService
    {
        public static MemoryStream DownloadCheque(IOrder order)
        {
            using var db = new DatabaseConnectionService();

            string getChequeHandle = $"/api/Order/cheque?orderTime={order.ID}";
            using HttpResponseMessage getChequeResponse = db.GetAsync(getChequeHandle).Result;

            if (!getChequeResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(getChequeResponse);

            var chequeBytes = getChequeResponse.Content.ReadAsByteArrayAsync().Result;
            return new MemoryStream(chequeBytes);
        }

        public static MemoryStream DownloadInvoice(IOrder order)
        {
            using var db = new DatabaseConnectionService();

            string getInvoiceHandle = $"/api/Order/invoice?orderTime={order.ID}";
            using HttpResponseMessage getInvoiceResponse = db.GetAsync(getInvoiceHandle).Result;

            if (!getInvoiceResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(getInvoiceResponse);

            var invoiceBytes = getInvoiceResponse.Content.ReadAsByteArrayAsync().Result;
            return new MemoryStream(invoiceBytes);
        }
    }
}
