using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class UserRegisterService
    {
        public static bool IsLoginFree(string login)
        {
            // Login verification is now inside the database
            return true;
        }

        public static void RegisterUser(IUserInfo userInfo)
        {
            //throw new NotImplementedException();

            using var db = new DatabaseConnectionService();

            const string handle = "/api/identity/register";
            var content = JsonContent.Create(new DbRegister(userInfo.Login, userInfo.Password, userInfo.Position));

            using HttpResponseMessage registerResponse = db.PostAsync(handle, content).Result;

            if (registerResponse.StatusCode != HttpStatusCode.OK)
                throw new ErrorResponseException(registerResponse.StatusCode);

            _ = LoginService.Login(userInfo.Login, userInfo.Password, db);
            
            SetUserInfo(userInfo, db);
        }

        private static void SetUserInfo(IUserInfo userInfo, DatabaseConnectionService db)
        {
            string profileHandle = $"/api/User/{userInfo.ID}/profile";

            var content = JsonContent.Create(new DbUserProfile()
            {
                firstName = userInfo.Name,
                middleName = userInfo.Surname,
                lastName = userInfo.Patronymic,
                phoneNumber = userInfo.PhoneNumber,
                userImage = BitmapService.BytesToString(userInfo.Icon),
                birthDate = DateTimeService.DateTimeToString(userInfo.DateOfBirth)
            });

            using HttpResponseMessage profileResponse = db.PostAsync(profileHandle, content).Result;

            if (profileResponse.StatusCode != HttpStatusCode.OK)
                throw new ErrorResponseException(profileResponse.StatusCode);
        }
    }
}
