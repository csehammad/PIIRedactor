using System.Windows;
using PIIRedactorApp.ViewModels;

namespace PIIRedactorApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

    }
}
