using Avalonia.Media.Imaging;
using System;
using System.IO;

namespace RentDesktop.Infrastructure.Services
{
    internal static class BitmapService
    {
        public static byte[] ConvertBitmapToBytes(Bitmap bitmap)
        {
            byte[] bytes;

            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms);
                bytes = ms.GetBuffer();
            }

            return bytes;
        }

        public static Bitmap ConvertBytesToBitmap(byte[] bytes)
        {
            Bitmap bitmap;

            using (var ms = new MemoryStream(bytes))
            {
                bitmap = new Bitmap(ms);
            }

            return bitmap;
        }

        public static bool TryConvertBitmapToBytes(Bitmap bitmap, out byte[] bytes)
        {
            try
            {
                bytes = ConvertBitmapToBytes(bitmap);
                return true;
            }
            catch
            {
                bytes = Array.Empty<byte>();
                return false;
            }
        }

        public static bool TryConvertBytesToBitmap(byte[] bytes, out Bitmap? bitmap)
        {
            try
            {
                bitmap = ConvertBytesToBitmap(bytes);
                return true;
            }
            catch
            {
                bitmap = null;
                return false;
            }
        }
    }
}
