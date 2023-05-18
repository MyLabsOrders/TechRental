namespace RentDesktop.Models.DB
{
#pragma warning disable IDE1006

    internal class DbChangePassword
    {
        public DbChangePassword()
        { 
        }

        public DbChangePassword(string currentPassword, string newPassword)
        {
            this.currentPassword = currentPassword;
            this.newPassword = newPassword;
        }

        public string currentPassword { get; set; } = string.Empty;
        public string newPassword { get; set; } = string.Empty;
    }

#pragma warning restore IDE1006
}
