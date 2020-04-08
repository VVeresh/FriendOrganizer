using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    /// <summary>
    /// Interface to be used in ViewModel
    /// </summary>
    public interface IFriendRepository
    {
        //IEnumerable<Friend> GetAll();
        Task<Friend> GetByIdAsync(int friendId);
        Task SaveAsync(/*Friend friend*/);
        bool HasChanges();
        void Add(Friend friend);
        void Remove(Friend model);
    }
}