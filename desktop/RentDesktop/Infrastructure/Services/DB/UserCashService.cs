using RentDesktop.Models;
using RentDesktop.Models.Informing;
using System.Collections.Generic;
using System.Linq;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class UserCashService
    {
        public static bool CanPayOrder(IEnumerable<TransportRent> cart, IUserInfo userInfo)
        {
            double price = cart.Sum(t => t.TotalPrice);
            return userInfo.Money >= price;
        }
    }
}
