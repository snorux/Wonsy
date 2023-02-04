using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Wonsy
{
    public class Wonsy
    {
        private readonly IConfiguration _config;

        private readonly DiscordSocketConfig _socketConfig = new()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged,
            AlwaysDownloadUsers = true,
        };

        public Wonsy()
        {
            // Create default config file if it doesn't exist:
            Configuration.CheckConfig();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory, "Configs"))
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .Build();

            this._config = configuration;

            if (string.IsNullOrEmpty(_config.GetSection("BotConfig:BotToken").Value))
                throw new ArgumentNullException("The bot token cannot be found in config.json! Please check and make sure it's there!");
        }
        public async Task RunAsync()
        {
            var logLevel = _config.GetSection("LogLevel").Value switch
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
            services.GetRequiredService<InteractionHandler>();

            await services.GetRequiredService<Startup>().InitializeAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private ServiceProvider ConfigureServices(DiscordSocketConfig config)
        {
            var services = new ServiceCollection()
                .Configure<Configuration>(_config)
                .AddHttpClient()
                .AddSingleton(new DiscordSocketClient(config))
                .AddSingleton<Startup>()
                .AddSingleton<Logging>()
                .AddSingleton<InteractionHandler>()
                .AddSingleton<MapCache>()
                .AddSingleton<ZEApi>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));

            return services.BuildServiceProvider();
        }
    }
}
