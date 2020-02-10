using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FriendOrganizer.UI.ViewModel
{
    /// <summary>
    /// Contain methods that are same for all ViewModel types
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        // Event that is rised when property is changed
        public event PropertyChangedEventHandler PropertyChanged;

        // Notify data bindings when property is changed
        // Private method that charge event
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)      // Name of property that is changed
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
