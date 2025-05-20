using System;

namespace PIIRedactorApp.Models
{
    public class HistoryEntry
    {
        public DateTime Timestamp { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
