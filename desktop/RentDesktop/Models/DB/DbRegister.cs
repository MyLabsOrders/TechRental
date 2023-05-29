namespace RentDesktop.Models.DB
{
#pragma warning disable IDE1006

    internal class DbRegister
    {
        public DbRegister()
        {
        }

        public DbRegister(string username, string password, string rolename)
        {
            this.username = username;
            this.password = password;
            this.rolename = rolename;
        }

        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string rolename { get; set; } = string.Empty;
    }

#pragma warning restore IDE1006
}
