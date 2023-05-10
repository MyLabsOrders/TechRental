using ReactiveUI;
using RentDesktop.Models.Informing;
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
            ChangeUserCommand = ReactiveCommand.Create<IUserInfo>(ChangeUser);
        }

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
    }
}
