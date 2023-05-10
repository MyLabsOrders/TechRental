using ReactiveUI;
using RentDesktop.Infrastructure.Services.DB;
using RentDesktop.Models.Informing;
using System.Collections.ObjectModel;
using System.Reactive;

namespace RentDesktop.ViewModels.Pages.AdminWindowPages
{
    public class EditUserViewModel : AdminProfileViewModel
    {
        public EditUserViewModel() : this(new UserInfo())
        {
        }

        public EditUserViewModel(IUserInfo userInfo) : base(userInfo)
        {
            Positions = GetPositions();
            ChangeUserCommand = ReactiveCommand.Create<IUserInfo>(ChangeUser);
        }

        #region Properties

        public ObservableCollection<string> Positions { get; }

        #endregion

        #region Commands

        public ReactiveCommand<IUserInfo, Unit> ChangeUserCommand { get; }

        #endregion

        #region Public Methods

        public void ChangeUser(IUserInfo? newUserInfo)
        {
            _userInfo = newUserInfo ?? new UserInfo();
            SetUserInfo(_userInfo);
        }

        #endregion

        #region Private Methods

        private static ObservableCollection<string> GetPositions()
        {
            var positions = InfoService.GetAllPositions();
            return new ObservableCollection<string>(positions);
        }

        #endregion
    }
}
