using RentDesktop.Models;
using RentDesktop.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class ShopService
    {
        public static List<Transport> GetTransports()
        {
            var transports = new List<Transport>();
            using var db = new DatabaseConnectionService();

            int currentPage = 1;
            IEnumerable<DbOrder> currentOrder;

            do
            {
                string getOrdersHandle = $"/api/Order?page={currentPage++}";
                using HttpResponseMessage getOrdersResponse = db.GetAsync(getOrdersHandle).Result;

                if (!getOrdersResponse.IsSuccessStatusCode)
                    throw new ErrorResponseException(getOrdersResponse);

                var orderCollection = getOrdersResponse.Content.ReadFromJsonAsync<DbOrderCollection>().Result;

                if (orderCollection is null || orderCollection.orders is null)
                    throw new IncorrectContentException(getOrdersResponse.Content);

                var transportsCollection = DatabaseModelConverterService.ConvertOrders(orderCollection.orders)
                    .Select(t => t.Models);

                foreach (var currTransports in transportsCollection)
                    transports.AddRange(currTransports);

                currentOrder = orderCollection.orders;
            }
            while (currentOrder.Any());

            return transports;
        }

        public static void AddTransport(ITransport transport)
        {
            using var db = new DatabaseConnectionService();

            const string addOrderHandle = "/api/Order";

            byte[] transportIconBytes = transport.Icon is null
                ? Array.Empty<byte>()
                : BitmapService.BitmapToBytes(transport.Icon);

            var content = new DbCreateOrder()
            {
                name = transport.Name,
                status = Order.AVAILABLE_STATUS,
                total = transport.Price,
                orderImage = BitmapService.BytesToString(transportIconBytes)
            };

            using HttpResponseMessage addOrderResponse = db.PostAsync(addOrderHandle, content).Result;

            if (!addOrderResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(addOrderResponse);
        }
    }
}
