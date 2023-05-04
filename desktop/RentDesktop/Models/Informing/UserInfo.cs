using System;

namespace RentDesktop.Models.Informing
{
    public class UserInfo : IUserInfo
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public byte[] Icon { get; set; } = Array.Empty<byte>();
        public DateTime DateOfBirth { get; set; }
    }
}
