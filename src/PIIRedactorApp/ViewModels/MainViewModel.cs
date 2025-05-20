using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using PIIRedactorApp.Services;
using PIIRedactorApp.Views;

namespace PIIRedactorApp.ViewModels
{
    public class ClipboardItem
    {
        public string Text { get; set; } = string.Empty;
        public DateTime Time { get; set; }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Predicate<T?>? _canExecute;

        public RelayCommand(Action<T?> execute, Predicate<T?>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute((T?)parameter);
        }

        public void Execute(object? parameter)
        {
            _execute((T?)parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ClipboardService _service;

        public ObservableCollection<ClipboardItem> ClipboardHistory { get; } = new();

        public ICommand CopyCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand OpenSettingsCommand { get; }

        public MainViewModel()
        {
            _service = new ClipboardService();
            _service.ClipboardChanged += Service_ClipboardChanged;

            CopyCommand = new RelayCommand<ClipboardItem>(CopyItem);
            DeleteCommand = new RelayCommand<ClipboardItem>(DeleteItem);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
        }

        private void Service_ClipboardChanged(object? sender, string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ClipboardHistory.Insert(0, new ClipboardItem
                {
                    Text = text,
                    Time = DateTime.Now
                });
            });
        }

        private void CopyItem(ClipboardItem? item)
        {
            if (item != null)
            {
                Clipboard.SetText(item.Text);
            }
        }

        private void DeleteItem(ClipboardItem? item)
        {
            if (item != null)
            {
                ClipboardHistory.Remove(item);
            }
        }

        private void OpenSettings()
        {
            var win = new SettingsWindow(_service.Config);
            if (win.ShowDialog() == true)
            {
                _service.Config = win.Config;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
