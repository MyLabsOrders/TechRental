using System;

namespace RentDesktop.Models.Informing
{
    public interface IUserInfo
    {
        string Login { get; set; }
        string Password { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
        string Patronymic { get; set; }
        string PhoneNumber { get; set; }
        string Gender { get; set; }
        string Position { get; set; }
        string Status { get; set; }
        byte[] Icon { get; set; }
        DateTime DateOfBirth { get; set; }

        void CopyTo(IUserInfo other);
        bool IsAdmin();
        bool IsUser();
    }
}
