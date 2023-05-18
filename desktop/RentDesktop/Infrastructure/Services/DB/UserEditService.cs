using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System.Net.Http;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class UserEditService
    {
        public static void EditInfo(IUserInfo initialUserInfo, IUserInfo newUserInfo)
        {
            if (initialUserInfo.Login != newUserInfo.Login)
                ChangeLogin(newUserInfo.Login);

            if (initialUserInfo.Password != newUserInfo.Password)
                ChangePassword(initialUserInfo.Password, newUserInfo.Password);

            if (initialUserInfo.Position != newUserInfo.Position)
                ChangePosition(newUserInfo.Login, newUserInfo.Position);

            // Future work: add the ability to change remaining fields
        }

        public static void ChangePassword(string currentPassword, string newPassword)
        {
            using var db = new DatabaseConnectionService();

            const string changePasswordHandle = "/api/identity/password";
            var content = new DbChangePassword(currentPassword, newPassword);

            using HttpResponseMessage changePasswordResponse = db.PutAsync(changePasswordHandle, content).Result;

            if (changePasswordResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(changePasswordResponse);
        }

        public static void ChangeLogin(string newLogin)
        {
            using var db = new DatabaseConnectionService();

            const string changeLoginHandle = "/api/identity/login";
            var content = new DbChangeLogin(newLogin);

            using HttpResponseMessage changeLoginResponse = db.PutAsync(changeLoginHandle, content).Result;

            if (changeLoginResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(changeLoginResponse);
        }

        public static void ChangePosition(string userLogin, string newPosition)
        {
            using var db = new DatabaseConnectionService();

            string changeRoleHandle = $"/api/identity/{userLogin}/role?roleName={newPosition}";
            var content = new { };

            using HttpResponseMessage changeRoleResponse = db.PutAsync(changeRoleHandle, content).Result;

            if (changeRoleResponse.IsSuccessStatusCode)
                throw new ErrorResponseException(changeRoleResponse);
        }
    }
}
