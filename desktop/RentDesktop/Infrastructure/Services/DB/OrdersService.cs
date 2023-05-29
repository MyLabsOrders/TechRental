using RentDesktop.Models;
using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class OrdersService
    {
        public static void ChangeOrderStatus(IOrder order, string newStatus)
        {
            using var db = new DatabaseConnectionService();

            const string changeOrderStatusHandle = "/api/Order/status";
            var content = new DbOrderStatus(order.ID, newStatus);

            using HttpResponseMessage changeOrderStatusResponse = db.PutAsync(changeOrderStatusHandle, content).Result;

            if (!changeOrderStatusResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(changeOrderStatusResponse);

            order.Status = newStatus;
        }

        public static Order CreateOrder(IEnumerable<TransportRent> cart, IUserInfo userInfo)
        {
            List<Tuple<TransportRent, int>> products = cart
                .GroupBy(t => t.Transport.ID)
                .Select(t => new Tuple<TransportRent, int>(t.First(), t.Count()))
                .ToList();

            return RegisterOrder(products, userInfo);
        }

        private static Order RegisterOrder(List<Tuple<TransportRent, int>> productsInfo, IUserInfo userInfo)
        {
            using var db = new DatabaseConnectionService();

            string addOrderHandle = $"/api/User/{userInfo.ID}/orders";

            var content = productsInfo
                .Select(t => new DbOrderProduct(t.Item1.Transport.ID, t.Item2, t.Item1.Days))
                .ToList();

            using HttpResponseMessage addOrderResponse = db.PutAsync(addOrderHandle, content).Result;

            if (!addOrderResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(addOrderResponse);

            string creationDateStamp = addOrderResponse.Content.ReadAsStringAsync().Result.Replace("\"", null);
            DateTime creationDate = DateTime.TryParse(creationDateStamp, out var date) ? date : DateTime.Now;

            string status = Order.RENTED_STATUS;
            string id = productsInfo[0].Item1.Transport.ID;
            double price = productsInfo.Sum(t => t.Item1.TotalPrice * t.Item2);
            var models = productsInfo.Select(t => t.Item1.Transport);

            userInfo.Money -= price;

            return new Order(id, price, status, creationDate, models, creationDateStamp);
        }
    }
}
