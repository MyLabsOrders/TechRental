using RentDesktop.Models.Informing;
using System;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class LoginService
    {
        public static bool Login(string login, string password, out IUserInfo? userInfo)
        {
            //throw new NotImplementedException();

            userInfo = new UserInfo()
            {
                Login = login,
                Password = password,
                Name = "Иван",
                Surname = "Иван",
                Patronymic = "Иванович",
                Gender = "Мужской",
                PhoneNumber = "8 (921) 123-4567",
                Status = "User",
                DateOfBirth = new DateTime(2000, 1, 21),
                Icon = Array.Empty<byte>()
            };

            return true;
        }
    }
}
