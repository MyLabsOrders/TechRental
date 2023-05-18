using System;
using System.Collections.Generic;

namespace RentDesktop.Models
{
    public interface IOrder
    {
        string ID { get; }
        double Price { get; }
        string Status { get; set; }
        DateTime DateOfCreation { get; }
        IReadOnlyList<string> Models { get; }
    }
}