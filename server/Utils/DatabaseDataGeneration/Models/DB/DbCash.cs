namespace RentDesktop.Models.DB
{
#pragma warning disable IDE1006

    internal class DbCash
    {
        public DbCash()
        {
        }

        public DbCash(double total)
        {
            this.total = total;
        }

        public double total { get; set; }
    }

#pragma warning restore IDE1006
}
