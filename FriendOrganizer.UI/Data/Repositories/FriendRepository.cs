using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        //private Func<FriendOrganizerDbContext> _contextCreator;
        private FriendOrganizerDbContext _context;

        public FriendRepository(FriendOrganizerDbContext context)        // Inject DbContext via Autofuc; need to register on container
        {
            _context = context;
        }

        public void Add(Friend friend)
        {
            _context.Friends.Add(friend);
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
            return await _context.Friends
                .Include(f => f.PhoneNumbers)
                .SingleAsync(f => f.Id == friendId);
            //using(var ctx = _contextCreator())
            //{
            /* return await ctx.Friends.AsNoTracking().SingleAsync(f => f.Id == friendId);*/      // It can be disposed before it return, so we shoul await method

            //// TESTING ASYNC LOADING:
            //var friends = await ctx.Friends.AsNoTracking().ToListAsync();
            //await Task.Delay(5000);
            //return friends;
            //}
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Remove(Friend model)
        {
            _context.Friends.Remove(model);
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            _context.FriendPhoneNumbers.Remove(model);
        }

        public async Task SaveAsync(/*Friend friend*/)
        {
            // Call Func to ged DbContext via _contextCreator
            //using(var ctx = _contextCreator())
            //{
            //    // First attach Friend to the Context so it is awere of this instance
            //    ctx.Friends.Attach(friend);
            //    ctx.Entry(friend).State = EntityState.Modified;     // Context is awere that this instance is changed
            //    await ctx.SaveChangesAsync();
            //}
            await _context.SaveChangesAsync();
        }
    }
}
