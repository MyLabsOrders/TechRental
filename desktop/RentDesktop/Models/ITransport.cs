using Avalonia.Media.Imaging;

namespace RentDesktop.Models
{
    public interface ITransport
    {
        string ID { get; }
        string Name { get; }
        string Company { get; }
        int Price { get; }
        Bitmap? Icon { get; }
    }
}