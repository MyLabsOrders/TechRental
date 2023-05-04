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
    public class UserProfileViewModel : ViewModelBase
    {
        #region Properties

        private bool _isEditModeEnabled = false;
        public bool IsEditModeEnabled
        {
            get => _isEditModeEnabled;
            set => this.RaiseAndSetIfChanged(ref _isEditModeEnabled, value);
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

        #region Commands

        public ReactiveCommand<Unit, Unit> SwapEditModeCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangeUserImageCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveUserInfoCommand { get; }
        public ReactiveCommand<string, Unit> SetGenderCommand { get; }

        #endregion

        public UserProfileViewModel()
        {
            SwapEditModeCommand = ReactiveCommand.Create(SwapEditMode);
            ChangeUserImageCommand = ReactiveCommand.Create(ChangeUserImage);
            SaveUserInfoCommand = ReactiveCommand.Create(SaveUserInfo);
            SetGenderCommand = ReactiveCommand.Create<string>(SetGender);
        }

        #region Private Methods

        private void SwapEditMode()
        {
            IsEditModeEnabled = !IsEditModeEnabled;
        }

        private async void ChangeUserImage()
        {
            if (WindowFinder.FindMainWindow() is not Window window)
                return;

            OpenFileDialog dialog = DialogProvider.GetOpenImageDialog();
            string[]? paths = await dialog.ShowAsync(window);

            if (paths is not null && paths.Length > 0)
                UserImagePath = paths[0];
        }

        private void SaveUserInfo()
        {
            throw new NotImplementedException();
        }

        private void SetGender(string gender)
        {
            Gender = gender;
        }

        #endregion
    }
}
