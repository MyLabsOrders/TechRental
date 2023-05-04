using Avalonia.Controls;
using ReactiveUI;
using RentDesktop.Models;
using RentDesktop.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;

namespace RentDesktop.ViewModels.Pages
{
    public class CartViewModel : ViewModelBase
    {
        #region Events

        public delegate void OrdersTabOpeningHandler();
        public event OrdersTabOpeningHandler? OrdersTabOpening;

        #endregion

        #region Properties

        #region Cart Subpage

        public ObservableCollection<TransportRent> Cart { get; }

        private TransportRent? _selectedTransportRent = null;
        public TransportRent? SelectedTransportRent
        {
            get => _selectedTransportRent;
            private set
            {
                this.RaiseAndSetIfChanged(ref _selectedTransportRent, value);

                IsTransportRentSelected = value is not null;
                SelectedTransportName = value is not null ? value.Transport.Name : string.Empty;
                SelectedTransportPrice = value is not null ? value.Transport.Price.ToString() : string.Empty;
            }
        }

        private string _selectedTransportName = string.Empty;
        public string SelectedTransportName
        {
            get => $"Название: {_selectedTransportName}";
            private set => this.RaiseAndSetIfChanged(ref _selectedTransportName, value);
        }

        private string _selectedTransportPrice = string.Empty;
        public string SelectedTransportPrice
        {
            get => $"Цена: {_selectedTransportPrice}";
            private set => this.RaiseAndSetIfChanged(ref _selectedTransportPrice, value);
        }

        private bool _isTransportRentSelected = false;
        public bool IsTransportRentSelected
        {
            get => _isTransportRentSelected;
            private set => this.RaiseAndSetIfChanged(ref _isTransportRentSelected, value);
        }

        private double _totalPrice = 0;
        public double TotalPrice
        {
            get => _totalPrice;
            private set => this.RaiseAndSetIfChanged(ref _totalPrice, value);
        }

        private bool _isCartNotEmpty = false;
        public bool IsCartNotEmpty
        {
            get => _isCartNotEmpty;
            private set => this.RaiseAndSetIfChanged(ref _isCartNotEmpty, value);
        }

        private bool _isCartPageVisible = true;
        public bool IsCartPageVisible
        {
            get => _isCartPageVisible;
            private set => this.RaiseAndSetIfChanged(ref _isCartPageVisible, value);
        }

        private bool _isOrderPlacingPageVisible = false;
        public bool IsOrderPlacingPageVisible
        {
            get => _isOrderPlacingPageVisible;
            private set => this.RaiseAndSetIfChanged(ref _isOrderPlacingPageVisible, value);
        }

        private bool _isOrderPaymentPageVisible = false;
        public bool IsOrderPaymentPageVisible
        {
            get => _isOrderPaymentPageVisible;
            private set => this.RaiseAndSetIfChanged(ref _isOrderPaymentPageVisible, value);
        }

        #endregion

        #region Order Placing Subpage

        public ObservableCollection<string> PaymentMethods { get; }

        private string _userName = string.Empty;
        public string UserName
        {
            get => _userName;
            private set => this.RaiseAndSetIfChanged(ref _userName, value);
        }

        private string _userPhoneNumber = string.Empty;
        public string UserPhoneNumber
        {
            get => _userPhoneNumber;
            private set => this.RaiseAndSetIfChanged(ref _userPhoneNumber, value);
        }

        private int _paymentMethodIndex = 0;
        public int PaymentMethodIndex
        {
            get => _paymentMethodIndex;
            set => this.RaiseAndSetIfChanged(ref _paymentMethodIndex, value);
        }

        #endregion

        #region Order Payment Subpage

        private bool _isOrderPaidFor = false;
        public bool IsOrderPaidFor
        {
            get => _isOrderPaidFor;
            private set => this.RaiseAndSetIfChanged(ref _isOrderPaidFor, value);
        }

        #endregion

        #endregion

        #region Private Fields

        private readonly ObservableCollection<Order> _orders;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> PlaceOrderCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearCartCommand { get; }
        public ReactiveCommand<TransportRent, Unit> RemoveFromCartCommand { get; }
        public ReactiveCommand<TransportRent, Unit> SelectTransportRentCommand { get; }
        public ReactiveCommand<NumericUpDownValueChangedEventArgs, Unit> UpdateTotalPriceCommand { get; }

        public ReactiveCommand<Unit, Unit> OpenCartPageCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenOrderPaymentPageCommand { get; }
        public ReactiveCommand<Unit, Unit> CloseOrderPaymentPageCommand { get; }
        public ReactiveCommand<Unit, Unit> PayOrderCommand { get; }
        public ReactiveCommand<Unit, Unit> DownloadReceiptCommand { get; }
        public ReactiveCommand<Unit, Unit> DownloadSummaryStatementCommand { get; }

        #endregion

        public CartViewModel() : this(new ObservableCollection<Order>())
        {
        }

        public CartViewModel(ObservableCollection<Order> orders)
        {
            PaymentMethods = GetSupportedPaymentMethods();
            Cart = new ObservableCollection<TransportRent>();

            Cart.CollectionChanged += (object? s, NotifyCollectionChangedEventArgs e) =>
            {
                CalcTotalPrice();
                CheckCartIsEmpty();
            };

            _orders = orders;

            PlaceOrderCommand = ReactiveCommand.Create(PlaceOrder);
            ClearCartCommand = ReactiveCommand.Create(ClearCart);
            RemoveFromCartCommand = ReactiveCommand.Create<TransportRent>(RemoveFromCart);
            SelectTransportRentCommand = ReactiveCommand.Create<TransportRent>(SelectTransportRent);
            UpdateTotalPriceCommand = ReactiveCommand.Create<NumericUpDownValueChangedEventArgs>(UpdateTotalPrice);

            OpenCartPageCommand = ReactiveCommand.Create(OpenCartPage);
            OpenOrderPaymentPageCommand = ReactiveCommand.Create(OpenOrderPaymentPage);
            CloseOrderPaymentPageCommand = ReactiveCommand.Create(CloseOrderPaymentPage);
            PayOrderCommand = ReactiveCommand.Create(PayOrder);
            DownloadReceiptCommand = ReactiveCommand.Create(DownloadReceipt);
            DownloadSummaryStatementCommand = ReactiveCommand.Create(DownloadSummaryStatement);
        }

        #region Private Methods

        private void PlaceOrder()
        {
            //throw new NotImplementedException();
            OpenOrderPlacingPage();
        }

        private void ClearCart()
        {
            Cart.Clear();
            SelectedTransportRent = null;
        }

        private void RemoveFromCart(TransportRent transportRent)
        {
            if (transportRent == SelectedTransportRent)
                SelectedTransportRent = null;

            _ = Cart.Remove(transportRent);
        }

        private void SelectTransportRent(TransportRent transportRent)
        {
            SelectedTransportRent = transportRent;
        }

        private void UpdateTotalPrice(NumericUpDownValueChangedEventArgs e)
        {
            if (SelectedTransportRent is not null)
                SelectedTransportRent.Days = (int)e.NewValue;

            CalcTotalPrice();
        }

        private void CalcTotalPrice()
        {
            TotalPrice = Cart.Sum(t => t.TotalPrice);
        }

        private void CheckCartIsEmpty()
        {
            IsCartNotEmpty = Cart.Count != 0;
        }

        private void HideAllPages()
        {
            IsCartPageVisible = false;
            IsOrderPlacingPageVisible = false;
            IsOrderPaymentPageVisible = false;
        }

        private void OpenCartPage()
        {
            HideAllPages();
            IsCartPageVisible = true;
        }

        private void OpenOrderPlacingPage()
        {
            HideAllPages();
            IsOrderPlacingPageVisible = true;
        }

        private void OpenOrderPaymentPage()
        {
            HideAllPages();
            IsOrderPaymentPageVisible = true;

            this.RaisePropertyChanged(nameof(Cart));
        }

        private void CloseOrderPaymentPage()
        {
            HideAllPages();

            IsCartPageVisible = true;
            IsOrderPaidFor = false;

            OrdersTabOpening?.Invoke();
        }

        private void PayOrder()
        {
            //throw new NotImplementedException();
            IsOrderPaidFor = true;

            int id = new Random().Next(0, 1000000);
            double price = TotalPrice;
            DateTime date = DateTime.Now;
            var models = Cart.Select(t => t.Transport.Name);

            var order = new Order(id, price, date, models);
            _orders.Add(order);

            ClearCart();
        }

        private void DownloadReceipt()
        {
            throw new NotImplementedException();
        }

        private void DownloadSummaryStatement()
        {
            throw new NotImplementedException();
        }

        private static ObservableCollection<string> GetSupportedPaymentMethods()
        {
            return new ObservableCollection<string>()
            {
                "Онлайн оплата"
            };
        }

        #endregion
    }
}
