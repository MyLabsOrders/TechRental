using System;
using System.Collections.ObjectModel;

namespace RentDesktop.Models.Informing
{
    public interface IUserInfo
    {
        string ID { get; set; }
        string Login { get; set; }
        string Password { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
        string Patronymic { get; set; }
        string PhoneNumber { get; set; }
        string Gender { get; set; }
        string Position { get; set; }
        string Status { get; set; }
        double Money { get; set; }
        byte[] Icon { get; set; }
        DateTime DateOfBirth { get; set; }
        ObservableCollection<Order> Orders { get; set; }

        public string DateOfBirthPresenter { get; }

        void CopyTo(IUserInfo other);
        bool IsAdmin();
        bool IsUser();
    }
}
