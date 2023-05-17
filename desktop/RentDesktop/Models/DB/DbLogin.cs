namespace RentDesktop.Models.DB
{
#pragma warning disable IDE1006

    internal class DbLogin
    {
        public DbLogin()
        {
        }

        public DbLogin(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }

#pragma warning restore IDE1006
}
