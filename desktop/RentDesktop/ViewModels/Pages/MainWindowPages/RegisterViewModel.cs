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

namespace RentDesktop.ViewModels.Pages.MainWindowPages
{
    public class RegisterViewModel : ViewModelBase
    {
        public RegisterViewModel() : this(UserInfo.USER_POSITION)
        {
        }

        public RegisterViewModel(string position)
        {
            _position = position;

            RegisterUserCommand = ReactiveCommand.Create(RegisterUser);
            ClosePageCommand = ReactiveCommand.Create(ClosePage);
            LoadUserImageCommand = ReactiveCommand.Create(LoadUserImage);
            SetGenderCommand = ReactiveCommand.Create<string>(SetGender);
        }

        #region Events

        public delegate void PageClosingHandler();
        public event PageClosingHandler? PageClosing;

        #endregion

        #region Properties

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

        private string _passwordConfirmation = string.Empty;
        public string PasswordConfirmation
        {
            get => _passwordConfirmation;
            set => this.RaiseAndSetIfChanged(ref _passwordConfirmation, value);
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

        private char? _passwordChar = HIDDEN_PASSWORD_CHAR;
        public char? PasswordChar
        {
            get => _passwordChar;
            private set => this.RaiseAndSetIfChanged(ref _passwordChar, value);
        }

        private bool _showPassword = false;
        public bool ShowPassword
        {
            get => _showPassword;
            set
            {
                this.RaiseAndSetIfChanged(ref _showPassword, value);
                PasswordChar = value ? null : HIDDEN_PASSWORD_CHAR;
            }
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

        #region Private Fields

        #region Constants

        private const int PHONE_NUMBER_DIGITS = 11;
        private const char HIDDEN_PASSWORD_CHAR = '*';

        #endregion

        private readonly string _position;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> RegisterUserCommand { get; }
        public ReactiveCommand<Unit, Unit> ClosePageCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadUserImageCommand { get; }
        public ReactiveCommand<string, Unit> SetGenderCommand { get; }

        #endregion

        #region Protected Methods

        protected virtual Type GetOwnerWindowType()
        {
            return typeof(MainWindow);
        }

        protected virtual IUserInfo GetUserInfo()
        {
            byte[] userImageBytes = UserImage is not null
                ? BitmapService.BitmapToBytes(UserImage)
                : Array.Empty<byte>();

            return new UserInfo()
            {
                Login = Login,
                Password = Password,
                Name = Name,
                Surname = Surname,
                Patronymic = Patronymic,
                PhoneNumber = PhoneNumber,
                Gender = Gender,
                Position = _position,
                Status = UserInfo.ACTIVE_STATUS,
                Icon = userImageBytes,
                DateOfBirth = DateOfBirth!.Value
            };
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
            if (Password != PasswordConfirmation)
            {
                QuickMessage.Info("Пароли не совпадают.").ShowDialog(window);
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

        protected virtual void ResetAllFields()
        {
            Login = string.Empty;
            Password = string.Empty;
            Name = string.Empty;
            Surname = string.Empty;
            Patronymic = string.Empty;
            PhoneNumber = string.Empty;
            Gender = string.Empty;

            UserImage?.Dispose();
            UserImage = null;
            DateOfBirth = null;

            ShowPassword = false;
            IsMaleGenderChecked = false;
            IsFemaleGenderChecked = false;
        }

        #endregion

        #region Private Methods

        private void RegisterUser()
        {
            if (!VerifyFieldsCorrectness())
                return;

            IUserInfo userInfo = GetUserInfo();

            if (UserRegisterService.RegisterUser(userInfo))
            {
                ResetAllFields();
                PageClosing?.Invoke();
            }
            else
            {
                var window = WindowFinder.FindByType(GetOwnerWindowType());
                QuickMessage.Error("Не удалось зарегистрировать пользователя.").ShowDialog(window);
            }
        }

        private async void LoadUserImage()
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

        private void ClosePage()
        {
            PageClosing?.Invoke();
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
