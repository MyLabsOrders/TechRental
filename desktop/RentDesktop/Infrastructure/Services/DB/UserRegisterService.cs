using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System.Net.Http;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class UserRegisterService
    {
        public static void RegisterUser(IUserInfo userInfo)
        {
            using var db = new DatabaseConnectionService();

            const string registerHandle = "/api/identity/user/register";
            var content = new DbRegister(userInfo.Login, userInfo.Password, userInfo.Position);

            using HttpResponseMessage registerResponse = db.PostAsync(registerHandle, content).Result;

            if (!registerResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(registerResponse);

            DbLoginResponseContent loginContent = LoginService.EnterSystem(db, userInfo.Login, userInfo.Password);

            db.SetAuthorizationToken(loginContent.token);
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
                // userImage = BitmapService.BytesToString(userInfo.Icon), // TOODO
                birthDate = DateTimeService.DateTimeToString(userInfo.DateOfBirth)
                // status = Future work: add status to user model
                // gender = Future work: add gender to user model
            };

            using HttpResponseMessage profileResponse = db.PostAsync(profileHandle, content).Result;

            //var content1 = profileResponse.Content.ReadAsStringAsync().Result;
            //var jobject = JsonConvert.DeserializeObject(content1);
            //var window = WindowFinder.FindMainWindow();
            //QuickMessage.Info(JsonConvert.SerializeObject(jobject, Formatting.Indented)).ShowDialog(window);

            if (!profileResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(profileResponse);
        }
    }
}
