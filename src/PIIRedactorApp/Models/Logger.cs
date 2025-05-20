using System;
using System.IO;

namespace PIIRedactorApp.Models
{
    public static class Logger
    {
        private static readonly string LogFile = "log.txt";

        public static void Log(string message)
        {
            try
            {
                File.AppendAllText(LogFile, $"{DateTime.Now:u} {message}{Environment.NewLine}");
            }
            catch
            {
                // ignore logging failures
            }
        }
    }
}
