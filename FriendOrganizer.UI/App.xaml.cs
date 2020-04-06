using Autofac;
using FriendOrganizer.UI.Startup;
using System;
using System.Windows;

namespace FriendOrganizer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstapper = new Bootstrapper();
            var container = bootstapper.Bootstrap();

            //var mainWindow = new MainWindow(          OLD CODE!
            //    new MainViewModel(
            //        new FriendDataService()));

            var mainWindow = container.Resolve<MainWindow>();

            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error occured. Please inform the admin. "
                + Environment.NewLine + e.Exception.Message, "Unexpected error");
            e.Handled = true; // property that set exceptions to handeled so app can run
        }
    }
}
