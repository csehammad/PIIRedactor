using System.Collections.Generic;

namespace PIIRedactorApp.Models
{
    public static class TemplateProvider
    {
        public static List<RedactorConfig> LoadTemplates()
        {
            return new List<RedactorConfig>
            {
                new RedactorConfig
                {
                    Name = "Default",
                    Patterns = new List<string>
                    {
                        "(?i)[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,}",
                        "(?i)bearer\\s+[a-z0-9-_]+",
                        "(?i)(password|secret|api[_-]?key)\\s*[:=]\\s*\\S+"
                    }
                },
                new RedactorConfig
                {
                    Name = "Minimal",
                    Patterns = new List<string>
                    {
                        "(?i)[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,}"
                    }
                }
            };
        }
    }
}
