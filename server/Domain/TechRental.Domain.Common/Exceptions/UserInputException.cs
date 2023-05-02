﻿namespace TechRental.Domain.Common.Exceptions;

public class UserInputException : DomainException
{
    private UserInputException() : base() { }

    private UserInputException(string message)
        : base(message) { }

    public static UserInputException InvalidPhoneNumberException(string message)
        => new UserInputException(message);

    public static UserInputException NegativeOrderTotalException()
        => new UserInputException();
}