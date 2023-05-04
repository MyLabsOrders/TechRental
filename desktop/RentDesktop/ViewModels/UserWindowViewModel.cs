using Avalonia.Threading;
using ReactiveUI;
using RentDesktop.ViewModels.Base;
using RentDesktop.ViewModels.Pages;
using System;

namespace RentDesktop.ViewModels
{
    public class UserWindowViewModel : ViewModelBase
    {
        #region ViewModels

        public UserViewModel UserVM { get; }
        public TransportViewModel TransportVM { get; }
        public CartViewModel CartVM { get; }

        #endregion

        #region Properties

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

        private const int PRELOAD_TABS_TIMER_INTERVAL_MILLISECONDS = 5;

        #endregion

        private int _preloadedTabs = 0;
        private readonly DispatcherTimer _preloadTabsTimer = new(DispatcherPriority.MaxValue);

        #endregion

        public UserWindowViewModel()
        {
            UserVM = new UserViewModel();
            CartVM = new CartViewModel();
            TransportVM = new TransportViewModel(OpenCartTab, CartVM.Cart);

            _preloadTabsTimer.Interval = new TimeSpan(0, 0, 0, 0, PRELOAD_TABS_TIMER_INTERVAL_MILLISECONDS);
            _preloadTabsTimer.Tick += PreloadTabs;
            _preloadTabsTimer.Start();
        }

        #region Private Methods

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

        private void PreloadTabs(object? sender, EventArgs e)
        {
            switch (_preloadedTabs++)
            {
                case 0:
                    OpenTransportTab();
                    break;

                case 1:
                    OpenCartTab();
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
