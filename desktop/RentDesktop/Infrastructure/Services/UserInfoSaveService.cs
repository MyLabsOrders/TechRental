using RentDesktop.Infrastructure.Security;
using System;
using System.IO;

namespace RentDesktop.Infrastructure.Services
{
    public static class UserInfoSaveService
    {
        private const string PATH = "saved_user.txt";

        public static void ClearInfo()
        {
            File.Create(PATH).Close();
        }

        public static void SaveInfo(string login, string password)
        {
            string encryptedPassword = RSA.Encrypt(password);
            File.WriteAllText(PATH, $"{login}{Environment.NewLine}{encryptedPassword}");
        }

        public static (string Login, string Password) LoadInfo()
        {
            string[] data = File.ReadAllLines(PATH);

            if (data.Length == 0)
                return (string.Empty, string.Empty);

            string login = data[0];
            string password = RSA.Decrypt(data[1]);

            return (login, password);
        }

        public static bool TryClearInfo()
        {
            try
            {
                ClearInfo();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TrySaveInfo(string login, string password)
        {
            try
            {
                SaveInfo(login, password);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryLoadInfo(out (string Login, string Password) info)
        {
            try
            {
                info = LoadInfo();
                return !string.IsNullOrEmpty(info.Login);
            }
            catch
            {
                info = (string.Empty, string.Empty);
                return false;
            }
        }
    }
}
