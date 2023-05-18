using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

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

            //return new List<IUserInfo>()
            //{
            //    new UserInfo() { Name = "Ivan", Surname = "Ivanov", Patronymic = "Ivanovich", Gender = "Мужской", ID = "abc-123d-dds", Position = "Admin" },
            //    new UserInfo() { Name = "Vasya", Surname = "Vaskin", Patronymic = "Vasilevich", Gender = "Мужской", ID = "nhd-976d-dfs", Position = "User" },
            //    new UserInfo() { Name = "Roman", Surname = "Romanov", Patronymic = "Romanovich" , Gender = "Мужской", ID = "ihg-343d-dfs", Position = "User"},
            //    new UserInfo() { Name = "Irina", Surname = "Irova", Patronymic = "Irekovna", Gender = "Женский", ID = "xge-343d-dfs", Position = "Admin" },
            //};

            using var db = new DatabaseConnectionService();

            const string allUsersHandle = "/api/User";
            using HttpResponseMessage allUsersResponse = db.GetAsync(allUsersHandle).Result;

            if (allUsersResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(allUsersResponse.StatusCode);

            DbUsers? allUsers = allUsersResponse.Content.ReadFromJsonAsync<DbUsers>().Result;

            return allUsers is not null && allUsers.users is not null
                ? DatabaseModelConverterService.ConvertUsers(allUsers)
                : throw new IncorrectResponseContentException(nameof(allUsers));
        }
    }
}
