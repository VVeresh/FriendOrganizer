using Autofac;
using FriendOrganizer.DataAccess;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.ViewModel;

namespace FriendOrganizer.UI.Startup
{
    /// <summary>
    /// Responsible for creating Autofac container
    /// Container knows about all types, and create all instances
    /// </summary>
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<FriendDataService>().As<IFriendDataService>();     // Use FriendDataService whenever IFriendDataService is required
            builder.RegisterType<FriendOrganizerDbContext>().AsSelf();

            return builder.Build();
        }
    }
}
