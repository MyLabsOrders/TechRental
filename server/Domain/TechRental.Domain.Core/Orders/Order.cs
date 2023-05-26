using TechRental.Domain.Common.Exceptions;
using TechRental.Domain.Core.Abstractions;
using TechRental.Domain.Core.Users;
#pragma warning disable CS8618

namespace TechRental.Domain.Core.Orders;

public class Order
{
    private int? _amount;
    private int? _rentDays;

    protected Order() { }

    public Order(
        Guid id,
        User? user,
        string name,
        Image image,
        OrderStatus status,
        decimal price,
        DateTime? orderDate)
    {
        Id = id;
        User = user;
        UserId = user?.Id;
        Name = name;
        Image = image;
        Status = status;
        Price = price;
        OrderDate = orderDate;
    }

    public Guid Id { get; }
    public virtual User? User { get; set; }
    public Guid? UserId { get; set; }
    public string Name { get; }
    public Image Image { get; }
    public OrderStatus Status { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal Price { get; }
    public int? Amount
    {
        get => _amount;
        set
        {
            if (value < 0)
                throw UserInputException.NegativeOrderAmountException();

            _amount = value;
        }
    }

    public int? Period
    {
        get => _rentDays;
        set
        {
            if (value < 0)
                throw UserInputException.NegativeOrderPeriodException();

            _rentDays = value;
        }
    }

    public decimal TotalPrice => Price * Amount ?? 0 * Period ?? 0;
}
