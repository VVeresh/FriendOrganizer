using FriendOrganizer.UI.ViewModel;
using System;
using System.Windows;

namespace FriendOrganizer.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;
        }

        /// <summary>
        /// Event that load data from MainViewModel.Load()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();   // Also changed from MainViewModel
        }
    }
}
