﻿using Newtonsoft.Json;

namespace Wonsy.Models
{
    public class Configuration
    {
        public string LogLevel { get; set; }

        public BotConfiguration BotConfig { get; set; }

        public ZEApiConfiguration ZEApi { get; set; }

        public static void CheckConfig()
        {
            var configFolder = Path.Combine(Directory.GetCurrentDirectory(), "Configs");
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);

            if (!File.Exists(Path.Combine(configFolder, "config.json")))
            {
                Configuration config = new()
                {
                    LogLevel = "verbose",
                    BotConfig = new BotConfiguration()
                    {
                        BotToken = "CHANGE-ME",
                        BotOwners = new List<ulong>(),
                        BotGameStatus = "CHANGE-ME",
                        BotStatus = 4,
                        ActivityType = 3,
                        DevMode = true,
                        DevServers = new List<ulong>(),
                    },
                    ZEApi = new ZEApiConfiguration()
                };

                File.WriteAllText(Path.Combine(configFolder, "config.json"), JsonConvert.SerializeObject(config, Formatting.Indented));

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Created new bot configuration file with default values.\n" +
                              $"Set your secrets in {Path.Combine(configFolder, "config.json")} before running the bot again.\n\n" +
                              "Exiting in 10 seconds...");
                Console.ResetColor();

                Thread.Sleep(10000);
                Environment.Exit(0);
            }
            else
                _ = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Path.Combine(configFolder, "config.json")));
        }
    }

    public class BotConfiguration
    {
        public string BotToken { get; set; }

        public List<ulong> BotOwners { get; set; }
        
        public string BotGameStatus { get; set; }

        public int BotStatus { get; set; }

        public int ActivityType { get; set; }

        public bool DevMode { get; set; }

        public List<ulong> DevServers { get; set; }
    }

    public class ZEApiConfiguration
    {
        public string APIWebsite { get; set; }

        public string AuthorizationHeader { get; set; }

        public string AuthorizationToken { get; set; }

        public string MapListUrl { get; set; }

        public string FastDLUrl { get; set; }
    }
}
