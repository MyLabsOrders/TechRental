using TechRental.Application.Dto.Orders;

namespace TechRental.Application.Dto.Users;

public record UserDto(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    string Image,
    DateTime BirthDate,
    string Number,
    string Gender,
    bool IsActive,
    decimal Money,
    IEnumerable<UserOrderDto> Orders);
