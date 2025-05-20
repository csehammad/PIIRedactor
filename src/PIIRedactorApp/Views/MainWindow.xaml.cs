using System.Windows;
using PIIRedactorApp.ViewModels;

namespace PIIRedactorApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var win = new SettingsWindow(viewModel.Config);
            if (win.ShowDialog() == true)
            {
                viewModel.Config = win.Config;
            }
        }
    }
}
