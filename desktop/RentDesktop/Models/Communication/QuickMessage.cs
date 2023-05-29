using Avalonia.Controls;
using MessageBox.Avalonia;
using MessageBox.Avalonia.BaseWindows.Base;
using MessageBox.Avalonia.Enums;
using RentDesktop.Infrastructure.App;
using System;

namespace RentDesktop.Models.Communication
{
    public class QuickMessage : IQuickMessage
    {
        public QuickMessage(string message, string title = "", ButtonEnum buttons = ButtonEnum.Ok, Icon icon = Icon.None)
        {
            Title = title;
            Message = message;
            Buttons = buttons;
            Icon = icon;
        }

        public string Title { get; }
        public string Message { get; }
        public ButtonEnum Buttons { get; }
        public Icon Icon { get; }

        public void Show(Window? ownerWindow)
        {
            if (ownerWindow is null)
                return;

            _ = GetMessageBoxWindow().Show(ownerWindow);
        }

        public void Show(Type ownerWindowType)
        {
            Window? window = WindowFinder.FindByType(ownerWindowType);
            Show(window);
        }

        public void ShowDialog(Window? ownerWindow)
        {
            if (ownerWindow is null)
                return;

            _ = GetMessageBoxWindow().ShowDialog(ownerWindow);
        }

        public void ShowDialog(Type ownerWindowType)
        {
            Window? window = WindowFinder.FindByType(ownerWindowType);
            ShowDialog(window);
        }

        public static QuickMessage Error(string message)
        {
            return new QuickMessage(message, "Ошибка", ButtonEnum.Ok, Icon.Error);
        }

        public static QuickMessage Info(string message)
        {
            return new QuickMessage(message, "Информация", ButtonEnum.Ok, Icon.Info);
        }

        private IMsBoxWindow<ButtonResult> GetMessageBoxWindow()
        {
            return MessageBoxManager.GetMessageBoxStandardWindow(Title, Message, Buttons, Icon);
        }
    }
}
