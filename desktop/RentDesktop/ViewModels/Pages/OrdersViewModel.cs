using RentDesktop.Models;
using RentDesktop.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
