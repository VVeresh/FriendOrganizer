using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    /// <summary>
    /// Interface to be used in ViewModel
    /// </summary>
    public interface IFriendDataService
    {
        //IEnumerable<Friend> GetAll();
        Task<List<Friend>> GetAllAsync();
    }
}