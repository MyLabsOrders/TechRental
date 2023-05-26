using TechRental.Domain.Common.Exceptions;
using TechRental.Domain.Core.Abstractions;
using TechRental.Domain.Core.Orders;
using TechRental.Domain.Core.ValueObject;
#pragma warning disable CS8618

namespace TechRental.Domain.Core.Users;

public class User
{
    private readonly List<Order> _orders;
    private decimal _money;

    protected User()
    {
        _orders = new List<Order>();
    }

    public User(
        Guid id,
        string firstName,
        string middleName,
        string lastName,
        Image image,
        DateTime birthDate,
        PhoneNumber phoneNumber,
        Gender gender)
    {
        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(middleName);
        ArgumentNullException.ThrowIfNull(lastName);
        ArgumentNullException.ThrowIfNull(image);
        ArgumentNullException.ThrowIfNull(phoneNumber);

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Image = image;
        MiddleName = middleName;
        BirthDate = birthDate;
        PhoneNumber = phoneNumber;
        Gender = gender;
        IsActive = true;

        _orders = new List<Order>();
        Money = 0;
    }

    public Guid Id { get; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public Image Image { get; set; }
    public DateTime BirthDate { get; set; }
    public PhoneNumber PhoneNumber { get; set; }
    public Gender Gender { get; set; }
    public bool IsActive { get; set; }
    public virtual IEnumerable<Order> Orders => _orders;
    public decimal Money
    {
        get => _money;
        set
        {
            if (value < 0)
                throw UserInputException.NegativeUserBalanceException("User has not enough money");

            _money = value;
        }
    }

    public override string ToString()
    {
        return $"{LastName} {FirstName} {MiddleName}";
    }

    public Order AddOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        _orders.Add(order);
        order.User = this;

        return order;
    }

    public void RemoveOrder(Order order)
    {
        order.User = null;
        _orders.Remove(order);
    }
}
