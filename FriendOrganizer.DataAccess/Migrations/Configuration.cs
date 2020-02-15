namespace FriendOrganizer.DataAccess.Migrations
{
    using FriendOrganizer.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizer.DataAccess.FriendOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOrganizer.DataAccess.FriendOrganizerDbContext context)
        {
            context.Friends.AddOrUpdate(
                f => f.FirstName,     // indentify expression for indentifying objects; if FirstName exist update, else add it
                new Friend { FirstName = "Toma", LastName = "Hujic" },
                new Friend { FirstName = "Uros", LastName = "Meric" },
                new Friend { FirstName = "Era", LastName = "Eregic" },
                new Friend { FirstName = "Sara", LastName = "Hujic" }
                );
        }
        // PM> Enable-Migration
        // PM> Add-Migrations InitialDatabase
        // PM> Add-Migrations InitialDatabase -Force       - after changing model
        // PM> Update-Database
    }
}
 