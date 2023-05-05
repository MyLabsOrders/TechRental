using RentDesktop.Models.Base;

namespace RentDesktop.Models
{
    public class TransportRent : ReactiveModel, ITransportRent
    {
        public TransportRent(Transport transport, int days)
        {
            Transport = transport;
            Days = days;
            TotalPrice = CalcTotalPrice();
            Presenter = GetPresenterString();
        }

        public TransportRent Self => this;
        public Transport Transport { get; }

        private int _days = 1;
        public int Days
        {
            get => _days;
            set
            {
                int days = value >= 1 ? value : 1;

                if (RaiseAndSetIfChanged(ref _days, days))
                {
                    TotalPrice = CalcTotalPrice();
                    Presenter = GetPresenterString();
                }
            }
        }

        private int _totalPrice = 0;
        public int TotalPrice
        {
            get => _totalPrice;
            private set => RaiseAndSetIfChanged(ref _totalPrice, value);
        }

        private string _presenter = string.Empty;
        public string Presenter
        {
            get => _presenter;
            private set => RaiseAndSetIfChanged(ref _presenter, value);
        }

        private int CalcTotalPrice()
        {
            return Transport.Price * _days;
        }

        private string GetPresenterString()
        {
            return $"{Transport.Name} за {TotalPrice} рублей (дней аренды: {Days})";
        }
    }
}
