using Avalonia.Media.Imaging;

namespace RentDesktop.Models
{
    public class Transport : ITransport
    {
        public Transport(string name, string company, int price, Bitmap? icon = null)
        {
            Name = name;
            Company = company;
            Price = price;
            Icon = icon;
        }

        public string Name { get; }
        public string Company { get; }
        public int Price { get; }
        public Bitmap? Icon { get; }

        public Transport Self => this;
        public string PricePresenter => $"Цена: {Price}";

        public Transport Copy()
        {
            return new Transport(Name, Company, Price, Icon);
        }
    }
}
