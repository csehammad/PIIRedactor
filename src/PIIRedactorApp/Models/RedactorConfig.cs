using System.Collections.Generic;

namespace PIIRedactorApp.Models
{
    public class RedactorConfig
    {
        public string Name { get; set; } = "Default";
        public List<string> Patterns { get; set; } = new();
        public bool UseMLModel { get; set; }
    }
}
