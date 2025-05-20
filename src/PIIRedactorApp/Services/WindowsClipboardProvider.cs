using System.Windows;

namespace PIIRedactorApp.Services
{
    public class WindowsClipboardProvider : IClipboardProvider
    {
        public bool ContainsText() => Clipboard.ContainsText();
        public string GetText() => Clipboard.GetText();
        public void SetText(string text) => Clipboard.SetText(text);
    }
}
