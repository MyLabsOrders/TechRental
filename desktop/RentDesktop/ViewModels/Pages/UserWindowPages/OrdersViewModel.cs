using RentDesktop.Models;
using RentDesktop.ViewModels.Base;
using System.Collections.ObjectModel;

namespace RentDesktop.ViewModels.Pages.UserWindowPages
{
    public class OrdersViewModel : ViewModelBase
    {
        public OrdersViewModel() : this(new ObservableCollection<Order>())
        {
        }

        public OrdersViewModel(ObservableCollection<Order> orders)
        {
            Orders = orders;
        }

        #region Properties

        public ObservableCollection<Order> Orders { get; }

        #endregion
    }
}
