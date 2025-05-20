using System;
using System.Text.RegularExpressions;
using Microsoft.ML;
using PIIRedactorApp.Models;
using System.Windows.Threading;
using System.IO;
using PIIRedactorApp.Services;

namespace PIIRedactorApp
{
    public class ClipboardService
    {
        private string lastText = string.Empty;
        private readonly DispatcherTimer timer;
        private ITransformer? model;
        private readonly IClipboardProvider clipboard;
        public RedactorConfig Config { get; set; }

        public event EventHandler<string>? ClipboardChanged;

        public ClipboardService()
        {
            Config = ConfigManager.Load();
            clipboard = Environment.OSVersion.Platform == PlatformID.Win32NT
                ? new WindowsClipboardProvider() as IClipboardProvider
                : new DummyClipboardProvider();
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
            timer.Tick += Timer_Tick;
            timer.Start();
            LoadModel();
        }

        private void LoadModel()
        {
            try
            {
                var ml = new MLContext();
                model = ml.Model.Load("models/PIIModel.zip", out var _);
            }
            catch (Exception ex)
            {
                model = null;
                Logger.Log($"Failed to load model: {ex.Message}");
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (clipboard.ContainsText())
            {
                var text = clipboard.GetText();
                if (text != lastText)
                {
                    lastText = Sanitize(text);
                    clipboard.SetText(lastText);
                    ClipboardChanged?.Invoke(this, lastText);
                    Logger.Log("Clipboard sanitized");
                }
            }
        }

        private string Sanitize(string input)
        {
            string result = input;
            foreach (var pattern in Config.Patterns)
            {
                result = Regex.Replace(result, pattern, "[REDACTED]");
            }
            if (Config.UseMLModel && model != null)
            {
                result = PredictSensitive(result);
            }
            else if (Config.UseMLModel && model == null)
            {
                Logger.Log("ML model not loaded; skipping ML sanitization.");
            }
            return result;
        }

        private string PredictSensitive(string text)
        {
            // TODO: integrate ML model prediction when model schema is known
            return text;
        }
    }
}
