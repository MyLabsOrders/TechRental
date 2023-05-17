using Avalonia.Media.Imaging;
using System;
using System.IO;

namespace RentDesktop.Infrastructure.Services
{
    internal static class BitmapService
    {
        public static byte[] StringToBytes(string base64string)
        {
            return Convert.FromBase64String(base64string);
        }

        public static string BytesToString(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static byte[] BitmapToBytes(Bitmap bitmap)
        {
            byte[] bytes;

            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms);
                bytes = ms.GetBuffer();
            }

            return bytes;
        }

        public static Bitmap BytesToBitmap(byte[] bytes)
        {
            Bitmap bitmap;

            using (var ms = new MemoryStream(bytes))
            {
                bitmap = new Bitmap(ms);
            }

            return bitmap;
        }


        public static bool TryStringToBytes(string text, out byte[] bytes)
        {
            try
            {
                bytes = StringToBytes(text);
                return true;
            }
            catch
            {
                bytes = Array.Empty<byte>();
                return false;
            }
        }

        public static bool TryBitmapToBytes(Bitmap bitmap, out byte[] bytes)
        {
            try
            {
                bytes = BitmapToBytes(bitmap);
                return true;
            }
            catch
            {
                bytes = Array.Empty<byte>();
                return false;
            }
        }

        public static bool TryBytesToBitmap(byte[] bytes, out Bitmap? bitmap)
        {
            try
            {
                bitmap = BytesToBitmap(bytes);
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
