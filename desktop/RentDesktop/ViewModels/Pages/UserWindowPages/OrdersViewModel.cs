using RentDesktop.Models;
using RentDesktop.ViewModels.Base;
using System.Collections.ObjectModel;

namespace RentDesktop.ViewModels.Pages.UserWindowPages
{
    public class OrdersViewModel : ViewModelBase
    {
        public OrdersViewModel()
        {
            Orders = new ObservableCollection<Order>();
        }

        #region Properties

        public ObservableCollection<Order> Orders { get; }

        #endregion
    }
}
