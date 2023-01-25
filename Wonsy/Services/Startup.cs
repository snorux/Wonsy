namespace Wonsy.Services
{
    internal class Startup
    {
        private readonly DiscordSocketClient _client;
        private readonly Configuration _config;

        public Startup(DiscordSocketClient client, IOptions<Configuration> config)
        {
            _client = client;
            _config = config.Value;
        }

        public async Task InitializeAsync()
        {
            Log.Information("Logging into discord");
            await _client.LoginAsync(TokenType.Bot, _config.BotConfig.BotToken);

            Log.Information("Setting status");
            await _client.SetStatusAsync((UserStatus)_config.BotConfig.BotStatus);

            if (!string.IsNullOrWhiteSpace(_config.BotConfig.BotGameStatus)) 
            {
                Log.Information("Setting custom status");
                await _client.SetGameAsync(_config.BotConfig.BotGameStatus, type: (ActivityType)_config.BotConfig.ActivityType);
            }

            Log.Information("Starting...");
            await _client.StartAsync();
        }
    }
}
