using Avalonia.Threading;
using ReactiveUI;
using RentDesktop.Infrastructure.App;
using RentDesktop.Infrastructure.Services.DB;
using RentDesktop.Models.Informing;
using RentDesktop.ViewModels.Base;
using RentDesktop.ViewModels.Pages;
using System;
using System.Reactive;

namespace RentDesktop.ViewModels
{
    internal class UserWindowViewModel : ViewModelBase
    {
        public UserWindowViewModel() : this(new UserInfo())
        {
        }

        public UserWindowViewModel(IUserInfo userInfo)
        {
            UserProfileVM = new UserProfileViewModel(userInfo);
            OrdersVM = new OrdersViewModel();
            CartVM = new CartViewModel(userInfo, OrdersVM.Orders);
            TransportVM = new TransportViewModel(CartVM.Cart);

            UserProfileVM.UserInfoUpdated += CartVM.UpdateUserInfo;
            CartVM.OrdersTabOpening += OpenOrdersTab;
            TransportVM.CartTabOpening += OpenCartTab;

            UserInfo = userInfo;

            _preloadTabsTimer = ConfigurePreloadTabsTimer();
            _preloadTabsTimer.Start();

            _inactivity_timer = ConfigureInactivityTimer();
            _inactivity_timer.Start();

            ResetInactivitySecondsCommand = ReactiveCommand.Create(ResetInactivitySeconds);
            ShowMainWindowCommand = ReactiveCommand.Create(ShowMainWindow);
            SaveOrdersStatusCommand = ReactiveCommand.Create(SaveOrdersStatus);
            DisposeUserImageCommand = ReactiveCommand.Create(DisposeUserImage);
        }

        #region ViewModels

        public UserProfileViewModel UserProfileVM { get; }
        public TransportViewModel TransportVM { get; }
        public CartViewModel CartVM { get; }
        public OrdersViewModel OrdersVM { get; }

        #endregion

        #region Properties

        public IUserInfo UserInfo { get; }

        private int _selectedTabIndex = 1;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedTabIndex, value);
        }

        #endregion

        #region Private Fields

        #region Constants

        private const int USER_TAB_INDEX = 0;
        private const int TRANSPORT_TAB_INDEX = 1;
        private const int CART_TAB_INDEX = 2;
        private const int ORDERS_TAB_INDEX = 3;

        private const int PRELOAD_TABS_TIMER_INTERVAL_MILLISECONDS = 5;

        private const int MAX_INACTIVITY_SECONDS = 60 * 2;
        private const int INACTIVITY_TIMER_INTERVAL_SECONDS = 1;

        #endregion

        private readonly DispatcherTimer _preloadTabsTimer;
        private int _preloadedTabs = 0;

        private readonly DispatcherTimer _inactivity_timer;
        private int _inactivity_seconds = 0;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> ResetInactivitySecondsCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowMainWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveOrdersStatusCommand { get; }
        public ReactiveCommand<Unit, Unit> DisposeUserImageCommand { get; }

        #endregion

        #region Private Methods

        private DispatcherTimer ConfigurePreloadTabsTimer()
        {
            return new DispatcherTimer(
                new TimeSpan(0, 0, 0, 0, PRELOAD_TABS_TIMER_INTERVAL_MILLISECONDS),
                DispatcherPriority.MaxValue,
                (sender, e) => PreloadTabs());
        }

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

        private void OpenUserTab()
        {
            SelectedTabIndex = USER_TAB_INDEX;
        }

        private void OpenTransportTab()
        {
            SelectedTabIndex = TRANSPORT_TAB_INDEX;
        }

        private void OpenCartTab()
        {
            SelectedTabIndex = CART_TAB_INDEX;
        }

        private void OpenOrdersTab()
        {
            SelectedTabIndex = ORDERS_TAB_INDEX;
        }

        private void ShowMainWindow()
        {
            AppInteraction.ShowMainWindow();
        }

        private void SaveOrdersStatus()
        {
            OrdersService.SaveOrdersStatus(OrdersVM.Orders, UserInfo);
        }

        private void DisposeUserImage()
        {
            UserProfileVM.UserImage?.Dispose();
        }

        private void PreloadTabs()
        {
            switch (_preloadedTabs++)
            {
                case 0:
                    OpenTransportTab();
                    break;

                case 1:
                    OpenCartTab();
                    break;

                case 2:
                    OpenOrdersTab();
                    break;

                default:
                    OpenUserTab();
                    _preloadTabsTimer.Stop();
                    break;
            }
        }

        #endregion
    }
}
