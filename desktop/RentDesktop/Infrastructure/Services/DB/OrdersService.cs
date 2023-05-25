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

            order.Status = newStatus;
        }

        public static List<Order> CreateOrders(IEnumerable<TransportRent> cart, IUserInfo userInfo)
        {
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

            string addOrderHandle = $"/api/User/{userInfo.ID}/orders";
            var content = new DbOrderId(transportRent.Transport.ID);

            using HttpResponseMessage addOrderResponse = db.PutAsync(addOrderHandle, content).Result;

            if (!addOrderResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(addOrderResponse);

            string status = Order.RENTED_STATUS;
            double price = transportRent.TotalPrice;
            string id = transportRent.Transport.ID;
            DateTime creationDate = DateTime.Now;
            var models = new[] { transportRent.Transport };

            userInfo.Money -= price;

            return new Order(id, price, status, creationDate, models);
        }
    }
}
