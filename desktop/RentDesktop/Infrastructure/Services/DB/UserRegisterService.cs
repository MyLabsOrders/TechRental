using Newtonsoft.Json;
using RentDesktop.Infrastructure.App;
using RentDesktop.Models.Communication;
using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System;
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

            const string handle = "/api/identity/user/register";
            var content = new DbRegister(userInfo.Login, userInfo.Password, userInfo.Position);

            using HttpResponseMessage registerResponse = db.PostAsync(handle, content).Result;

            if (registerResponse.StatusCode != HttpStatusCode.OK)
                throw new ErrorResponseException(registerResponse.StatusCode);

            DbLoginResponseContent loginContent = LoginService.EnterSystem(db, userInfo.Login, userInfo.Password);
            
            db.AddAuthorizationToken(loginContent.token);
            userInfo.ID = loginContent.userId;

            SetUserInfo(userInfo, db);
        }

        private static void SetUserInfo(IUserInfo userInfo, DatabaseConnectionService db)
        {
            string profileHandle = $"/api/User/{userInfo.ID}/profile";

            var content = new DbUserProfile()
            {
                firstName = userInfo.Name,
                middleName = userInfo.Surname,
                lastName = userInfo.Patronymic,
                phoneNumber = userInfo.PhoneNumber,
                userImage = BitmapService.BytesToString(userInfo.Icon),
                birthDate = DateTimeService.DateTimeToString(userInfo.DateOfBirth)
            };

            using HttpResponseMessage profileResponse = db.PostAsync(profileHandle, content).Result;

            //var content1 = profileResponse.Content.ReadAsStringAsync().Result;
            //var jobject = JsonConvert.DeserializeObject(content1);
            //var window = WindowFinder.FindMainWindow();
            //QuickMessage.Info(JsonConvert.SerializeObject(jobject, Formatting.Indented)).ShowDialog(window);

            if (profileResponse.StatusCode != HttpStatusCode.OK)
                throw new ErrorResponseException(profileResponse.StatusCode);
        }
    }
}
