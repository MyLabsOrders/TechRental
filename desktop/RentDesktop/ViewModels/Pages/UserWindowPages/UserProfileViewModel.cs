using Avalonia.Controls;
using Avalonia.Media.Imaging;
using ReactiveUI;
using RentDesktop.Infrastructure.App;
using RentDesktop.Infrastructure.Services;
using RentDesktop.Infrastructure.Services.DB;
using RentDesktop.Models.Communication;
using RentDesktop.Models.Informing;
using RentDesktop.ViewModels.Base;
using RentDesktop.Views;
using System;
using System.Linq;
using System.Reactive;

namespace RentDesktop.ViewModels.Pages.UserWindowPages
{
    public class UserProfileViewModel : ViewModelBase
    {
        public UserProfileViewModel() : this(new UserInfo())
        {
        }

        public UserProfileViewModel(IUserInfo userInfo)
        {
            _userInfo = userInfo;
            SetUserInfo(userInfo);

            SwapEditModeCommand = ReactiveCommand.Create(SwapEditMode);
            ChangeUserImageCommand = ReactiveCommand.Create(ChangeUserImage);
            SaveUserInfoCommand = ReactiveCommand.Create(SaveUserInfo);
            SetGenderCommand = ReactiveCommand.Create<string>(SetGender);
        }

        #region Events

        public delegate void UserInfoUpdatedHandler();
        public event UserInfoUpdatedHandler? UserInfoUpdated;

        #endregion

        #region Properties

        private bool _isEditModeEnabled = false;
        public bool IsEditModeEnabled
        {
            get => _isEditModeEnabled;
            private set => this.RaiseAndSetIfChanged(ref _isEditModeEnabled, value);
        }

        private string _login = string.Empty;
        public string Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _surname = string.Empty;
        public string Surname
        {
            get => _surname;
            set => this.RaiseAndSetIfChanged(ref _surname, value);
        }

        private string _patronymic = string.Empty;
        public string Patronymic
        {
            get => _patronymic;
            set => this.RaiseAndSetIfChanged(ref _patronymic, value);
        }

        private string _phoneNumber = string.Empty;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => this.RaiseAndSetIfChanged(ref _phoneNumber, value);
        }

        private string _position = string.Empty;
        public string Position
        {
            get => _position;
            private set => this.RaiseAndSetIfChanged(ref _position, value);
        }

        private DateTime? _dateOfBirth = null;
        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set => this.RaiseAndSetIfChanged(ref _dateOfBirth, value);
        }

        private string _gender = string.Empty;
        public string Gender
        {
            get => _gender;
            set => this.RaiseAndSetIfChanged(ref _gender, value);
        }

        private Bitmap? _userImage = null;
        public Bitmap? UserImage
        {
            get => _userImage;
            private set => this.RaiseAndSetIfChanged(ref _userImage, value);
        }

        private bool _isMaleGenderChecked = false;
        public bool IsMaleGenderChecked
        {
            get => _isMaleGenderChecked;
            set => this.RaiseAndSetIfChanged(ref _isMaleGenderChecked, value);
        }

        private bool _isFemaleGenderChecked = false;
        public bool IsFemaleGenderChecked
        {
            get => _isFemaleGenderChecked;
            set => this.RaiseAndSetIfChanged(ref _isFemaleGenderChecked, value);
        }

        #endregion

        #region Protected Fields

        #region Constants

        protected const int PHONE_NUMBER_DIGITS = 11;

        #endregion

        protected IUserInfo _userInfo;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> SwapEditModeCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangeUserImageCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveUserInfoCommand { get; }
        public ReactiveCommand<string, Unit> SetGenderCommand { get; }

        #endregion

        #region Protected Methods

        protected virtual Type GetOwnerWindowType()
        {
            return typeof(UserWindow);
        }

        protected virtual IUserInfo GetUserInfo()
        {
            byte[] userImageBytes = UserImage is not null
               ? BitmapService.ConvertBitmapToBytes(UserImage)
               : Array.Empty<byte>();

            return new UserInfo()
            {
                ID = 0, //TODO
                Login = Login,
                Password = Password,
                Name = Name,
                Surname = Surname,
                Patronymic = Patronymic,
                PhoneNumber = PhoneNumber,
                Gender = Gender,
                Position = Position,
                Status = "TODO",
                Icon = userImageBytes,
                DateOfBirth = DateOfBirth!.Value
            };
        }

        protected virtual void SetUserInfo(IUserInfo userInfo)
        {
            Login = userInfo.Login;
            Password = userInfo.Password;
            Name = userInfo.Name;
            Surname = userInfo.Surname;
            Patronymic = userInfo.Patronymic;
            PhoneNumber = userInfo.PhoneNumber;
            Gender = userInfo.Gender;
            Position = userInfo.Position;
            DateOfBirth = userInfo.DateOfBirth;

            UserImage?.Dispose();

            UserImage = userInfo.Icon.Length > 0
                ? BitmapService.ConvertBytesToBitmap(userInfo.Icon)
                : null;

            if (Gender == "Мужской")
                IsMaleGenderChecked = true;

            else if (Gender == "Женский")
                IsFemaleGenderChecked = true;
        }

        protected virtual bool VerifyFieldsCorrectness()
        {
            var window = WindowFinder.FindByType(GetOwnerWindowType());

            if (string.IsNullOrEmpty(Login))
            {
                QuickMessage.Info("Введите логин.").ShowDialog(window);
                return false;
            }
            if (!UserRegisterService.IsLoginFree(Login))
            {
                QuickMessage.Info("Логин уже занят.").ShowDialog(window);
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                QuickMessage.Info("Введите пароль.").ShowDialog(window);
                return false;
            }
            if (string.IsNullOrEmpty(Name))
            {
                QuickMessage.Info("Введите имя.").ShowDialog(window);
                return false;
            }
            if (string.IsNullOrEmpty(Surname))
            {
                QuickMessage.Info("Введите фамилию.").ShowDialog(window);
                return false;
            }
            if (string.IsNullOrEmpty(Patronymic))
            {
                QuickMessage.Info("Введите отчество.").ShowDialog(window);
                return false;
            }
            if (PhoneNumber.Where(t => char.IsDigit(t)).Count() != PHONE_NUMBER_DIGITS)
            {
                QuickMessage.Info("Введите корректный номер телефона.").ShowDialog(window);
                return false;
            }
            if (string.IsNullOrEmpty(Gender))
            {
                QuickMessage.Info("Выберите пол.").ShowDialog(window);
                return false;
            }
            if (DateOfBirth is null)
            {
                QuickMessage.Info("Введите дату рождения.").ShowDialog(window);
                return false;
            }

            return true;
        }

        #endregion

        #region Private Methods 

        private void SwapEditMode()
        {
            IsEditModeEnabled = !IsEditModeEnabled;
        }

        private async void ChangeUserImage()
        {
            if (WindowFinder.FindByType(GetOwnerWindowType()) is not Window window)
                return;

            OpenFileDialog dialog = DialogProvider.GetOpenImageDialog();
            string[]? paths = await dialog.ShowAsync(window);

            if (paths is null || paths.Length == 0)
                return;

            if (!TrySetUserImage(paths[0]))
                QuickMessage.Error("Не удалось открыть фото.").ShowDialog(window);
        }

        private void SaveUserInfo()
        {
            if (!VerifyFieldsCorrectness())
                return;

            IUserInfo newUserInfo = GetUserInfo();

            if (!UserEditService.EditInfo(newUserInfo))
            {
                var window = WindowFinder.FindByType(GetOwnerWindowType());
                QuickMessage.Error("Не удалось сохранить изменения.").ShowDialog(window);
                return;
            }

            newUserInfo.CopyTo(_userInfo);
            UserInfoUpdated?.Invoke();

            IsEditModeEnabled = false;
        }

        private void SetGender(string gender)
        {
            Gender = gender;
        }

        private bool TrySetUserImage(string path)
        {
            UserImage?.Dispose();
            UserImage = null;

            try
            {
                var image = new Bitmap(path);
                UserImage = image;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
