namespace Wonsy.Services
{
    public class Logging
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _interaction;

        public Logging(DiscordSocketClient client, InteractionService interaction)
        {
            _client = client;
            _interaction = interaction;

            _client.Log += LogAsync;
            _interaction.Log += LogAsync;
        }

        private static async Task LogAsync(LogMessage message)
        {
            var severity = message.Severity switch
            {
                LogSeverity.Verbose => Serilog.Events.LogEventLevel.Verbose,
                LogSeverity.Debug => Serilog.Events.LogEventLevel.Debug,
                LogSeverity.Info => Serilog.Events.LogEventLevel.Information,
                LogSeverity.Warning => Serilog.Events.LogEventLevel.Warning,
                LogSeverity.Error => Serilog.Events.LogEventLevel.Error,
                LogSeverity.Critical => Serilog.Events.LogEventLevel.Fatal,
                _ => Serilog.Events.LogEventLevel.Information
            };

            Log.Write(severity, message.Message);
            await Task.CompletedTask;
        }
    }
}
