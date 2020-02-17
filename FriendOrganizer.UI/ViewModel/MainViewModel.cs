using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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

        public MainViewModel(INavigationViewModel navigationViewModel, /*IFriendDataService friendDataService*/
                             IFriendDetailViewModel friendDetailViewModel)
        {
            NavigationViewModel = navigationViewModel;
            FriendDetailViewModel = friendDetailViewModel;
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

        public INavigationViewModel NavigationViewModel { get; }
        public IFriendDetailViewModel FriendDetailViewModel { get; }    // We don`t need set becouse we setup property directly into ctor, we bind it to friendView

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
    }
}
