﻿using Avalonia.Controls;
using ReactiveUI;
using RentDesktop.Infrastructure.App;
using RentDesktop.Infrastructure.Exceptions;
using RentDesktop.Infrastructure.Services.DB;
using RentDesktop.Models;
using RentDesktop.Models.Communication;
using RentDesktop.Models.Informing;
using RentDesktop.ViewModels.Base;
using RentDesktop.Views;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace RentDesktop.ViewModels.Pages.UserWindowPages
{
    public class CartViewModel : ViewModelBase
    {
        public CartViewModel() : this(new UserInfo(), new ObservableCollection<Order>())
        {
        }

        public CartViewModel(IUserInfo userInfo, ObservableCollection<Order> orders)
        {
            PaymentMethods = GetSupportedPaymentMethods();
            Cart = new ObservableCollection<TransportRent>();

            Cart.CollectionChanged += (s, e) =>
            {
                CalcTotalPrice();
                CheckCartIsEmpty();
            };

            _userInfo = userInfo;
            _orders = orders;

            UpdateUserInfo();

            OpenOrderPlacingPageCommand = ReactiveCommand.Create(OpenOrderPlacingPage);
            ClearCartCommand = ReactiveCommand.Create(ClearCart);
            RemoveFromCartCommand = ReactiveCommand.Create<TransportRent>(RemoveFromCart);
            SelectTransportRentCommand = ReactiveCommand.Create<TransportRent>(SelectTransportRent);
            UpdateTotalPriceCommand = ReactiveCommand.Create<NumericUpDownValueChangedEventArgs>(UpdateTotalPrice);
            UpdateInfoOnRelatedProductsCommand = ReactiveCommand.Create<NumericUpDownValueChangedEventArgs>(UpdateInfoOnRelatedProducts);

            OpenCartPageCommand = ReactiveCommand.Create(OpenCartPage);
            OpenOrderPaymentPageCommand = ReactiveCommand.Create(OpenOrderPaymentPage);
            CloseOrderPaymentPageCommand = ReactiveCommand.Create(CloseOrderPaymentPage);
            PayOrderCommand = ReactiveCommand.Create(PayOrder);
            DownloadReceiptCommand = ReactiveCommand.Create(DownloadReceipt);
            DownloadSummaryStatementCommand = ReactiveCommand.Create(DownloadSummaryStatement);
        }

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

        private readonly IUserInfo _userInfo;
        private readonly ObservableCollection<Order> _orders;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> OpenOrderPlacingPageCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearCartCommand { get; }
        public ReactiveCommand<TransportRent, Unit> RemoveFromCartCommand { get; }
        public ReactiveCommand<TransportRent, Unit> SelectTransportRentCommand { get; }
        public ReactiveCommand<NumericUpDownValueChangedEventArgs, Unit> UpdateTotalPriceCommand { get; }
        public ReactiveCommand<NumericUpDownValueChangedEventArgs, Unit> UpdateInfoOnRelatedProductsCommand { get; }

        public ReactiveCommand<Unit, Unit> OpenCartPageCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenOrderPaymentPageCommand { get; }
        public ReactiveCommand<Unit, Unit> CloseOrderPaymentPageCommand { get; }
        public ReactiveCommand<Unit, Unit> PayOrderCommand { get; }
        public ReactiveCommand<Unit, Unit> DownloadReceiptCommand { get; }
        public ReactiveCommand<Unit, Unit> DownloadSummaryStatementCommand { get; }

        #endregion

        #region Public Methods

        public void UpdateUserInfo()
        {
            UserName = $"{_userInfo.Name} {_userInfo.Surname} {_userInfo.Patronymic}";
            UserPhoneNumber = _userInfo.PhoneNumber;
        }

        public void ResetUserPaymentSteps()
        {
            OpenCartPage();
            IsOrderPaidFor = false;
        }

        #endregion

        #region Private Methods

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

        private void UpdateInfoOnRelatedProducts(NumericUpDownValueChangedEventArgs e)
        {
            if (SelectedTransportRent is null)
                return;

            var relatedProducts = Cart.Where(t => t.Transport.ID == SelectedTransportRent.Transport.ID);

            foreach (var relatedProduct in relatedProducts)
            {
                relatedProduct.Days = (int)e.NewValue;
            }

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
        }

        private void CloseOrderPaymentPage()
        {
            if (IsOrderPaidFor)
                OrdersTabOpening?.Invoke();

            ResetUserPaymentSteps();
        }

        private void PayOrder()
        {
            if (!UserCashService.CanPayOrder(Cart, _userInfo))
            {
                QuickMessage.Error("У вас не хватает средств для оплаты.").ShowDialog(typeof(UserWindow));
                return;
            }

            Order order;

            try
            {
                order = OrdersService.CreateOrder(Cart, _userInfo);
            }
            catch (Exception ex)
            {
                string message = "Не удалось оформить заказ.";
#if DEBUG
                message += $" Причина: {ex.Message}";
#endif
                QuickMessage.Error(message).ShowDialog(typeof(UserWindow));
                return;
            }

            _orders.Add(order);
            IsOrderPaidFor = true;

            ClearCart();
        }

        private async void DownloadReceipt()
        {
            try
            {
                using MemoryStream cheque = FileDownloadService.DownloadCheque(_orders[^1]);
                await SavePdfFileAsync(cheque);

                QuickMessage.Info("Чек успешно загружен.").ShowDialog(typeof(UserWindow));
            }
            catch (Exception ex)
            {
                string message = "Не удалось загрузить чек.";
#if DEBUG
                message += $" Причина: {ex.Message}";
#endif
                QuickMessage.Error(message).ShowDialog(typeof(UserWindow));
            }
        }

        private async void DownloadSummaryStatement()
        {
            try
            {
                using MemoryStream invoice = FileDownloadService.DownloadInvoice(_orders[^1]);
                await SavePdfFileAsync(invoice);

                QuickMessage.Info("Ведомость успешно загружена.").ShowDialog(typeof(UserWindow));
            }
            catch (Exception ex)
            {
                string message = "Не удалось загрузить ведомость.";
#if DEBUG
                message += $" Причина: {ex.Message}";
#endif
                QuickMessage.Error(message).ShowDialog(typeof(UserWindow));
            }
        }

        private static async Task SavePdfFileAsync(MemoryStream pdfStream)
        {
            if (WindowFinder.FindUserWindow() is not Window userWindow)
                throw new PathNotSpecifiedException();

            SaveFileDialog dialog = DialogProvider.GetSavePdfFileDialog();

            string? path = await dialog.ShowAsync(userWindow);

            if (string.IsNullOrEmpty(path))
                throw new PathNotSpecifiedException();

            using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write);
            pdfStream.WriteTo(fileStream);
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
