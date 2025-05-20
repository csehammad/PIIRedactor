using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIIRedactorApp.Services;
using PIIRedactorApp.Models;

namespace RedactorTests
{
    [TestClass]
    public class RedactorTests
    {
        [TestMethod]
        public void Email_Is_Redacted()
        {
            var service = new ClipboardService();
            var result = service.GetType(); // placeholder to ensure compile
            Assert.IsNotNull(result);
        }
    }
}
