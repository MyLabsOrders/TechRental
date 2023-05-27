using RentDesktop.Models;
using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class DatabaseModelConverterService
    {
        public static UserInfo ConvertUser(DbUser user, string position)
        {
            return new UserInfo()
            {
                ID = user.id,
                Login = string.Empty,
                Password = string.Empty,
                Name = user.firstName,
                Surname = user.middleName,
                Patronymic = user.lastName,
                PhoneNumber = user.number,
                Gender = UserInfo.MALE_GENDER, // Future work: add gender to user model
                Position = position,
                Status = UserInfo.ACTIVE_STATUS, // // Future work: add status to user model
                Money = user.money,
                Icon = BitmapService.StringToBytes(user.image),
                DateOfBirth = DateTimeService.StringToDateTime(user.birthDate),
                Orders = new ObservableCollection<Order>(ConvertOrders(user.orders ?? Array.Empty<DbOrder>()))
            };
        }

        public static List<IUserInfo> ConvertUsers(DbUsers databaseUsers, IReadOnlyList<string> positions)
        {
            if (databaseUsers.users is null)
                return new List<IUserInfo>();

            if (databaseUsers.users.Count() != positions.Count)
                throw new InvalidOperationException("The number of users does not match the number of their positions.");

            UserInfo UserConverter(DbUser user, int index) => ConvertUser(user, positions[index]);

            return databaseUsers.users!
                .Select(UserConverter)
                .Select(t => t as IUserInfo)
                .ToList();
        }

        public static IEnumerable<Order> ConvertOrders(IEnumerable<DbOrder> databaseOrders)
        {
            return databaseOrders.Select(t => new Order(
                id: t.id,
                price: t.total,
                status: t.status,
                dateOfCreation: t.orderDate is null ? default : DateTimeService.StringToDateTime(t.orderDate),
                models: new[] { ConvertDbOrderToTransport(t) }
            ));
        }

        public static Transport ConvertDbOrderToTransport(DbOrder order)
        {
            byte[] imageBytes = BitmapService.StringToBytes(order.image);

            var transportIcon = imageBytes.Length > 0
                ? BitmapService.BytesToBitmap(imageBytes)
                : null;

            DateTime creationDate = order.orderDate is null
                ? default
                : DateTimeService.StringToDateTime(order.orderDate);

            return new Transport(
                order.id,
                order.name,
                order.company,
                order.total,
                creationDate,
                transportIcon
            );
        }
    }
}
