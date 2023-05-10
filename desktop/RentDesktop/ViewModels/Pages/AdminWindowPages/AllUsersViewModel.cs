using ReactiveUI;
using RentDesktop.Models.Informing;
using RentDesktop.ViewModels.Base;

namespace RentDesktop.ViewModels.Pages.AdminWindowPages
{
    public class AllUsersViewModel : ViewModelBase
    {
        public AllUsersViewModel()
        {
        }

        #region Properties

        private IUserInfo? _selectedUserInfo = null;
        public IUserInfo? SelectedUserInfo
        {
            get => _selectedUserInfo;
            private set => this.RaiseAndSetIfChanged(ref _selectedUserInfo, value);
        }

        private bool _isUserSelected = false;
        public bool IsUserSelected
        {
            get => _isUserSelected;
            set => this.RaiseAndSetIfChanged(ref _isUserSelected, value);
        }

        #endregion

        #region Commands

        #endregion

        #region Private Methods

        #endregion
    }
}
