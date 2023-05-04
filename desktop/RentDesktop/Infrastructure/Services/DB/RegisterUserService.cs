using RentDesktop.Models.Informing;

namespace RentDesktop.Infrastructure.Services.DB
{
    public static class RegisterUserService
    {
        public static bool RegisterUser(IUserInfo userInfo)
        {
            return true;
        }

        public static bool IsLoginFree(string login)
        {
            return true;
        }
    }
}
