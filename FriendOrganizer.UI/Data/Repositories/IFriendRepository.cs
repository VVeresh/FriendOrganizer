﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    /// <summary>
    /// Interface to be used in ViewModel
    /// </summary>
    public interface IFriendRepository : IGenericRepository<Friend>
    {
        //IEnumerable<Friend> GetAll();        
        void RemovePhoneNumber(FriendPhoneNumber model);
        Task<bool> HasMeetingsAsync(int friendId);
    }
}