using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Wonsy
{
    internal class Wonsy
    {
        private readonly IConfiguration Configuration;

        private readonly DiscordSocketConfig _socketConfig = new()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged,
            AlwaysDownloadUsers = true,
        };

        public Wonsy()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory, "Configs"))
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .Build();

            this.Configuration = configuration;

            if (string.IsNullOrEmpty(Configuration.GetSection("BotToken").Value))
                throw new ArgumentNullException("The bot token cannot be found in config.json! Please check and make sure it's there!");
        }

        static void Main(string[] args)
            => new Wonsy().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var logLevel = Configuration.GetSection("LogLevel").Value switch
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

            var client = services.GetRequiredService<DiscordSocketClient>();


            await client.LoginAsync(TokenType.Bot, Configuration.GetSection("BotToken").Value);
            await client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private ServiceProvider ConfigureServices(DiscordSocketConfig config)
        {
            var services = new ServiceCollection()
                .Configure<Configuration>(this.Configuration)
                .AddSingleton(_socketConfig)
                .AddSingleton(new DiscordSocketClient(config))
                .AddSingleton<Logging>()
                .AddSingleton<InteractionHandler>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));

            return services.BuildServiceProvider();
        }
    }
}