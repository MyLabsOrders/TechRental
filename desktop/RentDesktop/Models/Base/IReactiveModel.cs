using System.Runtime.CompilerServices;

namespace RentDesktop.Models.Base
{
    internal interface IReactiveModel
    {
        bool RaiseAndSetIfChanged<T>(ref T backingField, T newValue, [CallerMemberName] string? propertyName = null);
    }
}
