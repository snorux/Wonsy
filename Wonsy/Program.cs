using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Wonsy
{
    internal class Wonsy
    {
        private readonly Configuration _config;

        private readonly DiscordSocketConfig _socketConfig = new()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged,
            AlwaysDownloadUsers = true,
        };

        public Wonsy()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .Build();

            _config = configuration.Get<Configuration>();
            _config.DevServers = configuration.GetSection("DevServers").Get<List<ulong>>();

            if (string.IsNullOrEmpty(_config.BotToken))
                throw new ArgumentNullException("The bot token cannot be found in config.json! Please check and make sure it's there!");
        }

        static void Main(string[] args)
            => new Wonsy().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var logLevel = _config.LogLevel switch
            {
                "verbose" => Serilog.Events.LogEventLevel.Verbose,
                "debug" => Serilog.Events.LogEventLevel.Debug,
                "info" => Serilog.Events.LogEventLevel.Information,
                "warn" => Serilog.Events.LogEventLevel.Warning,
                "error" => Serilog.Events.LogEventLevel.Error,
                "fatal" => Serilog.Events.LogEventLevel.Fatal,
                _ => throw new NotImplementedException()
            };

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(logLevel)
                .WriteTo.File("Logs/WonsyLog-.log", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            using var services = ConfigureServices(_socketConfig);

            services.GetRequiredService<Logging>();

            var client = services.GetRequiredService<DiscordSocketClient>();

            await client.LoginAsync(TokenType.Bot, _config.BotToken);
            await client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private ServiceProvider ConfigureServices(DiscordSocketConfig config)
        {
            var services = new ServiceCollection()
                .AddSingleton(_socketConfig)
                .AddSingleton(_config)
                .AddSingleton(new DiscordSocketClient(config))
                .AddSingleton<Logging>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));

            return services.BuildServiceProvider();
        }
    }
}