using System.Collections.Generic;

namespace RentDesktop.Models.DB
{
#pragma warning disable IDE1006

    internal class DbOrder
    {
        public string id { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string image { get; set; } = string.Empty;
        public decimal total { get; set; } = 0;
        public string orderDate { get; set; } = string.Empty;
    }

#pragma warning restore IDE1006
}
