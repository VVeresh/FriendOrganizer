using Autofac;
using FriendOrganizer.DataAccess;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.ViewModel;
using Prism.Events;

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

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();        // Register Prism event aggregator, we can add it to ctor parameter in ViewModel

            builder.RegisterType<FriendOrganizerDbContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>().As<IFriendDetailViewModel>();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();    // Register class for multiple interfaces
            builder.RegisterType<FriendDataService>().As<IFriendDataService>();     // Use FriendDataService whenever IFriendDataService is required
            
            return builder.Build();
        }
    }
}
