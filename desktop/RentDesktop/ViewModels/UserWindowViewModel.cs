using RentDesktop.ViewModels.Base;
using RentDesktop.ViewModels.Pages;

namespace RentDesktop.ViewModels
{
    public class UserWindowViewModel : ViewModelBase
    {
        public UserViewModel UserVM { get; }
        public TransportViewModel TransportVM { get; }
        public CartViewModel CartVM { get; }

        public UserWindowViewModel()
        {
            UserVM = new UserViewModel();
            TransportVM = new TransportViewModel();
            CartVM = new CartViewModel();
        }
    }
}
