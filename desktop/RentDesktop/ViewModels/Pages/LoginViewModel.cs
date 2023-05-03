using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using RentDesktop.Infrastructure.Services;
using RentDesktop.Models;
using RentDesktop.ViewModels.Base;
using System;
using System.Reactive;

namespace RentDesktop.ViewModels.Pages
{
    public class LoginViewModel : ViewModelBase
    {
        #region Properties

        public ICaptcha Captcha { get; } = new Captcha();

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

        private bool _rememberUser = true;
        public bool RememberUser
        {
            get => _rememberUser;
            set => this.RaiseAndSetIfChanged(ref _rememberUser, value);
        }

        private string _userCaptchaText = string.Empty;
        public string UserCaptchaText
        {
            get => _userCaptchaText;
            set => this.RaiseAndSetIfChanged(ref _userCaptchaText, value);
        }

        #endregion

        #region Private fields

        #region Constants

        private const char HIDDEN_PASSWORD_CHAR = '*';

        #endregion

        private readonly Action? _openRegisterPage;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> LoadLoginInfoCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdateCaptchaCommand { get; }
        public ReactiveCommand<Unit, Unit> EnterSystemCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenRegisterPageCommand { get; }
        public ReactiveCommand<Unit, Unit> CloseProgramCommand { get; }

        #endregion

        public LoginViewModel() : this(null)
        {
        }

        public LoginViewModel(Action? openRegisterPage)
        {
            _openRegisterPage = openRegisterPage;

            LoadLoginInfoCommand = ReactiveCommand.Create(LoadLoginInfo);
            UpdateCaptchaCommand = ReactiveCommand.Create(UpdateCaptcha);
            EnterSystemCommand = ReactiveCommand.Create(EnterSystem);
            OpenRegisterPageCommand = ReactiveCommand.Create(OpenRegisterPage);
            CloseProgramCommand = ReactiveCommand.Create(CloseProgram);
        }

        #region Private Methods

        private void UpdateCaptcha()
        {
            Captcha.UpdateText();
        }

        private void OpenRegisterPage()
        {
            _openRegisterPage?.Invoke();
        }

        private void EnterSystem()
        {
            if (RememberUser)
                SaveLoginInfo();

            throw new NotImplementedException();
        }

        private void CloseProgram()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
                lifetime.Shutdown();
        }

        private void LoadLoginInfo()
        {
            if (UserInfoSaveService.TryLoadInfo(out var info))
            {
                Login = info.Login;
                Password = info.Password;
            }
        }

        private void SaveLoginInfo()
        {
            UserInfoSaveService.TrySaveInfo(Login, Password);
        }

        #endregion
    }
}
