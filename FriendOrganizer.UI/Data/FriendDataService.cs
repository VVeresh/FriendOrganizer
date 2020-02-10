using FriendOrganizer.Model;
using System.Collections.Generic;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        /// <summary>
        /// Load data into View Model
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Friend> GetAll()
        {
            // TODO: Load data from real database
            // Fake data for loading in ViewModel
            yield return new Friend { FirstName = "Marko", LastName = "Markovic" };
            yield return new Friend { FirstName = "Ivan", LastName = "Ivanic" };
            yield return new Friend { FirstName = "Pera", LastName = "Peric" };
            yield return new Friend { FirstName = "Zarko", LastName = "Zaric" };
        }
    }
}
