using Avalonia.Media.Imaging;
using System;

namespace RentDesktop.Models
{
    public class Transport : ITransport
    {
        public Transport(string id, string name, string company, double price, DateTime creationDate, Bitmap? icon = null)
        {
            ID = id;
            Name = name;
            Company = company;
            Price = price;
            CreationDate = creationDate;
            Icon = icon;
        }

        public string ID { get; }
        public string Name { get; }
        public string Company { get; }
        public double Price { get; }
        public DateTime CreationDate { get; }
        public Bitmap? Icon { get; }

        public Transport Self => this;
        public string PricePresenter => $"Цена: {Price}";

        public Transport Copy()
        {
            return new Transport(ID, Name, Company, Price, CreationDate, Icon);
        }
    }
}
