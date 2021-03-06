﻿using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : DetailViewModelBase, IFriendDetailViewModel
    {
        private IFriendRepository _friendRepository;
        //private IEventAggregator _eventAggregator;
        //private IMessageDialogService _messageDialogService;
        private IProgrammingLanguageLookupDataService _programmingLanguageLookupDataService;
        private FriendWrapper _friend;
        //private bool _hasChanges;
        private FriendPhoneNumberWrapper _selectedPhoneNumber;

        public FriendDetailViewModel(IFriendRepository friendRepository,
                                     IEventAggregator eventAggregator, IMessageDialogService messageDialogService, IProgrammingLanguageLookupDataService programmingLanguageLookupDataService) : base(eventAggregator, messageDialogService)
        {
            _friendRepository = friendRepository;
            //_eventAggregator = eventAggregator;
            //_messageDialogService = messageDialogService;
            _programmingLanguageLookupDataService = programmingLanguageLookupDataService;

            eventAggregator.GetEvent<AfterCollectionSavedEvent>()
                .Subscribe(AfterCollectionSaved);

            //SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            //DeleteCommand = new DelegateCommand(OnDeleteExecute);
            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<FriendPhoneNumberWrapper>();
        }

        public override async Task LoadAsync(int friendId)
        {
            var friend = friendId > 0
                ? await _friendRepository.GetByIdAsync(friendId)
                : CreateNewFriend();

            Id = friendId;

            InitializeFriend(friend);

            InitializeFriendPhoneNumbers(friend.PhoneNumbers);

            await LoadProgrammingLanguagesLookupAsync();
        }

        private void InitializeFriend(Friend friend)
        {
            Friend = new FriendWrapper(friend);

            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _friendRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
                if (e.PropertyName == nameof(Friend.FirstName)
                 || e.PropertyName == nameof(Friend.LastName))
                {
                    SetTitle();
                }
            };

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Friend.Id == 0)
            {
                // Little trick to trigger validation
                Friend.FirstName = "";
            }
            SetTitle();
        }

        private void SetTitle()
        {
            Title = $"{Friend.FirstName} {Friend.LastName}";
        }

        private void InitializeFriendPhoneNumbers(ICollection<FriendPhoneNumber> phoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();
            foreach (var friendPhoneNumber in phoneNumbers)
            {
                var wrapper = new FriendPhoneNumberWrapper(friendPhoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            }
        }

        private void FriendPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _friendRepository.HasChanges();
            }
            if (e.PropertyName == nameof(FriendPhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem { DisplayMember = " - " });
            var lookup = await _programmingLanguageLookupDataService.GetProgrammingLanguageLookupAsync();
            foreach (var lookupItem in lookup)
            {
                ProgrammingLanguages.Add(lookupItem);
            }
        }

        public FriendWrapper Friend
        {
            get { return _friend; }
            private set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public FriendPhoneNumberWrapper SelectedPhoneNumber
        {
            get { return _selectedPhoneNumber; }
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }

        //public bool HasChanges
        //{
        //    get { return _hasChanges; }
        //    set
        //    {
        //        if (_hasChanges != value)
        //        {
        //            _hasChanges = value;
        //            OnPropertyChanged();
        //            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        //        }
        //    }
        //}

        //public ICommand SaveCommand { get; }        // we dont need setter becouse it is init in ctor of FriendDetailViewModel

        //public ICommand DeleteCommand { get; }

        public ICommand AddPhoneNumberCommand { get; }

        public ICommand RemovePhoneNumberCommand { get; }

        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }

        public ObservableCollection<FriendPhoneNumberWrapper> PhoneNumbers { get; }

        protected override async void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(_friendRepository.SaveAsync,
                () =>
                {
                    HasChanges = _friendRepository.HasChanges();
                    Id = Friend.Id;
                    RaiseDetailSavedEvent(Friend.Id, $"{Friend.FirstName} {Friend.LastName}");
                });
            //try
            //{
            //    await _friendRepository.SaveAsync();
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    var databaseValues = ex.Entries.Single().GetDatabaseValues();
            //    if (databaseValues == null)
            //    {
            //        MessageDialogService.ShowInfoDialog("The entity has been deletd by another user");
            //        RaiseDetailDeletedEvent(Id);
            //        return;
            //    }

            //    var result = MessageDialogService.ShowOkCancelDialog("The entity has been changed in the meantime by someone else. Click OK to save your changes anyway, click Cancel to reload the entity from the database.", "Question");

            //    if (result == MessageDialogResult.OK)
            //    {
            //        // Update the original values with database-values
            //        var entry = ex.Entries.Single();
            //        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
            //        await _friendRepository.SaveAsync();
            //    }
            //    else
            //    {
            //        // Reload entity from database
            //        await ex.Entries.Single().ReloadAsync();
            //        await LoadAsync(Friend.Id);
            //    }
            //};

            //HasChanges = _friendRepository.HasChanges();
            //Id = Friend.Id;
            //RaiseDetailSavedEvent(Friend.Id, $"{Friend.FirstName} {Friend.LastName}");

            //await _friendRepository.SaveAsync(/*Friend.Model*/);
            //HasChanges = _friendRepository.HasChanges();
            //Id = Friend.Id;
            //RaiseDetailSavedEvent(Friend.Id, $"{Friend.FirstName} {Friend.LastName}");
            //_eventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(
            //    new AfterDetailSavedEventArgs
            //    {
            //        Id = Friend.Id,
            //        DisplayMember = $"{Friend.FirstName} {Friend.LastName}",
            //        ViewModelName = nameof(FriendDetailViewModel)
            //    });
        }

        protected override bool OnSaveCanExecute()
        {
            return Friend != null
                && !Friend.HasErrors
                && PhoneNumbers.All(pn => !pn.HasErrors)
                && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            if (await _friendRepository.HasMeetingsAsync(Friend.Id))
            {
                await MessageDialogService.ShowInfoDialogAsync($"{Friend.FirstName} {Friend.LastName} can`t be deleted, as this friend is part of at least one meeting.");
                return;
            }

            var result = await MessageDialogService.ShowOkCancelDialogAsync($"Do you really want to delete the friend {Friend.FirstName} {Friend.LastName}", "Question");
            if (result == MessageDialogResult.OK)
            {
                _friendRepository.Remove(Friend.Model);
                await _friendRepository.SaveAsync();
                RaiseDetailDeletedEvent(Friend.Id);
                //_eventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(
                //    new AfterDetailDeletedEventArgs
                //    {
                //        Id = Friend.Id,
                //        ViewModelName = nameof(FriendDetailViewModel)
                //    });
            }
        }

        private void OnAddPhoneNumberExecute()
        {
            var newNumber = new FriendPhoneNumberWrapper(new FriendPhoneNumber());
            newNumber.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Friend.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = ""; // Trigger validation
        }

        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            _friendRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _friendRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemovePhoneNumberCanExecute()
        {
            return SelectedPhoneNumber != null;
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            _friendRepository.Add(friend);
            return friend;
        }

        private async void AfterCollectionSaved(AfterCollectionSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(ProgrammingLanguageDetailViewModel))
            {
                await LoadProgrammingLanguagesLookupAsync();
            }
        }
    }
}
