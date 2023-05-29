using ReactiveUI;
using RentDesktop.Infrastructure.Services.DB;
using RentDesktop.Models;
using RentDesktop.Models.Communication;
using RentDesktop.ViewModels.Base;
using RentDesktop.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace RentDesktop.ViewModels.Pages.UserWindowPages
{
    public class TransportViewModel : ViewModelBase
    {
        public TransportViewModel() : this(new ObservableCollection<TransportRent>())
        {
        }

        public TransportViewModel(ObservableCollection<TransportRent> cart)
        {
            Transports = GetTransports();
            _cart = cart;

            SelectTransportCommand = ReactiveCommand.Create<Transport>(SelectTransport);
            AddToCartCommand = ReactiveCommand.Create<Transport>(AddToCart);
            OpenCartCommand = ReactiveCommand.Create(OpenCartTab);
        }

        #region Events

        public delegate void CartTabOpeningHandler();
        public event CartTabOpeningHandler? CartTabOpening;

        public delegate void TransportAddingToCartHandler(Transport transport);
        public event TransportAddingToCartHandler? TransportAddingToCart;

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
            private set => this.RaiseAndSetIfChanged(ref _isTransportSelected, value);
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

        #region Private Methods

        private void SelectTransport(Transport transport)
        {
            SelectedTransport = transport;
        }

        private void AddToCart(Transport transport)
        {
            TransportAddingToCart?.Invoke(transport);

            TransportRent? existingCartItem = _cart.FirstOrDefault(t => t.Transport.ID == transport.ID);
            int days = existingCartItem is null ? 1 : existingCartItem.Days;

            var transportRent = new TransportRent(transport.Copy(), days);
            _cart.Add(transportRent);

            SelectedTransport = null;
        }

        private void OpenCartTab()
        {
            CartTabOpening?.Invoke();
        }

        private static ObservableCollection<Transport> GetTransports()
        {
            try
            {
                var transport = ShopService.GetTransports();
                return new ObservableCollection<Transport>(transport);
            }
            catch (Exception ex)
            {
                string message = "Не удалось получить коллекцию доступных товаров.";
#if DEBUG
                message += $" Причина: {ex.Message}";
#endif
                QuickMessage.Error(message).ShowDialog(typeof(UserWindow));
                return new ObservableCollection<Transport>();
            }
        }

        #endregion
    }
}
