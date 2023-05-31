namespace RentDesktop.Models.DB
{
#pragma warning disable IDE1006

    internal class DbOrder
    {
        public string id { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string company { get; set; } = string.Empty;
        public string image { get; set; } = string.Empty;
        public double total { get; set; } = 0;
        public string? orderDate { get; set; } = null;
        public int? count { get; set; } = null;
        public int? days { get; set; } = null;
    }

#pragma warning restore IDE1006
}
