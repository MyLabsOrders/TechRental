﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using RentDesktop.ViewModels;
using RentDesktop.Views;

namespace RentDesktop.Infrastructure.App
{
    public static class AppInteraction
    {
        public static void CloseCurrentApp()
        {
            var currAppLifetime = Application.Current?.ApplicationLifetime;

            if (currAppLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
                lifetime.Shutdown();
        }

        public static void CloseMainWindow()
        {
            Window? mainWindow = WindowFinder.FindMainWindow();
            mainWindow?.Close();
        }

        public static void CloseUserWindow()
        {
            Window? userWindow = WindowFinder.FindByType(typeof(UserWindow));
            userWindow?.Close();
        }

        public static void HideMainWindow()
        {
            Window? mainWindow = WindowFinder.FindMainWindow();
            var viewModel = mainWindow?.DataContext as MainWindowViewModel;

            viewModel?.HideWindow();
        }

        public static void ShowMainWindow()
        {
            Window? mainWindow = WindowFinder.FindMainWindow();
            var viewModel = mainWindow?.DataContext as MainWindowViewModel;

            viewModel?.ShowWindow();
        }
    }
}
