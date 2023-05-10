using ReactiveUI;
using RentDesktop.Infrastructure.App;
using RentDesktop.Models.Communication;
using RentDesktop.Models.Informing;
using RentDesktop.ViewModels.Pages.UserWindowPages;
using RentDesktop.Views;
using System;
using System.Collections.ObjectModel;

namespace RentDesktop.ViewModels.Pages.AdminWindowPages
{
    public class AdminProfileViewModel : UserProfileViewModel
    {
        public AdminProfileViewModel() : this(new UserInfo())
        {
        }

        public AdminProfileViewModel(IUserInfo userInfo) : base(userInfo)
        {
            Statuses = GetStatuses();
            SetUserInfo(userInfo);
        }

        #region Properties

        public ObservableCollection<string> Statuses { get; }

        private int _selectedStatusIndex = 0;
        public int SelectedStatusIndex
        {
            get => _selectedStatusIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedStatusIndex, value);
        }

        #endregion

        #region Protected Methods

        protected override Type GetOwnerWindowType()
        {
            return typeof(AdminWindow);
        }

        protected override IUserInfo GetUserInfo()
        {
            IUserInfo userInfo = base.GetUserInfo();
            userInfo.Status = Statuses[SelectedStatusIndex];

            return userInfo;
        }

        protected override void SetUserInfo(IUserInfo userInfo)
        {
            base.SetUserInfo(userInfo);
            SelectedStatusIndex = Statuses?.IndexOf(userInfo.Status) ?? -1;
        }

        protected override bool VerifyFieldsCorrectness()
        {
            var window = WindowFinder.FindByType(GetOwnerWindowType());

            if (SelectedStatusIndex < 0 || SelectedStatusIndex > Statuses.Count)
            {
                QuickMessage.Info("Выберите статус.").ShowDialog(window);
                return false;
            }

            return base.VerifyFieldsCorrectness();
        }

        #endregion

        #region Private Methods

        private static ObservableCollection<string> GetStatuses()
        {
            // TODO
            return new ObservableCollection<string>()
            {
                UserInfo.ACTIVE_STATUS,
                UserInfo.INACTIVE_STATUS
            };
        }

        #endregion
    }
}
