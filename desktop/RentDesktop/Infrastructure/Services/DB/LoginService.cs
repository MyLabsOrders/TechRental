using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System.Net.Http;
using System.Net.Http.Json;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class LoginService
    {
        public static IUserInfo Login(string login, string password)
        {
            //var userInfo = new UserInfo()
            //{
            //    ID = "test_id",
            //    Login = login,
            //    Password = password,
            //    Name = "Иван",
            //    Surname = "Иван",
            //    Patronymic = "Иванович",
            //    Gender = UserInfo.MALE_GENDER,
            //    PhoneNumber = "8 (921) 123-4567",
            //    Position = UserInfo.USER_POSITION,
            //    Status = UserInfo.ACTIVE_STATUS,
            //    DateOfBirth = new DateTime(2000, 1, 21),
            //    Icon = File.ReadAllBytes(@"D:\Testing\TechRental\human1.jpg"),
            //    Money = 100000,
            //    Orders = new ObservableCollection<Order>() { new Order("123", 5000, DateTime.Now, new string[] { "Lada 7, Lada 15" } ) }
            //};

            var db = new DatabaseConnectionService();

            DbLoginResponseContent loginContent = EnterSystem(db, login, password);
            db.SetAuthorizationToken(loginContent.token);

            return GetUserInfo(db, loginContent.userId, login, password);
        }

        public static DbLoginResponseContent EnterSystem(DatabaseConnectionService db, string login, string password)
        {
            const string loginHandle = "/api/identity/login";
            var content = new DbLogin(login, password);

            using HttpResponseMessage loginResponse = db.PostAsync(loginHandle, content).Result;

            if (loginResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(loginResponse);

            var loginContent = loginResponse.Content.ReadFromJsonAsync<DbLoginResponseContent>().Result;

            return loginContent ?? throw new IncorrectContentException(loginResponse.Content);
        }

        private static IUserInfo GetUserInfo(DatabaseConnectionService db, string userId, string login, string password)
        {
            string profileHandle = $"/api/User/{userId}";
            using HttpResponseMessage profileResponse = db.GetAsync(profileHandle).Result;

            if (profileResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(profileResponse);

            DbUser? profileContent = profileResponse.Content.ReadFromJsonAsync<DbUser>().Result
                ?? throw new IncorrectContentException(profileResponse.Content);

            string position = InfoService.GetUserPosition(login, db);

            UserInfo userInfo = DatabaseModelConverterService.ConvertUser(profileContent, position);
            userInfo.Login = login;
            userInfo.Password = password;

            return userInfo;
        }
    }
}
