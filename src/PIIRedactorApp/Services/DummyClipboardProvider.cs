namespace PIIRedactorApp.Services
{
    // Placeholder implementation for non-Windows platforms
    public class DummyClipboardProvider : IClipboardProvider
    {
        public bool ContainsText() => false;
        public string GetText() => string.Empty;
        public void SetText(string text) { }
    }
}
