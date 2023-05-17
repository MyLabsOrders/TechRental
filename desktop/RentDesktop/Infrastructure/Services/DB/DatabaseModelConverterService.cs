using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System;
using System.Collections.Generic;
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
                Gender = "Мужской", // TODO
                Position = UserInfo.USER_POSITION, // TODO
                Status = UserInfo.ACTIVE_STATUS, // TODO
                Icon = BitmapService.ConvertStringToBytes(t.image),
                DateOfBirth = DateTime.Parse(t.birthDate)
            }).ToList();
        }
    }
}
