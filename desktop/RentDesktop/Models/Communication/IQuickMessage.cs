using Avalonia.Controls;
using MessageBox.Avalonia.Enums;
using System;

namespace RentDesktop.Models.Communication
{
    public interface IQuickMessage
    {
        string Title { get; }
        string Message { get; }
        ButtonEnum Buttons { get; }
        Icon Icon { get; }

        void Show(Window? ownerWindow);
        void Show(Type ownerWindowType);
        void ShowDialog(Window? ownerWindow);
        void ShowDialog(Type ownerWindowType);
    }
}