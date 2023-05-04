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
        #region Properties

        public ObservableCollection<TransportRent> Cart { get; }

        private TransportRent? _selectedTransportRent = null;
        public TransportRent? SelectedTransportRent
        {
            get => _selectedTransportRent;
            set
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
            set => this.RaiseAndSetIfChanged(ref _selectedTransportName, value);
        }

        private string _selectedTransportPrice = string.Empty;
        public string SelectedTransportPrice
        {
            get => $"Цена: {_selectedTransportPrice}";
            set => this.RaiseAndSetIfChanged(ref _selectedTransportPrice, value);
        }

        private bool _isTransportRentSelected = false;
        public bool IsTransportRentSelected
        {
            get => _isTransportRentSelected;
            set => this.RaiseAndSetIfChanged(ref _isTransportRentSelected, value);
        }

        private double _totalPrice = 0;
        public double TotalPrice
        {
            get => _totalPrice;
            set => this.RaiseAndSetIfChanged(ref _totalPrice, value);
        }

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> PlaceOrderCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearCartCommand { get; }
        public ReactiveCommand<TransportRent, Unit> RemoveFromCartCommand { get; }
        public ReactiveCommand<TransportRent, Unit> SelectTransportRentCommand { get; }
        public ReactiveCommand<NumericUpDownValueChangedEventArgs, Unit> UpdateTotalPriceCommand { get; }

        #endregion

        public CartViewModel()
        {
            Cart = new ObservableCollection<TransportRent>();
            Cart.CollectionChanged += (object? s, NotifyCollectionChangedEventArgs e) => CalcTotalPrice();

            PlaceOrderCommand = ReactiveCommand.Create(PlaceOrder);
            ClearCartCommand = ReactiveCommand.Create(ClearCart);
            RemoveFromCartCommand = ReactiveCommand.Create<TransportRent>(RemoveFromCart);
            SelectTransportRentCommand = ReactiveCommand.Create<TransportRent>(SelectTransportRent);
            UpdateTotalPriceCommand = ReactiveCommand.Create<NumericUpDownValueChangedEventArgs>(UpdateTotalPrice);
        }

        #region Private Methods

        private void PlaceOrder()
        {
            throw new NotImplementedException();
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

        #endregion
    }
}
