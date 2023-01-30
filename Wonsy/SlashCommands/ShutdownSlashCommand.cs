namespace Wonsy.SlashCommands
{
    public class ShutdownSlashCommand : InteractionModuleBase
    {
        [BotOwnerOnly]
        [SlashCommand("shutdown", "Shutdown the bot. Require bot owner permissions.")]
        public async Task Shutdown()
        {
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var timeSpan = DateTime.Now - currentProcess.StartTime;
            string upTime = $"{timeSpan.Days}d {timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s";
            var peakMemUsage = currentProcess.PeakWorkingSet64;

            EmbedBuilder embedBuilder = new();
            embedBuilder.WithInformationColor();
            embedBuilder.WithDescription($"Shutting down the bot.\n\n" +
                $"Uptime was `{upTime}`\n" +
                $"Peak memory usage was `{peakMemUsage.ToReadableString()}`");
            embedBuilder.WithRequestedByFooter(Context);

            await RespondAsync(embed: embedBuilder.Build());

            // Log before disconnecting
            Context.LogCommandUsed();

            await Context.Client.StopAsync();
            Environment.Exit(0);
        }
    }
}
