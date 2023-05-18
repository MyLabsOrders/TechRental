namespace RentDesktop.Models.DB
{
#pragma warning disable IDE1006

    internal class DbChangeLogin
    {
        public DbChangeLogin()
        {
        }

        public DbChangeLogin(string username)
        {
            this.username = username;
        }

        public string username { get; set; } = string.Empty;
    }

#pragma warning restore IDE1006
}
