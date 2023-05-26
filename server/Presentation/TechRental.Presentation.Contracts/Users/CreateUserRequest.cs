namespace TechRental.Presentation.Contracts.Users;

public record CreateUserRequest(
    string FirstName,
    string MiddleName,
    string LastName,
    string PhoneNumber,
    string? UserImage,
    DateTime BirthDate,
    string Gender);
