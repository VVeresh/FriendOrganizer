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

            context.ProgrammingLanguages.AddOrUpdate(pl => pl.Name,
                new ProgrammingLanguage { Name = "C#" },
                new ProgrammingLanguage { Name = "TypeScript" },
                new ProgrammingLanguage { Name = "F#" },
                new ProgrammingLanguage { Name = "Swift" },
                new ProgrammingLanguage { Name = "Python" });

            context.SaveChanges(); // so first friend can have id

            context.FriendPhoneNumbers.AddOrUpdate(pn => pn.Number, new FriendPhoneNumber { Number = "+49 12345678", FriendId = context.Friends.First().Id });
        }
        // PM> Enable-Migrations
        // PM> Add-Migration InitialDatabase
        // PM> Add-Migration InitialDatabase -Force       - after changing model
        // PM> Update-Database
    }
}
 