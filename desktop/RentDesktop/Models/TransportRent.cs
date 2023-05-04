using RentDesktop.Models.Base;

namespace RentDesktop.Models
{
    public class TransportRent : ReactiveModel, ITransportRent
    {
        public TransportRent(Transport transport, int days)
        {
            Transport = transport;
            Days = days;
        }

        public Transport Transport { get; }

        private int _days = 1;
        public int Days
        {
            get => _days;
            set
            {
                int days = value >= 1 ? value : 1;
                RaiseAndSetIfChanged(ref _days, days);
            }
        }

        public int TotalPrice => Transport.Price * Days;
        public TransportRent Self => this;
    }
}
