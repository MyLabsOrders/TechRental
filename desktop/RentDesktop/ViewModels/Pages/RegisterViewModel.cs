using Avalonia.Controls;
using Avalonia.Media.Imaging;
using MessageBox.Avalonia;
using ReactiveUI;
using RentDesktop.Infrastructure.App;
using RentDesktop.ViewModels.Base;
using System;
using System.Reactive;

namespace RentDesktop.ViewModels.Pages
{
    public class RegisterViewModel : ViewModelBase
    {
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

        private string _userImagePath = string.Empty;
        public string UserImagePath
        {
            get => _userImagePath;
            set
            {
                if (value == _userImagePath)
                    return;

                this.RaiseAndSetIfChanged(ref _userImagePath, value);

                UserImage?.Dispose();

                try
                {
                    var image = new Bitmap(value);
                    UserImage = image;
                }
                catch
                {
                    UserImage = null;

                    if (WindowFinder.FindMainWindow() is not Window window)
                        return;

                    MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                        "Не удалось открыть фото").ShowDialog(window);
                }
            }
        }

        #endregion

        #region Private fields

        private readonly Action? _closeRegisterPage;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> RegisterUserCommand { get; }
        public ReactiveCommand<Unit, Unit> CloseRegisterPageCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadUserImageCommand { get; }
        public ReactiveCommand<string, Unit> SetGenderCommand { get; }

        #endregion

        public RegisterViewModel() : this(null)
        {
        }

        public RegisterViewModel(Action? closeRegisterPage)
        {
            _closeRegisterPage = closeRegisterPage;

            RegisterUserCommand = ReactiveCommand.Create(RegisterUser);
            CloseRegisterPageCommand = ReactiveCommand.Create(CloseRegisterPage);
            LoadUserImageCommand = ReactiveCommand.Create(LoadUserImage);
            SetGenderCommand = ReactiveCommand.Create<string>(SetGender);
        }

        #region Private Methods

        private void RegisterUser()
        {
            throw new NotImplementedException();
        }

        private void CloseRegisterPage()
        {
            _closeRegisterPage?.Invoke();
        }

        private async void LoadUserImage()
        {
            if (WindowFinder.FindMainWindow() is not Window window)
                return;

            OpenFileDialog dialog = DialogProvider.GetOpenImageDialog();
            string[]? paths = await dialog.ShowAsync(window);

            if (paths is not null && paths.Length > 0)
                UserImagePath = paths[0];
        }

        private void SetGender(string gender)
        {
            Gender = gender;
        }

        #endregion
    }
}
