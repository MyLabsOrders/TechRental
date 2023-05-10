using ReactiveUI;
using RentDesktop.Infrastructure.App;
using RentDesktop.Infrastructure.Services.DB;
using RentDesktop.Models.Communication;
using RentDesktop.Models.Informing;
using RentDesktop.ViewModels.Pages.MainWindowPages;
using RentDesktop.Views;
using System;
using System.Collections.ObjectModel;
using System.Reactive;

namespace RentDesktop.ViewModels.Pages.AdminWindowPages
{
    public class AddUserViewModel : RegisterViewModel
    {
        public AddUserViewModel() : base()
        {
            Positions = GetPositions();
            ResetAllFieldsCommand = ReactiveCommand.Create(ResetAllFields);
        }

        #region Properties

        public ObservableCollection<string> Positions { get; }

        private int _selectedPositionIndex = 0;
        public int SelectedPositionIndex
        {
            get => _selectedPositionIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedPositionIndex, value);
        }

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> ResetAllFieldsCommand { get; }

        #endregion

        #region Protected Methods

        protected override Type GetOwnerWindowType()
        {
            return typeof(AdminWindow);
        }

        protected override IUserInfo GetUserInfo()
        {
            IUserInfo userInfo = base.GetUserInfo();
            userInfo.Position = Positions[SelectedPositionIndex];

            return userInfo;
        }

        protected override bool VerifyFieldsCorrectness()
        {
            var window = WindowFinder.FindByType(GetOwnerWindowType());

            if (SelectedPositionIndex < 0 || SelectedPositionIndex > Positions.Count)
            {
                QuickMessage.Info("Выберите должность.").ShowDialog(window);
                return false;
            }

            return base.VerifyFieldsCorrectness();
        }

        protected override void ResetAllFields()
        {
            base.ResetAllFields();

            PasswordConfirmation = string.Empty;
            SelectedPositionIndex = 0;
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
