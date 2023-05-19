using RentDesktop.Models;
using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class UserCashService
    {
        public static bool CanPayOrder(IEnumerable<TransportRent> cart, IUserInfo userInfo)
        {
            double price = cart.Sum(t => t.TotalPrice);
            return userInfo.Money >= price;
        }

        public static void AddCash(IUserInfo userInfo, double sum, bool logIn = false)
        {
            using var db = new DatabaseConnectionService();

            if (logIn)
            {
                DbLoginResponseContent loginContent = LoginService.EnterSystem(userInfo.Login, userInfo.Password, db);
                db.SetAuthorizationToken(loginContent.token);
            }

            string addCashHandle = $"/api/User/{userInfo.ID}/account";
            var content = new DbCash(sum);

            using HttpResponseMessage addCashResponse = db.PutAsync(addCashHandle, content).Result;

            if (!addCashResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(addCashResponse);
        }

        public static double GetUserBalace(IUserInfo userInfo)
        {
            using var db = new DatabaseConnectionService();

            string getUserHandle = $"/api/User/{userInfo.ID}";
            using HttpResponseMessage getUserResponse = db.GetAsync(getUserHandle).Result;

            var dbUser = getUserResponse.Content.ReadFromJsonAsync<DbUser>().Result
                ?? throw new IncorrectContentException(getUserResponse.Content);

            return dbUser.money;
        }
    }
}
