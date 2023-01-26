using System.Reflection;

namespace Wonsy.Services
{
    public class Startup
    {
        private readonly DiscordSocketClient _client;
        private readonly Configuration _config;
        private readonly InteractionService _interactionService;
        private readonly IServiceProvider _serviceProvider;

        public Startup(DiscordSocketClient client, IOptions<Configuration> config, InteractionService interactionService, IServiceProvider serviceProvider)
        {
            _client = client;
            _config = config.Value;
            _interactionService = interactionService;
            _serviceProvider = serviceProvider;
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

            Log.Information("Adding slash commands");
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);

            Log.Information("Starting...");
            await _client.StartAsync();

            _client.Ready += ReadyAsync;
        }

        private async Task ReadyAsync()
        {
            if (_config.BotConfig.DevMode)
                foreach (var server in _config.BotConfig.DevServers)
                    await _interactionService.RegisterCommandsToGuildAsync(server, true);
            else
                await _interactionService.RegisterCommandsGloballyAsync(true);
        }
    }
}
