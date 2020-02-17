using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        private Func<FriendOrganizerDbContext> _contextCreator;

        public FriendDataService(Func<FriendOrganizerDbContext> contextCreator)        // Inject DbContext via Autofuc; need to register on container
        {
            _contextCreator = contextCreator;
        }

        /// <summary>
        /// Load data into View Model
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<Friend> GetAll()
        //{
        //    // TODO: Load data from real database - DONE
        //    //using(var ctx = new FriendOrganizerDbContext())     // We should inject DbContext via DI
        //    //{
        //    //    return ctx.Friends.AsNoTracking().ToList();
        //    //}
        //    using (var ctx = _contextCreator())     // Function that return dbContext
        //    {
        //        return ctx.Friends.AsNoTracking().ToList();
        //    }

        //    // Fake data for loading in ViewModel
        //    //yield return new Friend { FirstName = "Marko", LastName = "Markovic" };
        //    //yield return new Friend { FirstName = "Ivan", LastName = "Ivanic" };
        //    //yield return new Friend { FirstName = "Pera", LastName = "Peric" };
        //    //yield return new Friend { FirstName = "Zarko", LastName = "Zaric" };
        //}
       
        public async Task<Friend> GetByIdAsync(int friendId)       // Instead for Task<List<Friend>> we want to return a single friend
        {
            using(var ctx = _contextCreator())
            {
                return await ctx.Friends.AsNoTracking().SingleAsync(f => f.Id == friendId);      // It can be disposed before it return, so we shoul await method

                //// TESTING ASYNC LOADING:
                //var friends = await ctx.Friends.AsNoTracking().ToListAsync();
                //await Task.Delay(5000);
                //return friends;
            }
        }
    }
}
