using RentDesktop.Models;
using RentDesktop.Models.Informing;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class LoginService
    {
        public static bool Login(string login, string password, out IUserInfo? userInfo)
        {
            //throw new NotImplementedException();

            userInfo = new UserInfo()
            {
                ID = "test_id",
                Login = login,
                Password = password,
                Name = "Иван",
                Surname = "Иван",
                Patronymic = "Иванович",
                Gender = "Мужской",
                PhoneNumber = "8 (921) 123-4567",
                Position = UserInfo.USER_POSITION,
                Status = UserInfo.ACTIVE_STATUS,
                DateOfBirth = new DateTime(2000, 1, 21),
                Icon = File.ReadAllBytes(@"D:\Testing\TechRental\human1.jpg"),
                Orders = new ObservableCollection<Order>() { new Order("123", 5000, DateTime.Now, new string[] { "Lada 7, Lada 15" } ) }
            };

            return true;
        }
    }
}
