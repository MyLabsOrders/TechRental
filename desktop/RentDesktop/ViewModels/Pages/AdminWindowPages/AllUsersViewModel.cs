﻿using Avalonia;
using Avalonia.Interactivity;
using ReactiveUI;
using RentDesktop.Infrastructure.Extensions;
using RentDesktop.Infrastructure.Services.DB;
using RentDesktop.Models.Communication;
using RentDesktop.Models.Informing;
using RentDesktop.ViewModels.Base;
using RentDesktop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace RentDesktop.ViewModels.Pages.AdminWindowPages
{
    public class AllUsersViewModel : ViewModelBase
    {
        public AllUsersViewModel() : this(new UserInfo())
        {
        }

        public AllUsersViewModel(IUserInfo userInfo)
        {
            Users = new ObservableCollection<IUserInfo>();

            _currentUserInfo = userInfo;
            _databaseUsers = Array.Empty<IUserInfo>();

            RefreshUsers();

            Positions = GetPositions();
            Statuses = GetStatuses();
            Genders = GetGenders();

            FindUsersCommand = ReactiveCommand.Create(FindUsers);
            ResetSearchFieldsCommand = ReactiveCommand.Create(ResetSearchFields);
            RefreshUsersCommand = ReactiveCommand.Create(RefreshUsers);
            SelectUserCommand = ReactiveCommand.Create<RoutedEventArgs>(SelectClickedUser);
        }

        #region Events

        public delegate void SelectedUserChangedHandler(IUserInfo? userInfo);
        public event SelectedUserChangedHandler? SelectedUserChanged;

        public delegate void SelectedUserChangingHandler(IUserInfo? userInfo);
        public event SelectedUserChangingHandler? SelectedUserChanging;

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

        private IUserInfo _currentUserInfo;
        private ICollection<IUserInfo> _databaseUsers;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> FindUsersCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetSearchFieldsCommand { get; }
        public ReactiveCommand<Unit, Unit> RefreshUsersCommand { get; }
        public ReactiveCommand<RoutedEventArgs, Unit> SelectUserCommand { get; }

        #endregion

        #region Public Methods

        public void AddUser(IUserInfo user)
        {
            Users.Add(user);
            _databaseUsers.Add(user);
        }

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
                foundUsers = foundUsers.Where(t => t.Gender == Genders[SelectedGenderIndex]);

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
            List<IUserInfo> users;

            try
            {
                users = InfoService.GetAllUsers();
            }
            catch (Exception ex)
            {
                string message = "Не удалось обновить список пользователей.";
#if DEBUG
                message += $" Причина: {ex.Message}.";
#endif
                QuickMessage.Error(message).ShowDialog(typeof(AdminWindow));
                return;
            }

            _databaseUsers = users;
            SelectedUser = null;

            ResetUsers(_databaseUsers);
        }

        private void ResetUsers(IEnumerable<IUserInfo> newUsers)
        {
            Users.ResetItems(newUsers);
        }

        private void SelectClickedUser(RoutedEventArgs e)
        {
            if (e.Source is not IDataContextProvider p || p.DataContext is not IUserInfo userInfo)
                return;

            SelectedUserChanging?.Invoke(userInfo);

            if (userInfo.ID != _currentUserInfo.ID)
                SelectedUser = userInfo;
        }

        private static ObservableCollection<string> GetPositions()
        {
            List<string> positions;

            try
            {
                positions = InfoService.GetAllPositions();
            }
            catch (Exception ex)
            {
                positions = new List<string>();
#if DEBUG
                string message = $"Не удалось получить роли. Причина: {ex.Message}";
                QuickMessage.Error(message).ShowDialog(typeof(AdminWindow));
#endif
            }

            positions.Insert(0, NOT_SPECIFIED);
            return new ObservableCollection<string>(positions);
        }

        private static ObservableCollection<string> GetStatuses()
        {
            List<string> statuses;

            try
            {
                statuses = InfoService.GetAllStatuses();
            }
            catch (Exception ex)
            {
                statuses = new List<string>();
#if DEBUG
                string message = $"Не удалось загрузить статусы. Причина: {ex.Message}";
                QuickMessage.Error(message).ShowDialog(typeof(AdminWindow));
#endif
            }

            statuses.Insert(0, NOT_SPECIFIED);
            return new ObservableCollection<string>(statuses);
        }

        private static ObservableCollection<string> GetGenders()
        {
            List<string> genders;

            try
            {
                genders = InfoService.GetAllGenders();
            }
            catch (Exception ex)
            {
                genders = new List<string>();
#if DEBUG
                string message = $"Не удалось загрузить полы. Причина: {ex.Message}";
                QuickMessage.Error(message).ShowDialog(typeof(AdminWindow));
#endif
            }

            genders.Insert(0, NOT_SPECIFIED);
            return new ObservableCollection<string>(genders);
        }

        #endregion
    }
}
