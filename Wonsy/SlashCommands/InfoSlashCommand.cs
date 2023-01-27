using System.Reflection;
using Wonsy.Extensions;

namespace Wonsy.SlashCommands
{
    public class InfoSlashCommand : InteractionModuleBase
    {
        [SlashCommand("info", "Displays information about this bot")]
        public async Task GetInformation()
        {
            string netVersion = Environment.Version.ToString();
            string discordNetVersion = DiscordConfig.Version;
            string botVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            string commitHash = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var timeSpan = DateTime.Now - currentProcess.StartTime;
            string upTime = $"{timeSpan.Days}d {timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s";
            var currentMemUsage = currentProcess.WorkingSet64;

            var client = (DiscordSocketClient)Context.Client;

            EmbedBuilder embedBuilder = new();
            embedBuilder.WithColor(Constants.InformationColor);
            embedBuilder.WithDescription($"Wonsy is a discord bot made by {MentionUtils.MentionUser(107389572958158848)} for use in [GFL ZE Discord](https://discord.gg/gflze). The source code is available on [GitHub](https://github.com/snooooowy/Wonsy)\n\n" +
                $"Commit Hash: `{commitHash}`\n" +
                $"User Count: `{client.Guilds.Select(x => x.MemberCount).Sum()}`\n" +
                $"Guilds Count: `{client.Guilds.Count}`\n" +
                $"Memory Usage: `{currentMemUsage.ToReadableString()}`\n\n" +
                "Invite link coming soon™");
            embedBuilder.AddField("Bot Version:", $"{Emote.Parse("<:information:1068561454107730000>")} **{botVersion}**", true);
            embedBuilder.AddField(".NET Version:", $"{Emote.Parse("<:dotnet:1068561542460739697>")} **{netVersion}**", true);
            embedBuilder.AddField("Discord.Net Version:", $"{Emote.Parse("<:discordnet:1068561206215979112>")} **{discordNetVersion}**", true);
            embedBuilder.WithFooter(footer => footer.Text = $"Uptime: {upTime}");
            embedBuilder.WithCurrentTimestamp();

            await RespondAsync(embed: embedBuilder.Build());
        }
    }
}
 