namespace RentDesktop.Models.DB
{
#pragma warning disable IDE1006

    internal class DbOrderCreationInfo
    {
        public string name { get; set; } = string.Empty;
        public string image { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public double total { get; set; } = 0;
    }

#pragma warning restore IDE1006
}
