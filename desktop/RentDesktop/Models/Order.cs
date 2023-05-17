using RentDesktop.Models.Base;
using System;
using System.Collections.Generic;

namespace RentDesktop.Models
{
    public class Order : ReactiveModel, IOrder
    {
        public Order(string id, double price, DateTime dateOfCreation, IEnumerable<string> models)
        {
            ID = id;
            Price = price;
            DateOfCreation = dateOfCreation;
            Models = new List<string>(models);
        }

        public string ID { get; }
        public double Price { get; }
        public DateTime DateOfCreation { get; }
        public IReadOnlyList<string> Models { get; }

        public string ModelsPresenter => string.Join(", ", Models);
        public string DateOfCreationPresenter => DateOfCreation.ToShortDateString();
    }
}
