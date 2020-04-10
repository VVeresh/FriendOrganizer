using Autofac.Features.Indexed;
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
        private IDetailViewModel _detailViewModel;
        private IEventAggregator _eventAggregator;
        private IIndex<string, IDetailViewModel> _detailViewModelCreator;
        //private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
        //private Func<IMeetingDetailViewModel> _meetingDetailViewModelCreator;
        private IMessageDialogService _messageDialogService;

        public MainViewModel(INavigationViewModel navigationViewModel, /*IFriendDataService friendDataService*/
                             /*Func<IFriendDetailViewModel> friendDetailViewModelCreator,
                             Func<IMeetingDetailViewModel> meetingDetailViewModelCreator,*/
                             IIndex<string, IDetailViewModel> detailViewModelCreator,
                             IEventAggregator eventAggregator, 
                             IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _detailViewModelCreator = detailViewModelCreator;
            //_friendDetailViewModelCreator = friendDetailViewModelCreator;
            //_meetingDetailViewModelCreator = meetingDetailViewModelCreator;
            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailView);
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);

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

        public ICommand CreateNewDetailCommand { get; }

        public INavigationViewModel NavigationViewModel { get; }

        public IDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            private set
            {
                _detailViewModel = value;
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
        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You've made changes. Navigate away?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            //switch (args.ViewModelName)
            //{
            //    case nameof(FriendDetailViewModel):
            //        DetailViewModel = _friendDetailViewModelCreator();
            //        break;
            //    case nameof(MeetingDetailViewModel):
            //        DetailViewModel = _meetingDetailViewModelCreator();
            //        break;
            //    default:
            //        throw new Exception($"ViewModel {args.ViewModelName} not mapped");
            //}
            DetailViewModel = _detailViewModelCreator[args.ViewModelName];
            await DetailViewModel.LoadAsync(args.Id);
        }


        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(
                new OpenDetailViewEventArgs { ViewModelName = viewModelType.Name });
        }


        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            DetailViewModel = null;
        }


    }
}
