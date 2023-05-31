using RentDesktop.Models;
using RentDesktop.Models.DB;
using RentDesktop.Models.Informing;

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
                Password = "User321!",
                Name = "Виктор",
                Surname = "Сидоров",
                Patronymic = "Степонович",
                PhoneNumber = "8 (921) 987-1044",
                Gender = UserInfo.MALE_GENDER,
                Position = UserInfo.USER_POSITION,
                Status = UserInfo.ACTIVE_STATUS,
                DateOfBirth = new DateTime(1976, 4, 15),
                Money = 500,
            },
            new UserInfo()
            {
                Login = "user2",
                Password = "User321!",
                Name = "Рома",
                Surname = "Романов",
                Patronymic = "Викторович",
                PhoneNumber = "8 (900) 343-9877",
                Gender = UserInfo.MALE_GENDER,
                Position = UserInfo.USER_POSITION,
                Status = UserInfo.ACTIVE_STATUS,
                DateOfBirth = new DateTime(2008, 10, 25),
                Money = 6000,
            },
            new UserInfo()
            {
                Login = "user3",
                Password = "User321!",
                Name = "Катя",
                Surname = "Прохорова",
                Patronymic = "Евгеньевна",
                PhoneNumber = "8 (953) 654-1199",
                Gender = UserInfo.FEMALE_GENDER,
                Position = UserInfo.USER_POSITION,
                Status = UserInfo.ACTIVE_STATUS,
                DateOfBirth = new DateTime(1999, 3, 5),
                Money = 5000,
            },
            new UserInfo()
            {
                Login = "user4",
                Password = "User321!",
                Name = "Илья",
                Surname = "Ильин",
                Patronymic = "Петрович",
                PhoneNumber = "8 (921) 111-8766",
                Gender = UserInfo.MALE_GENDER,
                Position = UserInfo.USER_POSITION,
                Status = UserInfo.ACTIVE_STATUS,
                DateOfBirth = new DateTime(1980, 10, 20),
                Money = 500000,
            },
            new UserInfo()
            {
                Login = "user5",
                Password = "User321!",
                Name = "Лена",
                Surname = "Шишкина",
                Patronymic = "Олеговна",
                PhoneNumber = "8 (921) 566-9988",
                Gender = UserInfo.FEMALE_GENDER,
                Position = UserInfo.ADMIN_POSITION,
                Status = UserInfo.ACTIVE_STATUS,
                DateOfBirth = new DateTime(1970, 10, 21),
                Money = 505000,
            }
        };

        private readonly Transport[] _defaultTransports = new[]
        {
            new Transport("", "695ST", "Case", 20000, default, null),
            new Transport("", "CDM835", "Lonking", 40000, default, null),
            new Transport("", "Камаз", "КАМАЗ", 17600, default, null),
            new Transport("", "Niewiadow", "Niewiadow", 5000, default, null),
            new Transport("", "Robbins", "Robbins", 8000, default, null),
            new Transport("", "Шерп", "Sherp", 50000, default, null),
            new Transport("", "ST408", "RUNMAX", 13000, default, null),
            new Transport("", "Тактик", "Tactic", 55000, default, null),
            new Transport("", "TE244", "Lovol", 12000, default, null),
            new Transport("", "ZTC250V", "Zoomlion", 44000, default, null),
        };

        private readonly string[] _transportsIcons = new[]
        {
            Convert.ToBase64String(File.ReadAllBytes("Assets//695ST.jpg")),
            Convert.ToBase64String(File.ReadAllBytes("Assets//CDM835.jpg")),
            Convert.ToBase64String(File.ReadAllBytes("Assets//Kamaz.jpg")),
            Convert.ToBase64String(File.ReadAllBytes("Assets//Niewiadow.jpg")),
            Convert.ToBase64String(File.ReadAllBytes("Assets//Robbins.jpg")),
            Convert.ToBase64String(File.ReadAllBytes("Assets//Sherp.jpg")),
            Convert.ToBase64String(File.ReadAllBytes("Assets//ST408.jpg")),
            Convert.ToBase64String(File.ReadAllBytes("Assets//Taktic.jpg")),
            Convert.ToBase64String(File.ReadAllBytes("Assets//TE244.jpg")),
            Convert.ToBase64String(File.ReadAllBytes("Assets//ZTC250V.jpg")),
        };

        #endregion

        private bool _isDisposed;
        private readonly string? _previousAuthorizationToken;

        public DatabaseGenerationService()
        {
            if (_defaultTransports.Length != _transportsIcons.Length)
                throw new ApplicationException("Generator fields do not correspond to each other.");

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
                    UserCashService.AddCash(user, user.Money, true);
            }
        }

        private void AddTransports()
        {
            for (int i = 0; i < _defaultTransports.Length; i++)
            {
                var transport = _defaultTransports[i];
                string icon = _transportsIcons[i];

                ShopService.AddTransport(transport, icon);
            }
        }
    }
}
