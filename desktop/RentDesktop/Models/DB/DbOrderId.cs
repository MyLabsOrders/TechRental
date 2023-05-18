namespace RentDesktop.Models.DB
{
#pragma warning disable IDE1006

    internal class DbOrderId
    {
        public DbOrderId()
        {
        }

        public DbOrderId(string orderId)
        {
            this.orderId = orderId;
        }

        public string orderId { get; set; } = string.Empty;
    }

#pragma warning restore IDE1006
}
