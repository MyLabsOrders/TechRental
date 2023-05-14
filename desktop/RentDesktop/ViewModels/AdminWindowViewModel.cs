using Avalonia.Threading;
using ReactiveUI;
using RentDesktop.Infrastructure.App;
using RentDesktop.Models.Communication;
using RentDesktop.Models.Informing;
using RentDesktop.ViewModels.Base;
using RentDesktop.ViewModels.Pages.AdminWindowPages;
using RentDesktop.Views;
using System;
using System.Reactive;

namespace RentDesktop.ViewModels
{
    public class AdminWindowViewModel : ViewModelBase
    {
        public AdminWindowViewModel() : this(new UserInfo())
        {
        }

        public AdminWindowViewModel(IUserInfo userInfo)
        {
            AdminProfileVM = new AdminProfileViewModel(userInfo);
            AllUsersVM = new AllUsersViewModel();
            AddUserVM = new AddUserViewModel();
            EditUserVM = new EditUserViewModel();

            AllUsersVM.SelectedUserChanged += EditUserVM.ChangeUser;

            EditUserVM.UserInfoUpdated += () =>
            {
                var window = WindowFinder.FindByType(typeof(AdminWindow));
                QuickMessage.Info("Изменения успешно сохранены.").ShowDialog(window);
            };

            UserInfo = userInfo;

            _inactivity_timer = ConfigureInactivityTimer();
            _inactivity_timer.Start();

            ResetInactivitySecondsCommand = ReactiveCommand.Create(ResetInactivitySeconds);
            ShowMainWindowCommand = ReactiveCommand.Create(ShowMainWindow);
            DisposeUserImageCommand = ReactiveCommand.Create(DisposeUserImage);
        }

        #region ViewModels

        public AdminProfileViewModel AdminProfileVM { get; }
        public AllUsersViewModel AllUsersVM { get; }
        public AddUserViewModel AddUserVM { get; }
        public EditUserViewModel EditUserVM { get; }

        #endregion

        #region Properties

        public IUserInfo UserInfo { get; }

        private int _selectedTabIndex = 0;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedTabIndex, value);
        }

        #endregion

        #region Private Fields

        #region Constants

        private const int MAX_INACTIVITY_SECONDS = 60 * 2;
        private const int INACTIVITY_TIMER_INTERVAL_SECONDS = 1;

        #endregion

        private readonly DispatcherTimer _inactivity_timer;
        private int _inactivity_seconds = 0;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> ResetInactivitySecondsCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowMainWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> DisposeUserImageCommand { get; }

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

            _inactivity_timer.Stop();
            ResetInactivitySeconds();

            AppInteraction.CloseUserWindow();
        }

        private void ResetInactivitySeconds()
        {
            _inactivity_seconds = 0;
        }

        private void ShowMainWindow()
        {
            AppInteraction.ShowMainWindow();
        }

        private void DisposeUserImage()
        {
            AdminProfileVM.UserImage?.Dispose();
        }

        #endregion
    }
}
