using RentDesktop.Models;
using RentDesktop.ViewModels.Base;
using System.Collections.ObjectModel;

namespace RentDesktop.ViewModels.Pages
{
    public class TransportViewModel : ViewModelBase
    {
        public Transport? SelectedTransport { get; private set; }

        public ObservableCollection<Transport> Transports { get; }

        public TransportViewModel()
        {
            Transports = new ObservableCollection<Transport>()
            {
                new Transport("Opel", "Company 1", 100000),
                new Transport("Mazda", "Company 2", 5900000),
                new Transport("BMW", "Company 3", 30000),
                new Transport("Mercedes", "Company 4", 4500000)
            };

            SelectedTransport = Transports[1];
        }
    }
}
