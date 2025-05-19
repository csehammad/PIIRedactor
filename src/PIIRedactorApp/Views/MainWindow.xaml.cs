using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace PIIRedactorApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly ClipboardService clipboardService;
        public ObservableCollection<string> ClipboardHistory { get; } = new();

        public MainWindow()
        {
            InitializeComponent();
            clipboardService = new ClipboardService();
            clipboardService.ClipboardChanged += OnClipboardChanged;
        }

        private void OnClipboardChanged(object sender, string text)
        {
            Dispatcher.Invoke(() => {
                ClipboardHistory.Insert(0, text);
                ClipboardList.ItemsSource = ClipboardHistory;
            });
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var win = new SettingsWindow(clipboardService.Config);
            if (win.ShowDialog() == true)
            {
                clipboardService.Config = win.Config;
            }
        }
    }
}
