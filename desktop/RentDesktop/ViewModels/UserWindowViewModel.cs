using ReactiveUI;
using RentDesktop.ViewModels.Base;
using RentDesktop.ViewModels.Pages;

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
        private const int CART_TAB_INDEX = 2;

        #endregion

        #endregion

        public UserWindowViewModel()
        {
            UserVM = new UserViewModel();
            TransportVM = new TransportViewModel(OpenCartTab, OpenUserTab);
            CartVM = new CartViewModel();
        }

        #region Private Methods

        private void OpenUserTab()
        {
            SelectedTabIndex = USER_TAB_INDEX;
        }

        private void OpenCartTab()
        {
            SelectedTabIndex = CART_TAB_INDEX;
        }

        #endregion
    }
}
