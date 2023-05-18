using System.Collections.Generic;

namespace RentDesktop.Models.DB
{
#pragma warning disable IDE1006

    internal class DbUser
    {
        public string id { get; set; } = string.Empty;
        public string firstName { get; set; } = string.Empty;
        public string middleName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string image { get; set; } = string.Empty;
        public string birthDate { get; set; } = string.Empty;
        public string number { get; set; } = string.Empty;
        public double money { get; set; } = 0;
        public IEnumerable<DbOrder>? orders { get; set; } = null;
    }

#pragma warning restore IDE1006
}
