namespace Wonsy.SlashCommands
{
    public class TimeleftSlashCommand : InteractionModuleBase
    {
        private readonly ZEApi _zeApi;

        public TimeleftSlashCommand(ZEApi zeApi) 
        {
            _zeApi = zeApi;
        }

        [SlashCommand("timeleft", "Gets timeleft and additional info for the current map.")]
        public async Task GetTimeleftInformation()
        {
            await Context.Interaction.DeferAsync();
            var result = await _zeApi.GetTimeleftAsync();

            EmbedBuilder embedBuilder = new();
            embedBuilder.WithInformationColor();
            embedBuilder.WithDescription($"Current information for \'{result.CurrentMap}\'\n");
            embedBuilder.AddField("Timeleft", result.Timeleft, true);
            embedBuilder.AddField("Extends Used", $"{result.ExtendsLeft} / {result.TotalExtends}", true);
            embedBuilder.AddField("Nextmap", result.NextMap, true);
            embedBuilder.WithRequestedByFooter(Context);

            await FollowupAsync(embed: embedBuilder.Build());
        }
    }
}
