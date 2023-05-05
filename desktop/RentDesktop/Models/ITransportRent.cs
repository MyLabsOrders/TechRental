namespace RentDesktop.Models
{
    public interface ITransportRent
    {
        Transport Transport { get; }
        int Days { get; set; }
        int TotalPrice { get; }
    }
}