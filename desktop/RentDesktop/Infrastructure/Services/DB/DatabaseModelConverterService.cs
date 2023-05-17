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
        public static UserInfo ConvertUser(DbUser user)
        {
            return new UserInfo()
            {
                ID = user.id,
                Login = "", // TODO,
                Password = "", // TODO
                Name = user.firstName,
                Surname = user.middleName,
                Patronymic = user.lastName,
                PhoneNumber = user.number,
                Gender = UserInfo.MALE_GENDER, // TODO
                Position = UserInfo.USER_POSITION, // TODO
                Status = UserInfo.ACTIVE_STATUS, // TODO
                Icon = BitmapService.ConvertStringToBytes(user.image),
                DateOfBirth = DateTime.Parse(user.birthDate),
                Orders = new ObservableCollection<Order>(ConvertOrders(user.orders ?? Array.Empty<DbOrder>()))
            };
        }
        
        public static List<IUserInfo> ConvertUsers(DbUsers databaseUsers)
        {
            return databaseUsers.users!
                .Select(t => ConvertUser(t) as IUserInfo)
                .ToList();
        }

        public static IEnumerable<Order> ConvertOrders(IEnumerable<DbOrder> databaseOrders)
        {
            return databaseOrders.Select(t => new Order(
                id: t.id,
                price: t.total,
                dateOfCreation: DateTime.Parse(t.orderDate),
                models: t.name.Split(", ")
            ));
        }
    }
}
