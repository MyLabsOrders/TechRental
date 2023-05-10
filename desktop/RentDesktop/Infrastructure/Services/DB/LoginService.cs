using RentDesktop.Models.Informing;
using System;
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
                ID = 999, //TODO
                Login = login,
                Password = password,
                Name = "Иван",
                Surname = "Иван",
                Patronymic = "Иванович",
                Gender = "Мужской",
                PhoneNumber = "8 (921) 123-4567",
                Position = UserInfo.ADMIN_POSITION,
                Status = UserInfo.ACTIVE_STATUS,
                DateOfBirth = new DateTime(2000, 1, 21),
                Icon = File.ReadAllBytes(@"D:\Testing\TechRental\human1.jpg")
            };

            return true;
        }
    }
}
