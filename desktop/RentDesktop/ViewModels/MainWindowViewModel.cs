using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using ReactiveUI;
using RentDesktop.ViewModels.Base;
using RentDesktop.ViewModels.Pages;
using RentDesktop.Views;
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
            RegisterVM = new RegisterViewModel();

            _inactivity_timer = new DispatcherTimer(
                new TimeSpan(0, 0, INACTIVITY_TIMER_INTERVAL_SECONDS),
                DispatcherPriority.Background,
                (sender, e) => VerifyInactivityStatus());

            _inactivity_timer.Start();

            ResetInactivitySecondsCommand = ReactiveCommand.Create(ResetInactivitySeconds);

            // temp
            var window = new UserWindow() { DataContext = new UserWindowViewModel() };
            window.Show();
            // end temp
        }

        #region Private Methods

        private void VerifyInactivityStatus()
        {
            _inactivity_seconds += INACTIVITY_TIMER_INTERVAL_SECONDS;

            if (_inactivity_seconds < MAX_INACTIVITY_SECONDS)
                return;

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
                lifetime.Shutdown();
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

        private void OpenRegisterPage()
        {
            HideAllPages();
            IsRegisterPageVisible = true;
        }

        private void CloseRegisterPage()
        {
            HideAllPages();
            IsRegisterPageVisible = false;
        }

        #endregion
    }
}
