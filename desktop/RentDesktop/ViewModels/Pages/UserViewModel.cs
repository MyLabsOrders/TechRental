using ReactiveUI;
using RentDesktop.ViewModels.Base;

namespace RentDesktop.ViewModels.Pages
{
    public class UserViewModel : ViewModelBase
    {
        private bool _isEditModeEnabled = false;
        public bool IsEditModeEnabled
        {
            get => _isEditModeEnabled;
            set => this.RaiseAndSetIfChanged(ref _isEditModeEnabled, value);
        }
    }
}
