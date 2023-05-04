namespace RentDesktop.Models
{
    internal interface ITransportRent
    {
        Transport Transport { get; }
        int Days { get; set; }
        int TotalPrice { get; }
    }
}