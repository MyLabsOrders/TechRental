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
        public static bool MarkOrdersAsCompleted(IEnumerable<Order> orders, IUserInfo userInfo)
        {
            //throw new NotImplementedException();
            return true;
        }

        public static void ChangeOrderStatus(IOrder order, string newStatus)
        {
            // throw new NotImplementedException()

            using var db = new DatabaseConnectionService();

            const string changeOrderStatusHandle = "/api/Order/status";
            var content = new DbOrderStatus(order.ID, newStatus);

            using HttpResponseMessage changeOrderStatusResponse = db.PutAsync(changeOrderStatusHandle, content).Result;

            if (changeOrderStatusResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(changeOrderStatusResponse.StatusCode);

            order.Status = newStatus; // move to ViewModel -> TODO
        }

        public static bool CreateOrder(IEnumerable<TransportRent> cart, IUserInfo userInfo, out Order order)
        {
            //throw new NotImplementedException();

            //string id = new Random().Next(0, 1000000).ToString();
            //string status = Order.ACTIVE_STATUS;
            //var date = DateTime.Now;

            //var models = cart.Select(t => t.Transport.Name);
            //double price = cart.Sum(t => t.TotalPrice);

            //order = new Order(id, price, status, date, models);

            var models = cart.Select(t => t.Transport.Name);
            string modelsPresenter = string.Join(Order.ORDERS_MODELS_DELIMITER, models);

            string status = Order.ACTIVE_STATUS;
            double price = cart.Sum(t => t.TotalPrice);
            string image = string.Empty;

            using var db = new DatabaseConnectionService();

            var content = new DbOrderCreationInfo()
            {
                name = modelsPresenter,
                image = image,
                status = status,
                total = price
            };

            const string changeOrderStatusHandle = "/api/Order/status";

            order = null;
            return true;
        }

        private static void GetOrderInfo()
        {

        }
    }
}
