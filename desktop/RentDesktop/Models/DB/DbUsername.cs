namespace RentDesktop.Models.DB
{
#pragma warning disable IDE1006

    internal class DbUsername
    {
        public DbUsername()
        {
        }

        public DbUsername(string username)
        {
            this.username = username;
        }

        public string username { get; set; } = string.Empty;
    }

#pragma warning restore IDE1006
}
