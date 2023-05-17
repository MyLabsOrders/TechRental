using RentDesktop.Models.Informing;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class UserRegisterService
    {
        public static bool RegisterUser(IUserInfo userInfo)
        {
            //throw new NotImplementedException();
            //return true;

            using var db = new DatabaseConnectionService();

        }

        public static bool IsLoginFree(string login)
        {
            // Login verification is now inside the database
            return true;
        }
    }
}
