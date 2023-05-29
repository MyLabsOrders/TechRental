using System;
using System.Collections.ObjectModel;

namespace RentDesktop.Models.Informing
{
    public class UserInfo : IUserInfo
    {
        public const string ADMIN_POSITION = "admin";
        public const string USER_POSITION = "user";
        public const string ACTIVE_STATUS = "Активен";
        public const string INACTIVE_STATUS = "Неактивен";
        public const string MALE_GENDER = "Мужской";
        public const string FEMALE_GENDER = "Женский";
        public const string HIDDEN_PASSWORD = "*";

        public string ID { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public double Money { get; set; } = 0;
        public byte[] Icon { get; set; } = Array.Empty<byte>();
        public DateTime DateOfBirth { get; set; } = default;
        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();

        public string DateOfBirthPresenter => DateOfBirth.ToShortDateString();

        public void CopyTo(IUserInfo other)
        {
            other.ID = ID;
            other.Login = Login;
            other.Password = Password;
            other.Name = Name;
            other.Surname = Surname;
            other.Patronymic = Patronymic;
            other.PhoneNumber = PhoneNumber;
            other.Gender = Gender;
            other.Position = Position;
            other.Status = Status;
            other.Money = Money;
            other.Icon = Icon;
            other.DateOfBirth = DateOfBirth;
        }

        public bool IsAdmin()
        {
            return Position == ADMIN_POSITION;
        }

        public bool IsUser()
        {
            return Position == USER_POSITION;
        }
    }
}
