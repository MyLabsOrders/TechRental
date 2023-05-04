using RentDesktop.Models;
using RentDesktop.ViewModels.Base;
using System.Collections.ObjectModel;

namespace RentDesktop.ViewModels.Pages
{
    public class OrdersViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<Order> Orders { get; }

        #endregion

        public OrdersViewModel()
        {
            Orders = new ObservableCollection<Order>();
        }
    }
}
