using RentDesktop.Models.Base;
using System;
using System.Collections.Generic;

namespace RentDesktop.Models
{
    public class Order : ReactiveModel, IOrder
    {
        public const string AVAILABLE_STATUS = "Available";
        public const string RENTED_STATUS = "Rented";

        public const string ACTIVE_STATUS = "Активен";
        public const string COMPLETED_STATUS = "Выполнен";
        public const string EXPIRED_STATUS = "Истек";
        public const string CANCELLED_STATUS = "Отменен";

        public const string ORDERS_MODELS_DELIMITER = ", ";

        public Order(string id, double price, string status, DateTime dateOfCreation, IEnumerable<Transport> models)
        {
            ID = id;
            Price = price;
            Status = status;
            DateOfCreation = dateOfCreation;
            Models = new List<Transport>(models);
        }

        public string ID { get; }
        public double Price { get; }
        public string Status { get; set; }
        public DateTime DateOfCreation { get; }
        public IReadOnlyList<Transport> Models { get; }

        public string ModelsPresenter => string.Join(ORDERS_MODELS_DELIMITER, Models);
        public string DateOfCreationPresenter => DateOfCreation.ToShortDateString();
    }
}
