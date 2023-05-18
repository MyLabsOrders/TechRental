using RentDesktop.Models.Informing;
using RentDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class UserCashService
    {
        public static bool CanPayOrder(IEnumerable<TransportRent> cart, IUserInfo userInfo)
        {
            //throw new NotImplementedException();

            double price = cart.Sum(t => t.TotalPrice);
            double userMoney = int.MaxValue;

            return userMoney >= price;
        }
    }
}
