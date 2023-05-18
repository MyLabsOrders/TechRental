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
                Gender = UserInfo.MALE_GENDER, // not in backend -> TODO
                Position = position,
                Status = UserInfo.ACTIVE_STATUS, // not in backend -> TODO
                Money = user.money,
                Icon = BitmapService.StringToBytes(user.image),
                DateOfBirth = DateTimeService.StringToDateTime(user.birthDate),
                Orders = new ObservableCollection<Order>(ConvertOrders(user.orders ?? Array.Empty<DbOrder>()))
            };
        }

        public static List<IUserInfo> ConvertUsers(DbUsers databaseUsers)
        {
            return databaseUsers.users!
                .Select(t => ConvertUser(t, UserInfo.USER_POSITION) as IUserInfo) // position -> TODO
                .ToList();
        }

        public static IEnumerable<Order> ConvertOrders(IEnumerable<DbOrder> databaseOrders)
        {
            return databaseOrders.Select(t => new Order(
                id: t.id,
                price: t.total,
                status: t.status,
                dateOfCreation: DateTimeService.StringToDateTime(t.orderDate),
                models: t.name.Split(Order.ORDERS_MODELS_DELIMITER)
            ));
        }
    }
}
