using RentDesktop.Models;
using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class OrdersService
    {
        public static void MarkOrdersAsCompleted(IEnumerable<Order> orders, IUserInfo userInfo)
        {
            // Future work: add the ability to mark orders as completed
            // throw new NotImplementedException();
        }

        public static void ChangeOrderStatus(IOrder order, string newStatus)
        {
            using var db = new DatabaseConnectionService();

            const string changeOrderStatusHandle = "/api/Order/status";
            var content = new DbOrderStatus(order.ID, newStatus);

            using HttpResponseMessage changeOrderStatusResponse = db.PutAsync(changeOrderStatusHandle, content).Result;

            if (!changeOrderStatusResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(changeOrderStatusResponse);

            // TODO: move to ViewModel
            order.Status = newStatus;
        }

        public static List<Order> CreateOrders(IEnumerable<TransportRent> cart, IUserInfo userInfo)
        {
            //string id = new Random().Next(0, 1000000).ToString();
            //string status = Order.ACTIVE_STATUS;
            //var date = DateTime.Now;

            //var models = cart.Select(t => t.Transport.Name);
            //double price = cart.Sum(t => t.TotalPrice);

            //order = new Order(id, price, status, date, models);

            var orders = new List<Order>();

            foreach (var cartItem in cart)
            {
                Order order = CreateOrder(cartItem, userInfo);
                orders.Add(order);
            }

            return orders;
        }

        private static Order CreateOrder(TransportRent transportRent, IUserInfo userInfo)
        {
            using var db = new DatabaseConnectionService();

            string addOrderHandle = $"/api/User/{userInfo.ID}/order";
            var content = new DbOrderId(transportRent.Transport.ID);

            using HttpResponseMessage addOrderResponse = db.PutAsync(addOrderHandle, content).Result;

            if (!addOrderResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(addOrderResponse);

            string status = Order.AVAILABLE_STATUS;
            double price = transportRent.TotalPrice;
            string id = transportRent.Transport.ID;
            DateTime creationDate = transportRent.Transport.CreationDate;
            var models = new[] { transportRent.Transport };

            return new Order(id, price, status, creationDate, models);
        }
    }
}
