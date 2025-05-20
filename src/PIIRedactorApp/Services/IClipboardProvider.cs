namespace PIIRedactorApp.Services
{
    public interface IClipboardProvider
    {
        bool ContainsText();
        string GetText();
        void SetText(string text);
    }
}
