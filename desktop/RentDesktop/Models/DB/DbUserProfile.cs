namespace RentDesktop.Models.DB
{
#pragma warning disable IDE1006

    internal class DbUserProfile
    {
        public string firstName { get; set; } = string.Empty;
        public string middleName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string phoneNumber { get; set; } = string.Empty;
        //public string userImage { get; set; } = string.Empty; // TODO
        public string birthDate { get; set; } = string.Empty;
    }

#pragma warning restore IDE1006
}
