using Avalonia.Media.Imaging;
using RentDesktop.Models;
using System.Collections.Generic;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class ShopService
    {
        public static bool GetTransports(out IEnumerable<Transport> transports)
        {
            //throw new NotImplementedException();

            transports = new[]
            {
                new Transport("1", "Lada 7", "Company 1", 10000, new Bitmap(@"D:\Testing\TechRental\lada7.jpg")),
                new Transport("2", "Lada 10", "Company 1", 5000, new Bitmap(@"D:\Testing\TechRental\lada10.jpg")),
                new Transport("3", "Lada 15", "Company 1", 7000, new Bitmap(@"D:\Testing\TechRental\lada15.jpg")),
                new Transport("4", "Niva", "Company 2", 25000, new Bitmap(@"D:\Testing\TechRental\niva.jpg")),
                new Transport("5", "UAZ", "Company 3", 30000, new Bitmap(@"D:\Testing\TechRental\uaz.jpg")),
            };

            return true;
        }
    }
}
