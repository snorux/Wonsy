﻿namespace Wonsy.Models
{
    public class Configuration
    {
        public string LogLevel { get; set; }

        public BotConfiguration BotConfig { get; set; }
    }

    public class BotConfiguration
    {
        public string BotToken { get; set; }
        
        public string BotGameStatus { get; set; }

        public int BotStatus { get; set; }

        public int ActivityType { get; set; }

        public bool DevMode { get; set; }

        public List<ulong> DevServers { get; set; }
    }
}
