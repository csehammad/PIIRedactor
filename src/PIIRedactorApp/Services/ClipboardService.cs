using System;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.ML;
using PIIRedactorApp.Models;
using System.Windows.Threading;

namespace PIIRedactorApp.Services
{
    public class ClipboardService
    {
        private string lastText = string.Empty;
        private readonly DispatcherTimer timer;
        private ITransformer? model;
        public RedactorConfig Config { get; set; }

        public event EventHandler<string>? ClipboardChanged;

        public ClipboardService()
        {
            Config = TemplateProvider.LoadTemplates()[0];
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
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
            catch
            {
                model = null;
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                var text = Clipboard.GetText();
                if (text != lastText)
                {
                    lastText = Sanitize(text);
                    Clipboard.SetText(lastText);
                    ClipboardChanged?.Invoke(this, lastText);
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
                // TODO: integrate ML model prediction
            }
            return result;
        }
    }
}
