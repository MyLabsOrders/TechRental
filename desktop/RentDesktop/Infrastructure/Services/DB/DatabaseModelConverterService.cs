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

        public static List<IUserInfo> ConvertUsers(DbUsers databaseUsers)
        {
            return databaseUsers.users!
                .Select(t => ConvertUser(t, UserInfo.USER_POSITION) as IUserInfo) // TODO: position
                .ToList();
        }

        public static IEnumerable<Order> ConvertOrders(IEnumerable<DbOrder> databaseOrders)
        {
            return databaseOrders.Select(t => new Order(
                id: t.id,
                price: t.total,
                status: t.status,
                dateOfCreation: DateTimeService.StringToDateTime(t.orderDate),
                models: new[] { ConvertDbOrderToTransport(t) }
            ));
        }

        public static Transport ConvertDbOrderToTransport(DbOrder order)
        {
            byte[] imageBytes = BitmapService.StringToBytes(order.image);

            return new Transport(
                order.id,
                order.name,
                "MyCompany", // Future work: add company to order model
                order.total,
                DateTimeService.StringToDateTime(order.orderDate),
                BitmapService.BytesToBitmap(imageBytes)
            );
        }
    }
}
