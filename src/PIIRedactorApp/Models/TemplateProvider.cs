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
                        "(?i)(password|secret|api[_-]?key)\\s*[:=]\\s*\\S+",
                        "\\b\\d{3}-\\d{2}-\\d{4}\\b",
                        "\\b(?:\\d[ -]*?){13,16}\\b",
                        "(?i)\b(?:\+?1[-.\s]?)?(?:\(\d{3}\)|\d{3})[-.\s]?\d{3}[-.\s]?\d{4}\b"
                    }
                },
                new RedactorConfig
                {
                    Name = "Minimal",
                    Patterns = new List<string>
                    {
                        "(?i)[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,}"
                    }
                },
                new RedactorConfig
                {
                    Name = "Extended",
                    Patterns = new List<string>
                    {
                        "(?i)[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,}",
                        "\\b\\d{3}-\\d{2}-\\d{4}\\b",
                        "\\b(?:\\d[ -]*?){13,16}\\b"
                    }
                }
            };
        }
    }
}
