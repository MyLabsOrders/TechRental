using RentDesktop.Models.Informing;
using System.Collections.Generic;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class InfoService
    {
        public static List<string> GetAllStatuses()
        {
            //throw new NotImplementedException();

            return new List<string>()
            {
                UserInfo.ACTIVE_STATUS,
                UserInfo.INACTIVE_STATUS
            };
        }

        public static List<string> GetAllPositions()
        {
            //throw new NotImplementedException();

            return new List<string>()
            {
                UserInfo.USER_POSITION,
                UserInfo.ADMIN_POSITION
            };
        }

        public static List<IUserInfo> GetAllUsers()
        {
            //throw new NotImplementedException();

            return new List<IUserInfo>()
            {
                new UserInfo() { Name = "Ivan", Surname = "Ivanov", Patronymic = "Ivanovich", Gender = "Мужской", ID = 987, Position = "Admin" },
                new UserInfo() { Name = "Vasya", Surname = "Vaskin", Patronymic = "Vasilevich", Gender = "Мужской", ID = 1234, Position = "User" },
                new UserInfo() { Name = "Roman", Surname = "Romanov", Patronymic = "Romanovich" , Gender = "Мужской", ID = 764, Position = "User"},
                new UserInfo() { Name = "Irina", Surname = "Irova", Patronymic = "Irekovna", Gender = "Женский", ID = 1340, Position = "Admin" },
            };
        }
    }
}
