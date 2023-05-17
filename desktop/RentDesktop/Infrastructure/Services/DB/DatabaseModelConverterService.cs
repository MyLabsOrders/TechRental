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
        public static List<IUserInfo> ConvertUsers(DbUsers databaseUsers)
        {
            return databaseUsers.users!.Select(t => (IUserInfo)new UserInfo()
            {
                ID = t.id,
                Login = "", // TODO,
                Password = "", // TODO
                Name = t.firstName,
                Surname = t.middleName,
                Patronymic = t.lastName,
                PhoneNumber = t.number,
                Gender = UserInfo.MALE_GENDER, // TODO
                Position = UserInfo.USER_POSITION, // TODO
                Status = UserInfo.ACTIVE_STATUS, // TODO
                Icon = BitmapService.ConvertStringToBytes(t.image),
                DateOfBirth = DateTime.Parse(t.birthDate),
                Orders = new ObservableCollection<Order>(ConvertOrders(t.orders ?? Array.Empty<DbOrder>()))
            }).ToList();
        }

        public static IEnumerable<Order> ConvertOrders(IEnumerable<DbOrder> databaseOrders)
        {
            return databaseOrders.Select(t => new Order(
                id: t.id,
                price: t.total,
                dateOfCreation: DateTime.Parse(t.orderDate),
                models: Array.Empty<string>() // TODO
            ));
        }
    }
}
