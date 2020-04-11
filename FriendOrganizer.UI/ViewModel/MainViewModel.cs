using Autofac.Features.Indexed;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private IDetailViewModel _selectedDetailViewModel;
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
            _eventAggregator.GetEvent<AfterDetailClosedEvent>()
                .Subscribe(AfterDetailClosed);

            DetailViewModels = new ObservableCollection<IDetailViewModel>();

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            OpenSingleDetailViewCommand = new DelegateCommand<Type>(OnOpenSingleDetailViewExecute);

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

        public ICommand OpenSingleDetailViewCommand { get; }

        public INavigationViewModel NavigationViewModel { get; }

        public ObservableCollection<IDetailViewModel> DetailViewModels { get; }

        public IDetailViewModel SelectedDetailViewModel
        {
            get { return _selectedDetailViewModel; }
            set
            {
                _selectedDetailViewModel = value;
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

        // This method is called whenever user click item in navigation
        // If no one detailViewModel exist one is created by if statement
        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            // args.Id contain Id of view that is clicked
            var detailViewModel = DetailViewModels
                .SingleOrDefault(vm => vm.Id == args.Id && vm.GetType().Name == args.ViewModelName);

            // If ViewModel did not exist in observable collection
            if (detailViewModel == null)
            {
                detailViewModel = _detailViewModelCreator[args.ViewModelName];
                try
                {
                    await detailViewModel.LoadAsync(args.Id);
                }
                catch
                {
                    _messageDialogService.ShowInfoDialog("Could not load the entity maybe it was deleted in the meantime by another user. The navigation is refreshed for you.");
                    await NavigationViewModel.LoadAsync();
                    return;
                }
                DetailViewModels.Add(detailViewModel);
            }

            // Logic if user change something and navigate from current detailView
            //if (SelectedDetailViewModel != null && SelectedDetailViewModel.HasChanges)
            //{
            //    var result = _messageDialogService.ShowOkCancelDialog("You've made changes. Navigate away?", "Question");
            //    if (result == MessageDialogResult.Cancel)
            //    {
            //        return;
            //    }
            //}
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

            SelectedDetailViewModel = detailViewModel;

        }

        private int nextNewItemId = 0;

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(
                new OpenDetailViewEventArgs
                {
                    Id = nextNewItemId--,
                    ViewModelName = viewModelType.Name
                });
        }

        private void OnOpenSingleDetailViewExecute(Type viewModelType)
        {
            OnOpenDetailView(
                new OpenDetailViewEventArgs
                {
                    Id = -1,
                    ViewModelName = viewModelType.Name
                });
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);

            //SelectedDetailViewModel = null;
        }

        private void AfterDetailClosed(AfterDetailClosedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void RemoveDetailViewModel(int id, string viewModelName)
        {
            var detailViewModel = DetailViewModels
                            .SingleOrDefault(vm => vm.Id == id && vm.GetType().Name == viewModelName);

            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }
    }
}
