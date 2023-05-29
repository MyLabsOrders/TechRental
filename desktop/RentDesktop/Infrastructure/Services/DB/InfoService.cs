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
            var allUsers = new List<IUserInfo>();
            using var db = new DatabaseConnectionService();

            int currentPage = 1;
            IEnumerable<DbUser> currentOrder;

            do
            {
                string allUsersHandle = $"/api/User?page={currentPage++}";
                using HttpResponseMessage allUsersResponse = db.GetAsync(allUsersHandle).Result;

                if (!allUsersResponse.IsSuccessStatusCode)
                    throw new ErrorResponseException(allUsersResponse);

                var usersCollection = allUsersResponse.Content.ReadFromJsonAsync<DbUsers>().Result;

                if (usersCollection is null || usersCollection.users is null)
                    throw new IncorrectContentException(allUsersResponse.Content);

                string[] positions = usersCollection.users
                    .Select(t => GetUserIdentityInfo(t.id, db).role)
                    .ToArray();

                List<IUserInfo> currentUsers = DatabaseModelConverterService.ConvertUsers(usersCollection, positions);

                foreach (var user in currentUsers)
                {
                    user.Login = GetUserIdentityInfo(user.ID, db).username;
                    user.Password = UserInfo.HIDDEN_PASSWORD;
                }

                allUsers.AddRange(currentUsers);
                currentOrder = usersCollection.users;
            }
            while (currentOrder.Any());

            return allUsers;
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
