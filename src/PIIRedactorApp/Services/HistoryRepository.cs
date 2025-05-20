using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using PIIRedactorApp.Models;

namespace PIIRedactorApp.Services
{
    public class HistoryRepository
    {
        private readonly string path;

        public HistoryRepository(string path)
        {
            this.path = path;
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
        }

        public void Add(string text)
        {
            var entry = new HistoryEntry
            {
                Timestamp = DateTime.UtcNow,
                Text = text
            };
            var json = JsonSerializer.Serialize(entry);
            File.AppendAllText(path, json + Environment.NewLine);
        }

        public IEnumerable<HistoryEntry> LoadLatestEntries(int count)
        {
            var queue = new Queue<string>();
            foreach (var line in File.ReadLines(path))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                queue.Enqueue(line);
                if (queue.Count > count)
                    queue.Dequeue();
            }

            foreach (var line in queue)
            {
                var entry = JsonSerializer.Deserialize<HistoryEntry>(line);
                if (entry != null)
                    yield return entry;
            }
        }
    }
}
