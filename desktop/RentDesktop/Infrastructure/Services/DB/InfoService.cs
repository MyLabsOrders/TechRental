using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public static List<string> GetAllGenders()
        {
            //throw new NotImplementedException();

            return new List<string>()
            {
                UserInfo.MALE_GENDER,
                UserInfo.FEMALE_GENDER
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
                throw new ErrorResponseException(allUsersResponse);

            DbUsers? allUsers = allUsersResponse.Content.ReadFromJsonAsync<DbUsers>().Result;

            if (allUsers is null || allUsers.users is null)
                throw new IncorrectContentException(allUsersResponse.Content);

            //IEnumerable<string> positions = allUsers.users.Select(t =>
            //{
            //    return GetUserPosition(t.)
            //});
            // TODO

            return allUsers is not null && allUsers.users is not null
                ? DatabaseModelConverterService.ConvertUsers(allUsers)
                : throw new IncorrectContentException(allUsersResponse.Content);
        }

        public static string GetUserPosition(string login, DatabaseConnectionService? db = null)
        {
            db ??= new DatabaseConnectionService();

            string adminCheckHandle = $"/api/identity/authorize-admin";
            var content = new DbUsername(login);

            using HttpResponseMessage adminCheckResponse = db.PostAsync(adminCheckHandle, content).Result;

            return adminCheckResponse.StatusCode == HttpStatusCode.OK
                ? UserInfo.ADMIN_POSITION
                : UserInfo.USER_POSITION;
        }
    }
}
