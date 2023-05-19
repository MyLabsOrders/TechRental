using RentDesktop.Models;
using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;
using System;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal class DatabaseGenerationService : IDisposable
    {
        #region Default values

        private readonly UserInfo _defaultAdmin = new()
        {
            Login = "admin",
            Password = "Admin123!"
        };

        private readonly UserInfo[] _defaultUsers = new[]
        {
            new UserInfo()
            {
                Login = "user1",
                Password = "User123$",
                Name = "Иван",
                Surname = "Иванов",
                Patronymic = "Иванович",
                PhoneNumber = "8 (921) 123-4567",
                Gender = UserInfo.MALE_GENDER,
                Position = UserInfo.USER_POSITION,
                Status = UserInfo.ACTIVE_STATUS,
                DateOfBirth = new DateTime(2000, 1, 21),
                Money = 100,
            },
            new UserInfo()
            {
                Login = "user2",
                Password = "User123$",
                Name = "Саша",
                Surname = "Сашов",
                Patronymic = "Иванович",
                PhoneNumber = "8 (863) 376-4567",
                Gender = UserInfo.MALE_GENDER,
                Position = UserInfo.USER_POSITION,
                Status = UserInfo.ACTIVE_STATUS,
                DateOfBirth = new DateTime(2002, 4, 20),
                Money = 1000,
            },
            new UserInfo()
            {
                Login = "user3",
                Password = "User123$",
                Name = "Маша",
                Surname = "Машова",
                Patronymic = "Машович",
                PhoneNumber = "8 (314) 198-4567",
                Gender = UserInfo.FEMALE_GENDER,
                Position = UserInfo.USER_POSITION,
                Status = UserInfo.ACTIVE_STATUS,
                DateOfBirth = new DateTime(2005, 12, 3),
                Money = 5000,
            },
            new UserInfo()
            {
                Login = "user4",
                Password = "User123$",
                Name = "Петр",
                Surname = "Петров",
                Patronymic = "Петрович",
                PhoneNumber = "8 (953) 976-0088",
                Gender = UserInfo.MALE_GENDER,
                Position = UserInfo.ADMIN_POSITION,
                Status = UserInfo.ACTIVE_STATUS,
                DateOfBirth = new DateTime(1990, 10, 10),
                Money = 100000,
            },
            new UserInfo()
            {
                Login = "user5",
                Password = "User123$",
                Name = "Таня",
                Surname = "Танина",
                Patronymic = "Татьянович",
                PhoneNumber = "8 (921) 999-1455",
                Gender = UserInfo.FEMALE_GENDER,
                Position = UserInfo.USER_POSITION,
                Status = UserInfo.ACTIVE_STATUS,
                DateOfBirth = new DateTime(1980, 5, 5),
                Money = 400000,
            }
        };

        private readonly Transport[] _defaultTransports = new[]
        {
            new Transport("", "Lada 7", "", 10000, default),
            new Transport("", "Lada 10", "", 5000, default),
            new Transport("", "Lada 15", "", 7000, default),
            new Transport("", "Niva", "", 25000, default),
            new Transport("", "UAZ", "", 30000, default),
        };

        #endregion

        private bool _isDisposed;
        private readonly string? _previousAuthorizationToken;

        public DatabaseGenerationService()
        {
            _previousAuthorizationToken = DatabaseConnectionService.AuthorizationToken;

            DbLoginResponseContent loginContent = LoginService.EnterSystem(_defaultAdmin.Login, _defaultAdmin.Password, null, true);
            DatabaseConnectionService.AuthorizationToken = loginContent.token;
        }

        ~DatabaseGenerationService()
        {
            if (!_isDisposed)
                Dispose();
        }

        public void Dispose()
        {
            DatabaseConnectionService.AuthorizationToken = _previousAuthorizationToken;
            _isDisposed = true;

            GC.SuppressFinalize(this);
        }

        public void Generate()
        {
            AddUsers();
            AddTransports();
        }

        private void AddUsers()
        {
            foreach (var user in _defaultUsers)
            {
                UserRegisterService.RegisterUser(user);

                if (user.Position != UserInfo.ADMIN_POSITION)
                    UserCashService.AddCash(user, user.Money);
            }
        }

        private void AddTransports()
        {
            foreach (var transport in _defaultTransports)
            {
                ShopService.AddTransport(transport);
            }
        }
    }
}
