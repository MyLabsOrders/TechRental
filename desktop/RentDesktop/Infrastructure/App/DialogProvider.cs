using Avalonia.Controls;
using System.Collections.Generic;

namespace RentDesktop.Infrastructure.App
{
    internal static class DialogProvider
    {
        public static OpenFileDialog GetOpenImageDialog()
        {
            return new OpenFileDialog()
            {
                AllowMultiple = false,
                Title = "Выберите изображение",

                Filters = new List<FileDialogFilter>()
                {
                    new FileDialogFilter()
                    {
                        Name = "Image files",
                        Extensions = new List<string>() { "bmp", "jpg", "gif", "png" }
                    },
                    new FileDialogFilter()
                    {
                        Name = "BMP files",
                        Extensions = new List<string>() { "bmp" }
                    },
                    new FileDialogFilter()
                    {
                        Name = "JPG files",
                        Extensions = new List<string>() { "jpg" }
                    },
                    new FileDialogFilter()
                    {
                        Name = "GIF files",
                        Extensions = new List<string>() { "gif" }
                    },
                    new FileDialogFilter()
                    {
                        Name = "PNG files",
                        Extensions = new List<string>() { "png" }
                    },
                    new FileDialogFilter()
                    {
                        Name = "All files",
                        Extensions = new List<string>() { "*" }
                    }
                }
            };
        }
    }
}
