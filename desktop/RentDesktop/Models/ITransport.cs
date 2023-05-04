using Avalonia.Media.Imaging;

namespace RentDesktop.Models
{
    internal interface ITransport
    {
        string Name { get; }
        string Company { get; }
        int Price { get; }
        Bitmap? Icon { get; }
    }
}