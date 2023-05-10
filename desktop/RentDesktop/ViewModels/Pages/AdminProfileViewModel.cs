using ReactiveUI;
using RentDesktop.Infrastructure.App;
using RentDesktop.Models.Communication;
using RentDesktop.Models.Informing;
using RentDesktop.Views;
using System;
using System.Collections.ObjectModel;

namespace RentDesktop.ViewModels.Pages
{
    public class AdminProfileViewModel : UserProfileViewModel
    {
        public AdminProfileViewModel() : this(new UserInfo())
        {
        }

        public AdminProfileViewModel(IUserInfo userInfo) : base(userInfo)
        {
            Statuses = GetStatuses();
        }

        #region Properties

        public ObservableCollection<string> Statuses { get; }

        private string _status = string.Empty;
        public string Status
        {
            get => _status;
            set => this.RaiseAndSetIfChanged(ref _status, value);
        }

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
            userInfo.Status = Status;

            return userInfo;
        }

        protected override void SetUserInfo(IUserInfo userInfo)
        {
            base.SetUserInfo(userInfo);
            Status = userInfo.Status;
        }

        protected override bool VerifyFieldsCorrectness()
        {
            var window = WindowFinder.FindByType(GetOwnerWindowType());

            if (string.IsNullOrEmpty(Status))
            {
                QuickMessage.Info("Введите статус.").ShowDialog(window);
                return false;
            }

            return base.VerifyFieldsCorrectness();
        }

        #endregion

        #region Private Methods

        private ObservableCollection<string> GetStatuses()
        {
            // TODO
            return new ObservableCollection<string>()
            {
                "Активен",
                "Неактивен"
            };
        }

        #endregion
    }
}
