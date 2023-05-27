namespace TechRental.Presentation.Contracts.Users;

public record UpdateUserRequest(
    Guid IdentityId,
    string? FirstName,
    string? MiddleName,
    string? LastName,
    string? PhoneNumber,
    string? UserImage,
    DateOnly? BirthDate,
    string? Gender,
    bool? IsActive);
