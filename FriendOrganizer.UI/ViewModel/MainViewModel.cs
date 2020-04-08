using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    /// <summary>
    /// Class that use DataService for loading model, it provide data for View
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        // NOT REQUIRED
        //private IFriendDataService _friendDataService;      // property for geting all friend data
        //private Friend _selectedFriend;                     // property of specific friend. when it is setup we need to rise event handler
        private IFriendDetailViewModel _friendDetailViewModel;
        private IEventAggregator _eventAggregator;
        private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
        private IMessageDialogService _messageDialogService;

        public MainViewModel(INavigationViewModel navigationViewModel, /*IFriendDataService friendDataService*/
                             Func<IFriendDetailViewModel> friendDetailViewModelCreator, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
                .Subscribe(OnOpenFriendDetailView);
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Subscribe(AfterFriendDeleted);

            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendExecute);

            NavigationViewModel = navigationViewModel;

            //Friends = new ObservableCollection<Friend>();   // Inicialize collection that implement INotifyPropertyChanged interface
            //_friendDataService = friendDataService;         // Load data from Friend data service
        }

        /// <summary>
        /// Loading friends data to Friends collection, each time new data
        /// </summary>
        public async Task LoadAsync()        // Insted void it should return Task
        {
            await NavigationViewModel.LoadAsync();

            //var friends = _friendDataService.GetAll();
            //var friends = await _friendDataService.GetAllAsync();    // await to get friends
            //Friends.Clear();
            //foreach(var friend in friends)
            //{
            //    Friends.Add(friend);
            //}
        }        

        public ICommand CreateNewFriendCommand { get; }

        public INavigationViewModel NavigationViewModel { get; }

        public IFriendDetailViewModel FriendDetailViewModel
        {
            get { return _friendDetailViewModel; }
            private set
            {
                _friendDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /*public IFriendDetailViewModel FriendDetailViewModel { get; }*/    // We don`t need set becouse we setup property directly into ctor, we bind it to friendView

        // WE DON`T NEED THIS IN MainViewModel
        //public ObservableCollection<Friend> Friends { get; set; }        

        //public Friend SelectedFriend
        //{
        //    get { return _selectedFriend; }
        //    set
        //    {
        //        _selectedFriend = value;
        //        OnPropertyChanged();        // rise event handler; property name if automaticly passed from compiler via atribut in the method
        //    }
        //}     
        private async void OnOpenFriendDetailView(int? friendId)
        {
            if (FriendDetailViewModel != null && FriendDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You've made changes. Navigate away?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            FriendDetailViewModel = _friendDetailViewModelCreator();
            await FriendDetailViewModel.LoadAsync(friendId);
        }


        private void OnCreateNewFriendExecute()
        {
            OnOpenFriendDetailView(null);
        }


        private void AfterFriendDeleted(int friendId)
        {
            FriendDetailViewModel = null;
        }


    }
}
