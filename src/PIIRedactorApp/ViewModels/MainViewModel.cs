using System;
using System.Collections.ObjectModel;
using System.IO;
using PIIRedactorApp.Models;
using PIIRedactorApp.Services;

namespace PIIRedactorApp.ViewModels
{
    public class MainViewModel
    {
        private const int MaxItemsInMemory = 100;
        private readonly ClipboardService clipboardService;
        private readonly HistoryRepository repository;

        public ObservableCollection<HistoryEntry> ClipboardHistory { get; } = new();

        public MainViewModel()
        {
            clipboardService = new ClipboardService();
            repository = new HistoryRepository(Path.Combine(AppContext.BaseDirectory, "history.json"));

            foreach (var entry in repository.LoadLatestEntries(MaxItemsInMemory))
            {
                ClipboardHistory.Insert(0, entry);
            }

            clipboardService.ClipboardChanged += OnClipboardChanged;
        }

        private void OnClipboardChanged(object? sender, string text)
        {
            repository.Add(text);
            ClipboardHistory.Insert(0, new HistoryEntry { Timestamp = DateTime.UtcNow, Text = text });
            while (ClipboardHistory.Count > MaxItemsInMemory)
            {
                ClipboardHistory.RemoveAt(ClipboardHistory.Count - 1);
            }
        }

        public RedactorConfig Config
        {
            get => clipboardService.Config;
            set => clipboardService.Config = value;
        }
    }
}
