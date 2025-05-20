using System.IO;
using System.Text.Json;

namespace PIIRedactorApp.Models
{
    public static class ConfigManager
    {
        private const string ConfigFile = "config.json";

        public static void Save(RedactorConfig config)
        {
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigFile, json);
        }

        public static RedactorConfig Load()
        {
            try
            {
                if (File.Exists(ConfigFile))
                {
                    var json = File.ReadAllText(ConfigFile);
                    var cfg = JsonSerializer.Deserialize<RedactorConfig>(json);
                    if (cfg != null)
                    {
                        return cfg;
                    }
                }
            }
            catch
            {
            }
            return TemplateProvider.LoadTemplates()[0];
        }
    }
}
