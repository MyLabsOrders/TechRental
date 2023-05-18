using Avalonia.Media.Imaging;

namespace RentDesktop.Models
{
    public class Transport : ITransport
    {
        public Transport(string id, string name, string company, int price, Bitmap? icon = null)
        {
            ID = id;
            Name = name;
            Company = company;
            Price = price;
            Icon = icon;
        }

        public string ID { get; }
        public string Name { get; }
        public string Company { get; }
        public int Price { get; }
        public Bitmap? Icon { get; }

        public Transport Self => this;
        public string PricePresenter => $"Цена: {Price}";

        public Transport Copy()
        {
            return new Transport(ID, Name, Company, Price, Icon);
        }
    }
}
