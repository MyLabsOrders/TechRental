using RentDesktop.Models;
using System.Collections.Generic;

namespace RentDesktop.Infrastructure.Services.DB
{
    public static class ShopService
    {
        public static bool GetTransports(out IEnumerable<Transport> transports)
        {
            //throw new NotImplementedException();

            transports = new[]
            {
                new Transport("Opel", "Company 1", 100000),
                new Transport("Mazda", "Company 2", 5900000),
                new Transport("BMW", "Company 3", 30000),
                new Transport("Mercedes", "Company 4", 4500000)
            };

            return true;
        }
    }
}
