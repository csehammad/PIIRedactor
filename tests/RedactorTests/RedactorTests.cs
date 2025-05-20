using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIIRedactorApp.Services;
using PIIRedactorApp.Models;
using System.Reflection;
using System.Linq;
using System.IO;
using System;
using System.Windows.Threading;

namespace RedactorTests
{
    [TestClass]
    public class RedactorTests
    {
        [TestMethod]
        public void Email_Is_Redacted()
        {
            var service = new ClipboardService();
            // stop internal timer to avoid side effects during tests
            var timerField = typeof(ClipboardService)
                .GetField("timer", BindingFlags.NonPublic | BindingFlags.Instance);
            (timerField?.GetValue(service) as DispatcherTimer)?.Stop();

            var sanitize = typeof(ClipboardService)
                .GetMethod("Sanitize", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(sanitize);

            var input = "please contact me at test@example.com";
            var output = (string)sanitize!.Invoke(service, new object[] { input })!;

            Assert.IsFalse(output.Contains("test@example.com"));
            Assert.IsTrue(output.Contains("[REDACTED]"));
        }

        [TestMethod]
        public void Templates_Are_Loaded()
        {
            var templates = TemplateProvider.LoadTemplates();
            Assert.IsTrue(templates.Count >= 3);
            CollectionAssert.Contains(templates.Select(t => t.Name).ToList(), "Default");
        }

        [TestMethod]
        public void ConfigManager_Save_And_Load()
        {
            var originalDir = Directory.GetCurrentDirectory();
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            Directory.SetCurrentDirectory(tempDir);

            try
            {
                var cfg = new RedactorConfig { Name = "Test", UseMLModel = true };
                cfg.Patterns.Add("foo");
                ConfigManager.Save(cfg);

                var loaded = ConfigManager.Load();
                Assert.AreEqual("Test", loaded.Name);
                Assert.IsTrue(loaded.UseMLModel);
                Assert.AreEqual(1, loaded.Patterns.Count);
                Assert.AreEqual("foo", loaded.Patterns[0]);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDir);
                Directory.Delete(tempDir, true);
            }
        }

        [TestMethod]
        public void Phone_Number_Is_Redacted()
        {
            var service = new ClipboardService();
            var timerField = typeof(ClipboardService)
                .GetField("timer", BindingFlags.NonPublic | BindingFlags.Instance);
            (timerField?.GetValue(service) as DispatcherTimer)?.Stop();

            var sanitize = typeof(ClipboardService)
                .GetMethod("Sanitize", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(sanitize);

            var input = "call me at 123-456-7890";
            var output = (string)sanitize!.Invoke(service, new object[] { input })!;

            Assert.IsFalse(output.Contains("123-456-7890"));
            Assert.IsTrue(output.Contains("[REDACTED]"));
        }
    }
}
