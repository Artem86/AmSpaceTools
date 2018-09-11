﻿using AmSpaceClient;
using AmSpaceModels.Organization;
using AmSpaceTools.Infrastructure;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmSpaceTools.ViewModels
{
    public class SearchPeopleViewModel : BaseViewModel
    {
        private IAmSpaceClient _client;
        private IEnumerable<SearchUserResult> _searchResult;
        private SearchUserResultWithContractViewModel _selectedUser;
        private string _managerName;
        private string _managerDomain;
        public ObservableCollection<SearchUserResultWithContractViewModel> SearchResultWithContract { get; set; }

        public RelayCommand SearchCommand { get; }
        public RelayCommand ClearCommand { get; }
        public RelayCommand ApplyCommand { get; }

        public string ManagerName
        {
            get { return _managerName; }
            set { _managerName = value; OnPropertyChanged(); }
        }
        public string ManagerDomain
        {
            get { return _managerDomain; }
            set { _managerDomain = value; OnPropertyChanged(); }
        }
        public SearchUserResultWithContractViewModel SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; OnPropertyChanged(nameof(SearchButtonVisible)); OnPropertyChanged(nameof(ApplyButtonVisible)); }
        }
        public bool SearchButtonVisible
        {
            get
            {
                if (SelectedUser == null)
                    return true;
                return false;
            }
        }
        public bool ApplyButtonVisible
        {
            get
            {
                return !SearchButtonVisible;
            }
        }

        public IEnumerable<SearchUserResult> SearchResult
        {
            get { return _searchResult; }
            set { _searchResult = value; OnPropertyChanged(); }
        }

        public SearchPeopleViewModel(IAmSpaceClient client)
        {
            _client = client;
            SearchCommand = new RelayCommand(Search);
            ClearCommand = new RelayCommand(Clear);
            ApplyCommand = new RelayCommand(Apply);
            SearchResultWithContract = new ObservableCollection<SearchUserResultWithContractViewModel>();
        }

        private void Apply(object obj)
        {
            DialogHost.CloseDialogCommand.Execute(true, null);
        }

        private void Clear(object obj)
        {
            ManagerName = string.Empty;
            ManagerDomain = string.Empty;
            SearchResult = null;
            SelectedUser = null;
            SearchResultWithContract.Clear();
        }

        private async void Search(object obj)
        {
            IsLoading = true;
            SearchResultWithContract.Clear();
            SearchResult = await _client.FindUser(ManagerName, null, null, UserStatus.ACTIVE, ManagerDomain);
            SearchResult.ForEach(_ => SearchResultWithContract.Add(new SearchUserResultWithContractViewModel {
                User = _,
                MainContract = _.Contracts.First()
            }));
            IsLoading = false;
        }
    }
}