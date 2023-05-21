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
            return new List<string>()
            {
                UserInfo.ACTIVE_STATUS,
                UserInfo.INACTIVE_STATUS
            };
        }

        public static List<string> GetAllPositions()
        {
            return new List<string>()
            {
                UserInfo.USER_POSITION,
                UserInfo.ADMIN_POSITION
            };
        }

        public static List<string> GetAllGenders()
        {
            return new List<string>()
            {
                UserInfo.MALE_GENDER,
                UserInfo.FEMALE_GENDER
            };
        }

        public static List<IUserInfo> GetAllUsers()
        {
            using var db = new DatabaseConnectionService();

            const string allUsersHandle = "/api/User";
            using HttpResponseMessage allUsersResponse = db.GetAsync(allUsersHandle).Result;

            if (!allUsersResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(allUsersResponse);

            DbUsers? allUsers = allUsersResponse.Content.ReadFromJsonAsync<DbUsers>().Result;

            if (allUsers is null || allUsers.users is null)
                throw new IncorrectContentException(allUsersResponse.Content);

            string[] positions = allUsers.users
                .Select(t => GetUserIdentityInfo(t.id, db).role)
                .ToArray();

            List<IUserInfo> users = DatabaseModelConverterService.ConvertUsers(allUsers, positions);

            foreach (var user in users)
            {
                user.Login = GetUserIdentityInfo(user.ID, db).username;
                user.Password = UserInfo.HIDDEN_PASSWORD;
            }

            return users;
        }

        public static bool CheckUserIsAdmin(string login, DatabaseConnectionService? db = null)
        {
            db ??= new DatabaseConnectionService();

            string adminCheckHandle = "/api/identity/authorize-admin";
            var content = new DbUsername(login);

            using HttpResponseMessage adminCheckResponse = db.PostAsync(adminCheckHandle, content).Result;

            return adminCheckResponse.StatusCode == HttpStatusCode.OK;
        }

        public static DbIdentityInfo GetUserIdentityInfo(string userId, DatabaseConnectionService? db = null)
        {
            db ??= new DatabaseConnectionService();

            string getIdentityInfoHandle = $"/api/identity/{userId}";
            using HttpResponseMessage getIdentityInfoResponse = db.GetAsync(getIdentityInfoHandle).Result;

            return getIdentityInfoResponse.Content.ReadFromJsonAsync<DbIdentityInfo>().Result
                ?? throw new IncorrectContentException(getIdentityInfoResponse.Content);
        }
    }
}
