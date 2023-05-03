namespace RentDesktop.Models
{
    public class Transport : ITransport
    {
        public string Name { get; }
        public string Company { get; }
        public int Price { get; }

        public Transport Self => this;

        public string NamePresenter => $"Название: {Name}";
        public string CompanyPresenter => $"Компания: {Company}";
        public string PricePresenter => $"Цена: {Price}";

        public Transport(string name, string company, int price)
        {
            Name = name;
            Company = company;
            Price = price;
        }
    }
}
