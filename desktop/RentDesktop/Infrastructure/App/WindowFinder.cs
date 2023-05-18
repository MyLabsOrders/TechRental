using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using RentDesktop.Views;
using System;
using System.Linq;

namespace RentDesktop.Infrastructure.App
{
    internal static class WindowFinder
    {
        public static Window? FindMainWindow()
        {
            return Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime app
                ? null
                : app.MainWindow;
        }

        public static Window? FindUserWindow()
        {
            return FindByType(typeof(UserWindow));
        }

        public static Window? FindAdminWindow()
        {
            return FindByType(typeof(AdminWindow));
        }

        public static Window? Find(Func<Window, bool> predicate)
        {
            return Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime app
                ? null
                : app.Windows.FirstOrDefault(predicate);
        }

        public static Window? FindByName(string name)
        {
            return Find(t => t.Name == name);
        }

        public static Window? FindByType(Type type)
        {
            return Find(t => t.GetType() == type);
        }
    }
}
