using ReactiveUI;
using RentDesktop.Models;
using RentDesktop.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Reactive;

namespace RentDesktop.ViewModels.Pages
{
    public class TransportViewModel : ViewModelBase
    {
        #region Events

        public delegate void CartTabOpeningHandler();
        public event CartTabOpeningHandler? CartTabOpening;

        #endregion

        #region Properties

        public ObservableCollection<Transport> Transports { get; }

        private Transport? _selectedTransport = null;
        public Transport? SelectedTransport
        {
            get => _selectedTransport;
            private set
            {
                this.RaiseAndSetIfChanged(ref _selectedTransport, value);

                IsTransportSelected = value is not null;
                SelectedTransportName = value is not null ? value.Name : string.Empty;
                SelectedTransportCompany = value is not null ? value.Company : string.Empty;
                SelectedTransportPrice = value is not null ? value.Price.ToString() : string.Empty;
            }
        }

        private string _selectedTransportName = string.Empty;
        public string SelectedTransportName
        {
            get => $"Название: {_selectedTransportName}";
            private set => this.RaiseAndSetIfChanged(ref _selectedTransportName, value);
        }

        private string _selectedTransportCompany = string.Empty;
        public string SelectedTransportCompany
        {
            get => $"Компания: {_selectedTransportCompany}";
            private set => this.RaiseAndSetIfChanged(ref _selectedTransportCompany, value);
        }

        private string _selectedTransportPrice = string.Empty;
        public string SelectedTransportPrice
        {
            get => $"Цена: {_selectedTransportPrice}";
            private set => this.RaiseAndSetIfChanged(ref _selectedTransportPrice, value);
        }

        private bool _isTransportSelected = false;
        public bool IsTransportSelected
        {
            get => _isTransportSelected;
            set => this.RaiseAndSetIfChanged(ref _isTransportSelected, value);
        }

        #endregion

        #region Private Fields

        private readonly ObservableCollection<TransportRent> _cart;

        #endregion

        #region Commands

        public ReactiveCommand<Transport, Unit> SelectTransportCommand { get; }
        public ReactiveCommand<Transport, Unit> AddToCartCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenCartCommand { get; }

        #endregion

        public TransportViewModel() : this(new ObservableCollection<TransportRent>())
        {
        }

        public TransportViewModel(ObservableCollection<TransportRent> cart)
        {
            // temp
            Transports = new ObservableCollection<Transport>()
            {
                new Transport("Opel", "Company 1", 100000),
                new Transport("Mazda", "Company 2", 5900000),
                new Transport("BMW", "Company 3", 30000),
                new Transport("Mercedes", "Company 4", 4500000)
            };
            // end temp

            _cart = cart;

            SelectTransportCommand = ReactiveCommand.Create<Transport>(SelectTransport);
            AddToCartCommand = ReactiveCommand.Create<Transport>(AddToCart);
            OpenCartCommand = ReactiveCommand.Create(OpenCartTab);
        }

        #region Private Methods

        private void SelectTransport(Transport transport)
        {
            SelectedTransport = transport;
        }

        private void AddToCart(Transport transport)
        {
            var transportCopy = transport.Copy();
            var item = new TransportRent(transportCopy, 1);

            _cart.Add(item);
        }

        private void OpenCartTab()
        {
            CartTabOpening?.Invoke();
        }

        #endregion
    }
}
