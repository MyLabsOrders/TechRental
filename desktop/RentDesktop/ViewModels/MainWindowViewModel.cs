using ReactiveUI;
using RentDesktop.ViewModels.Base;
using RentDesktop.ViewModels.Pages;
using RentDesktop.Views;

namespace RentDesktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public RegisterViewModel LoginVM { get; }
        public RegisterViewModel RegisterVM { get; }
        
        private bool _isLoginPageVisible = true;
        public bool IsLoginPageVisible
        {
            get => _isLoginPageVisible;
            set => this.RaiseAndSetIfChanged(ref _isLoginPageVisible, value);
        }

        private bool _isRegisterPageVisible = false;
        public bool IsRegisterPageVisible
        {
            get => _isRegisterPageVisible;
            set => this.RaiseAndSetIfChanged(ref _isRegisterPageVisible, value);
        }

        public MainWindowViewModel()
        {
            LoginVM = new RegisterViewModel();
            RegisterVM = new RegisterViewModel();

            var window = new UserWindow() { DataContext = new UserWindowViewModel() };
            window.Show();
        }
    }
}
