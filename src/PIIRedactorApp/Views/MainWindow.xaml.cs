using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using PIIRedactorApp.Models;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PIIRedactorApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly ClipboardService clipboardService;
        private readonly NotifyIcon trayIcon;
        public ObservableCollection<string> ClipboardHistory { get; } = new();
        private const string HistoryFile = "history.txt";

        public MainWindow()
        {
            InitializeComponent();
            clipboardService = new ClipboardService();
            clipboardService.ClipboardChanged += OnClipboardChanged;
            LoadHistory();
            this.Closing += OnClosing;

            trayIcon = new NotifyIcon
            {
                Icon = System.Drawing.SystemIcons.Application,
                Visible = true,
                Text = "PII Redactor"
            };
            trayIcon.DoubleClick += (s, e) => { this.Show(); this.WindowState = WindowState.Normal; };
            var menu = new ContextMenuStrip();
            menu.Items.Add("Exit", null, (s, e) => Close());
            trayIcon.ContextMenuStrip = menu;
        }

        private void OnClipboardChanged(object sender, string text)
        {
            Dispatcher.Invoke(() => {
                ClipboardHistory.Insert(0, text);
                ApplyFilter(SearchBox.Text);
            });
        }

        private void LoadHistory()
        {
            if (File.Exists(HistoryFile))
            {
                foreach (var line in File.ReadAllLines(HistoryFile))
                {
                    ClipboardHistory.Add(line);
                }
            }
            ClipboardList.ItemsSource = ClipboardHistory;
        }

        private void OnClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            File.WriteAllLines(HistoryFile, ClipboardHistory);
            trayIcon.Visible = false;
            trayIcon.Dispose();
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ApplyFilter(SearchBox.Text);
        }

        private void ApplyFilter(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                ClipboardList.ItemsSource = ClipboardHistory;
            }
            else
            {
                ClipboardList.ItemsSource = new ObservableCollection<string>(ClipboardHistory.Where(h => h.Contains(query, System.StringComparison.OrdinalIgnoreCase)));
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var win = new SettingsWindow(clipboardService.Config);
            if (win.ShowDialog() == true)
            {
                clipboardService.Config = win.Config;
                ConfigManager.Save(win.Config);
            }
        }
    }
}
