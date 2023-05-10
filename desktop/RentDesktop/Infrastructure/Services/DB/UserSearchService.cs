using RentDesktop.Models.Informing;
using System.Collections.Generic;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class UserSearchService
    {
        public static List<IUserInfo> GetAllUsers()
        {
            //throw new NotImplementedException();

            return new List<IUserInfo>()
            {
                new UserInfo() { Name = "Ivan", Surname = "Ivanov", Patronymic = "Ivanovich" },
                new UserInfo() { Name = "Vasya", Surname = "Vaskin", Patronymic = "Vasilevich" },
                new UserInfo() { Name = "Roman", Surname = "Romanov", Patronymic = "Romanovich" },
                new UserInfo() { Name = "Irina", Surname = "Irova", Patronymic = "Irekovna" },
            };
        }
    }
}
