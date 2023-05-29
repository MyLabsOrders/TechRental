namespace RentDesktop.Models.DB
{
#pragma warning disable IDE1006

    internal class DbOrderProduct
    {
        public DbOrderProduct()
        {
        }

        public DbOrderProduct(string orderId, int count, int days)
        {
            this.orderId = orderId;
            this.count = count;
            this.days = days;
        }

        public string orderId { get; set; } = string.Empty;
        public int count { get; set; } = 0;
        public int days { get; set; } = 0;
    }

#pragma warning restore IDE1006
}
