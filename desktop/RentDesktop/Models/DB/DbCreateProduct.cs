namespace RentDesktop.Models.DB
{
#pragma warning disable IDE1006

    internal class DbCreateProduct
    {
        public string name { get; set; } = string.Empty;
        public string company { get; set; } = string.Empty;
        public string orderImage { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public double price { get; set; } = 0;
    }

#pragma warning restore IDE1006
}
