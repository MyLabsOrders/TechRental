using ReactiveUI;
using RentDesktop.Infrastructure.Extensions;
using RentDesktop.Infrastructure.Services.DB;
using RentDesktop.Models.Informing;
using RentDesktop.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace RentDesktop.ViewModels.Pages.AdminWindowPages
{
    public class AllUsersViewModel : ViewModelBase
    {
        public AllUsersViewModel()
        {
            _databaseUsers = Array.Empty<IUserInfo>();
            Users = new ObservableCollection<IUserInfo>();

            RefreshUsers();

            Positions = GetPositions();
            Statuses = GetStatuses();
            Genders = GetGenders();

            FindUsersCommand = ReactiveCommand.Create(FindUsers);
            ResetSearchFieldsCommand = ReactiveCommand.Create(ResetSearchFields);
            RefreshUsersCommand = ReactiveCommand.Create(RefreshUsers);
        }

        #region Events

        public delegate void SelectedUserChangedHandler(IUserInfo? userInfo);
        public event SelectedUserChangedHandler? SelectedUserChanged;

        #endregion

        #region Properties

        public ObservableCollection<IUserInfo> Users { get; }
        public ObservableCollection<string> Positions { get; }
        public ObservableCollection<string> Statuses { get; }
        public ObservableCollection<string> Genders { get; }

        private IUserInfo? _selectedUser = null;
        public IUserInfo? SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (value is null)
                    return;

                bool isChanged = _selectedUser != value;

                this.RaiseAndSetIfChanged(ref _selectedUser, value);
                IsUserSelected = value is not null;

                if (isChanged)
                    SelectedUserChanged?.Invoke(value);
            }
        }

        private bool _isUserSelected = false;
        public bool IsUserSelected
        {
            get => _isUserSelected;
            private set => this.RaiseAndSetIfChanged(ref _isUserSelected, value);
        }

        private int _selectedPositionIndex = 0;
        public int SelectedPositionIndex
        {
            get => _selectedPositionIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedPositionIndex, value);
        }

        private int _selectedStatusIndex = 0;
        public int SelectedStatusIndex
        {
            get => _selectedStatusIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedStatusIndex, value);
        }

        private int _selectedGenderIndex = 0;
        public int SelectedGenderIndex
        {
            get => _selectedGenderIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedGenderIndex, value);
        }

        private string _searchQuery = string.Empty;
        public string SearchQuery
        {
            get => _searchQuery;
            set => this.RaiseAndSetIfChanged(ref _searchQuery, value);
        }

        #endregion

        #region Private Fields

        #region Constants

        private const string NOT_SPECIFIED = "Не указано";

        #endregion

        private IEnumerable<IUserInfo> _databaseUsers;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> FindUsersCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetSearchFieldsCommand { get; }
        public ReactiveCommand<Unit, Unit> RefreshUsersCommand { get; }

        #endregion

        #region Private Methods

        private void FindUsers()
        {
            IEnumerable<IUserInfo> foundUsers = _databaseUsers;

            if (SelectedPositionIndex > 0 && SelectedPositionIndex < Positions.Count)
                foundUsers = foundUsers.Where(t => t.Position == Positions[SelectedPositionIndex]);

            if (SelectedStatusIndex > 0 && SelectedStatusIndex < Statuses.Count)
                foundUsers = foundUsers.Where(t => t.Status == Statuses[SelectedStatusIndex]);

            if (SelectedGenderIndex > 0 && SelectedGenderIndex < Genders.Count)
                foundUsers = foundUsers.Where(t => t.Gender == Positions[SelectedGenderIndex]);

            if (string.IsNullOrEmpty(SearchQuery))
            {
                ResetUsers(foundUsers.ToArray());
                return;
            }

            foundUsers = foundUsers.Where(t => t.Login.Contains(SearchQuery)
                || t.Password.Contains(SearchQuery)
                || t.Name.Contains(SearchQuery)
                || t.Surname.Contains(SearchQuery)
                || t.Patronymic.Contains(SearchQuery)
                || t.PhoneNumber.Contains(SearchQuery)
                || t.DateOfBirth.ToShortDateString().Contains(SearchQuery));

            ResetUsers(foundUsers.ToArray());
        }

        private void ResetSearchFields()
        {
            SelectedPositionIndex = 0;
            SelectedStatusIndex = 0;
            SelectedGenderIndex = 0;
            SearchQuery = string.Empty;

            ResetUsers(_databaseUsers);
        }

        private void RefreshUsers()
        {
            _databaseUsers = UserSearchService.GetAllUsers();

            ResetUsers(_databaseUsers);
            DeselectUser();
        }

        private void ResetUsers(IEnumerable<IUserInfo> newUsers)
        {
            Users.ResetItems(newUsers);
        }

        private void DeselectUser()
        {
            bool isChanged = _selectedUser != null;

            _selectedUser = null;
            IsUserSelected = false;

            if (isChanged)
                SelectedUserChanged?.Invoke(null);

            this.RaisePropertyChanged(nameof(SelectedUser));
        }

        private static ObservableCollection<string> GetPositions()
        {
            return new ObservableCollection<string>()
            {
                NOT_SPECIFIED,
                UserInfo.USER_POSITION,
                UserInfo.ADMIN_POSITION
            };
        }

        private static ObservableCollection<string> GetStatuses()
        {
            return new ObservableCollection<string>()
            {
                NOT_SPECIFIED,
                UserInfo.ACTIVE_STATUS,
                UserInfo.INACTIVE_STATUS
            };
        }

        private static ObservableCollection<string> GetGenders()
        {
            return new ObservableCollection<string>()
            {
                NOT_SPECIFIED,
                "Мужской",
                "Женский"
            };
        }

        #endregion
    }
}
