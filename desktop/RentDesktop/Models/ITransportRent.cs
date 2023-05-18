namespace RentDesktop.Models
{
    public interface ITransportRent
    {
        Transport Transport { get; }
        int Days { get; set; }
        double TotalPrice { get; }
    }
}