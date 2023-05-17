using RentDesktop.Models;
using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class LoginService
    {
        public static async Task<(bool success, IUserInfo? userInfo)> Login(string login, string password)
        {
            //throw new NotImplementedException();

            var userInfo = new UserInfo()
            {
                ID = "test_id",
                Login = login,
                Password = password,
                Name = "Иван",
                Surname = "Иван",
                Patronymic = "Иванович",
                Gender = UserInfo.MALE_GENDER,
                PhoneNumber = "8 (921) 123-4567",
                Position = UserInfo.USER_POSITION,
                Status = UserInfo.ACTIVE_STATUS,
                DateOfBirth = new DateTime(2000, 1, 21),
                Icon = File.ReadAllBytes(@"D:\Testing\TechRental\human1.jpg"),
                Orders = new ObservableCollection<Order>() { new Order("123", 5000, DateTime.Now, new string[] { "Lada 7, Lada 15" } ) }
            };

            using var db = new DatabaseConnectionService();

            const string loginHandle = "/api/identity/login";
            var content = JsonContent.Create(new DbLogin(login, password));

            using HttpResponseMessage response = await db.PostAsync(loginHandle, content);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new ErrorResponseException(response.StatusCode);

            const string profileHandle = "/api/User/profile";

            return true;
        }
    }
}
