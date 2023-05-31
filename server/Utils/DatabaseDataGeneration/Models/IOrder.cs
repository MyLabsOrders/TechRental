namespace RentDesktop.Models
{
    public interface IOrder
    {
        string ID { get; }
        double Price { get; }
        string Status { get; set; }
        string? DateOfCreationStamp { get; set; }
        DateTime DateOfCreation { get; }
        IReadOnlyList<Transport> Models { get; }
    }
}