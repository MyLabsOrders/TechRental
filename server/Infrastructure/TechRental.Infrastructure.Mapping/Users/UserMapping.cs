using TechRental.Application.Dto.Users;
using TechRental.Domain.Core.Users;
using TechRental.Infrastructure.Mapping.Orders;

namespace TechRental.Infrastructure.Mapping.Users;

public static class UserMapping
{
    public static IEnumerable<UserDto> ToDto(this IEnumerable<User> users)
    {
        return users.Select(x => x.ToDto()!);
    }

    public static UserDto? ToDto(this User? user)
    {
        if (user is null)
            return null;

        return new UserDto(
            user.Id,
            user.FirstName,
            user.MiddleName,
            user.LastName,
            user.Image.Value,
            user.BirthDate,
            user.PhoneNumber.Value,
            user.Gender.ToString(),
            user.IsActive,
            user.Money,
            user.Orders.ToDto());
    }
}
