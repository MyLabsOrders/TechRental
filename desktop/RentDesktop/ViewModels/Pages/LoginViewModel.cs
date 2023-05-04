using Avalonia.Threading;
using ReactiveUI;
using RentDesktop.Infrastructure.App;
using RentDesktop.Infrastructure.Services;
using RentDesktop.Infrastructure.Services.DB;
using RentDesktop.Models.Communication;
using RentDesktop.Models.Informing;
using RentDesktop.Models.Security;
using RentDesktop.ViewModels.Base;
using RentDesktop.Views;
using System.Reactive;
using System.Threading.Tasks;

namespace RentDesktop.ViewModels.Pages
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            LoadLoginInfoCommand = ReactiveCommand.Create(LoadLoginInfo);
            UpdateCaptchaCommand = ReactiveCommand.Create(UpdateCaptcha);
            EnterSystemCommand = ReactiveCommand.Create(EnterSystem);
            OpenRegisterPageCommand = ReactiveCommand.Create(OpenRegisterPage);
            CloseProgramCommand = ReactiveCommand.Create(CloseProgram);
        }

        #region Events

        public delegate void RegisterPageOpeningHandler();
        public event RegisterPageOpeningHandler? RegisterPageOpening;

        #endregion

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
        private const int HIDE_MAIN_WINDOW_AFTER_MILLISECONDS = 10;

        #endregion

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> LoadLoginInfoCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdateCaptchaCommand { get; }
        public ReactiveCommand<Unit, Unit> EnterSystemCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenRegisterPageCommand { get; }
        public ReactiveCommand<Unit, Unit> CloseProgramCommand { get; }

        #endregion

        #region Private Methods

        private void UpdateCaptcha()
        {
            Captcha.UpdateText();
        }

        private void OpenRegisterPage()
        {
            RegisterPageOpening?.Invoke();
        }

        private void EnterSystem()
        {
            if (!VerifyFieldsCorrectness())
                return;

            if (!LoginService.Login(Login, Password, out IUserInfo? userInfo) || userInfo is null)
            {
                var window = WindowFinder.FindMainWindow();
                QuickMessage.Error("Не удалось войти в систему.").ShowDialog(window);
                return;
            }

            if (RememberUser)
                SaveLoginInfo();
            else
                ClearLoginInfo();

            ResetAllFields(true);

            var viewModel = new UserWindowViewModel(userInfo);
            var userWindow = new UserWindow() { DataContext = viewModel };

            userWindow.Show();

            _ = Task.Delay(HIDE_MAIN_WINDOW_AFTER_MILLISECONDS).ContinueWith(t =>
            {
                Dispatcher.UIThread.Post(AppInteraction.HideMainWindow);
            });
        }

        private void CloseProgram()
        {
            AppInteraction.CloseMainWindow();
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
            _ = UserInfoSaveService.TrySaveInfo(Login, Password);
        }

        private void ClearLoginInfo()
        {
            _ = UserInfoSaveService.TryClearInfo();
        }

        private bool VerifyFieldsCorrectness()
        {
            var window = WindowFinder.FindMainWindow();

            if (string.IsNullOrEmpty(Login))
            {
                QuickMessage.Info("Введите логин.").ShowDialog(window);
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                QuickMessage.Info("Введите пароль.").ShowDialog(window);
                return false;
            }
            if (UserCaptchaText != Captcha.Text)
            {
                QuickMessage.Info("Текст с картинки введен неверно.").ShowDialog(window);
                UpdateCaptcha();
                return false;
            }

            return true;
        }

        private void ResetAllFields(bool leaveLoginInfo = false)
        {
            if (!leaveLoginInfo)
            {
                Login = string.Empty;
                Password = string.Empty;
                RememberUser = true;
            }

            UserCaptchaText = string.Empty;
            ShowPassword = false;

            UpdateCaptcha();
        }

        #endregion
    }
}
