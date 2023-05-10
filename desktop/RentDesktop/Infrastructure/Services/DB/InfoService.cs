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
                new UserInfo() { Name = "Ivan", Surname = "Ivanov", Patronymic = "Ivanovich" },
                new UserInfo() { Name = "Vasya", Surname = "Vaskin", Patronymic = "Vasilevich" },
                new UserInfo() { Name = "Roman", Surname = "Romanov", Patronymic = "Romanovich" },
                new UserInfo() { Name = "Irina", Surname = "Irova", Patronymic = "Irekovna" },
            };
        }
    }
}
