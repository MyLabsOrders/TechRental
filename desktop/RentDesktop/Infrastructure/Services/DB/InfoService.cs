using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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

        public async static Task<List<IUserInfo>> GetAllUsersAsync()
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

            const string handle = "/api/User";
            using HttpResponseMessage response = await db.GetAsync(handle);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new ErrorResponseException(response.StatusCode);

            DbUsers? readedUsers = await response.Content.ReadFromJsonAsync<DbUsers>();

            if (readedUsers is null || readedUsers.users is null)
                throw new IncorrectDataInResponseException(nameof(readedUsers));

            var converter = new Func<List<IUserInfo>>(() =>
            {
                return DatabaseModelConverterService.ConvertUsers(readedUsers);
            });

            return await new Task<List<IUserInfo>>(converter);
        }
    }
}
