using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class LoginService
    {
        public static IUserInfo Login(string login, string password, DatabaseConnectionService? db = null)
        {
            //throw new NotImplementedException();

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
            //    Orders = new ObservableCollection<Order>() { new Order("123", 5000, DateTime.Now, new string[] { "Lada 7, Lada 15" } ) }
            //};

            db ??= new DatabaseConnectionService();

            string userId = EnterSystem(db, login, password);
            return GetUserInfo(db, userId, login, password);
        }

        private static string EnterSystem(DatabaseConnectionService db, string login, string password)
        {
            const string loginHandle = "/api/identity/login";
            var content = JsonContent.Create(new DbLogin(login, password));

            using HttpResponseMessage loginResponse = db.PostAsync(loginHandle, content).Result;

            if (loginResponse.StatusCode != HttpStatusCode.OK)
                throw new ErrorResponseException(loginResponse.StatusCode);

            var loginContent = loginResponse.Content.ReadFromJsonAsync<DbLoginResponseContent>().Result;

            return loginContent?.userId
                ?? throw new IncorrectResponseContentException(nameof(loginContent));
        }

        private static IUserInfo GetUserInfo(DatabaseConnectionService db, string id, string login, string password)
        {
            string profileHandle = $"/api/User/{id}";
            using HttpResponseMessage profileResponse = db.GetAsync(profileHandle).Result;

            if (profileResponse.StatusCode != HttpStatusCode.OK)
                throw new ErrorResponseException(profileResponse.StatusCode);

            DbUser? profileContent = profileResponse.Content.ReadFromJsonAsync<DbUser>().Result;

            if (profileContent is null)
                throw new IncorrectResponseContentException(nameof(profileContent));

            UserInfo userInfo = DatabaseModelConverterService.ConvertUser(profileContent);
            userInfo.Login = login;
            userInfo.Password = password;

            return userInfo;
        }
    }
}
