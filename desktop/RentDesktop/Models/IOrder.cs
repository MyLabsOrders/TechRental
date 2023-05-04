using System;
using System.Collections.Generic;

namespace RentDesktop.Models
{
    internal interface IOrder
    {
        int ID { get; }
        double Price { get; }
        DateTime DateOfCreation { get; }
        IReadOnlyList<string> Models { get; }
    }
}