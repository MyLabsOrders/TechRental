namespace TechRental.Domain.Common.Exceptions;

public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string message) : base(message) { }

    public static EntityNotFoundException For<T>(Guid id)
        => new EntityNotFoundException($"Entity of type {typeof(T).Name} with id \'{id}\' was not found");

    public static EntityNotFoundException For<T>(string attribute)
        => new EntityNotFoundException($"Entity of type {typeof(T).Name} with attribute \'{attribute}\' was not found");
}
