namespace Wonsy.SlashCommands
{
    public class InfoCommand : InteractionModuleBase
    {
        [SlashCommand("about", "Displays information about this bot")]
        public async Task AboutCommand()
            => await GetInformation();

        [SlashCommand("info", "Displays information about this bot")]
        public async Task GetInformation()
            => await RespondAsync(text: "ping!");
    }
}
 