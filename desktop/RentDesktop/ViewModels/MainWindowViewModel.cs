using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;
using RentDesktop.Infrastructure.App;
using RentDesktop.ViewModels.Base;
using RentDesktop.ViewModels.Pages;
using System;
using System.Reactive;

namespace RentDesktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region ViewModels

        public LoginViewModel LoginVM { get; }
        public RegisterViewModel RegisterVM { get; }

        #endregion

        #region Properties

        private bool _isLoginPageVisible = true;
        public bool IsLoginPageVisible
        {
            get => _isLoginPageVisible;
            private set => this.RaiseAndSetIfChanged(ref _isLoginPageVisible, value);
        }

        private bool _isRegisterPageVisible = false;
        public bool IsRegisterPageVisible
        {
            get => _isRegisterPageVisible;
            private set => this.RaiseAndSetIfChanged(ref _isRegisterPageVisible, value);
        }

        #endregion

        #region Private fields

        #region Constants

        private const int MAX_INACTIVITY_SECONDS = 60;
        private const int INACTIVITY_TIMER_INTERVAL_SECONDS = 1;

        #endregion

        private readonly DispatcherTimer _inactivity_timer;
        private int _inactivity_seconds = 0;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> ResetInactivitySecondsCommand { get; }

        #endregion

        public MainWindowViewModel()
        {
            LoginVM = new LoginViewModel(OpenRegisterPage);
            RegisterVM = new RegisterViewModel(OpenLoginPage);

            _inactivity_timer = ConfigureInactivityTimer();
            _inactivity_timer.Start();

            ResetInactivitySecondsCommand = ReactiveCommand.Create(ResetInactivitySeconds);
        }

        #region Public Methods

        public void HideWindow()
        {
            ResetInactivitySeconds();
            _inactivity_timer.Stop();

            Window? mainWindow = WindowFinder.FindMainWindow();
            mainWindow?.Hide();
        }

        public void ShowWindow()
        {
            ResetInactivitySeconds();
            _inactivity_timer.Start();

            Window? mainWindow = WindowFinder.FindMainWindow();
            mainWindow?.Show();
        }

        #endregion

        #region Private Methods

        private DispatcherTimer ConfigureInactivityTimer()
        {
            return new DispatcherTimer(
                new TimeSpan(0, 0, INACTIVITY_TIMER_INTERVAL_SECONDS),
                DispatcherPriority.Background,
                (sender, e) => VerifyInactivityStatus());
        }

        private void VerifyInactivityStatus()
        {
            _inactivity_seconds += INACTIVITY_TIMER_INTERVAL_SECONDS;

            if (_inactivity_seconds < MAX_INACTIVITY_SECONDS)
                return;

            ResetInactivitySeconds();

            if (IsRegisterPageVisible)
            {
                OpenLoginPage();
            }
            else
            {
                _inactivity_timer.Stop();
                AppInteraction.CloseCurrentApp();
            }
        }

        private void ResetInactivitySeconds()
        {
            _inactivity_seconds = 0;
        }

        private void HideAllPages()
        {
            IsLoginPageVisible = false;
            IsRegisterPageVisible = false;
        }

        private void OpenLoginPage()
        {
            HideAllPages();
            IsLoginPageVisible = true;
        }

        private void OpenRegisterPage()
        {
            HideAllPages();
            IsRegisterPageVisible = true;
        }

        #endregion
    }
}
