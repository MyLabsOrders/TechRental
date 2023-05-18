using RentDesktop.Models;
using RentDesktop.Models.Informing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class OrdersService
    {
        public static bool SaveOrdersStatus(IEnumerable<Order> orders, IUserInfo userInfo)
        {
            //throw new NotImplementedException();
            return true;
        }

        public static bool CreateOrder(IEnumerable<TransportRent> cart, IUserInfo userInfo, out Order order)
        {
            //throw new NotImplementedException();

            string id = new Random().Next(0, 1000000).ToString();
            string status = Order.ACTIVE_STATUS;
            var date = DateTime.Now;

            var models = cart.Select(t => t.Transport.Name);
            double price = cart.Sum(t => t.TotalPrice);

            order = new Order(id, price, status, date, models);

            return true;
        }

        public static bool CanPayOrder(IEnumerable<TransportRent> cart, IUserInfo userInfo)
        {
            //throw new NotImplementedException();

            double price = cart.Sum(t => t.TotalPrice);
            double userMoney = int.MaxValue;

            return userMoney >= price;
        }
    }
}
