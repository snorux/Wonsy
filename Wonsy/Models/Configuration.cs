namespace Wonsy.Models
{
    internal class Configuration
    {
        public string BotToken { get; set; }

        public string LogLevel { get; set; }

        public bool DevMode { get; set; }

        public List<ulong> DevServers { get; set; }
    }
}
