namespace Wonsy.SlashCommands
{
    public class MapSlashCommand : InteractionModuleBase
    {
        private readonly MapCache _mapCache;
        private readonly ZEApi _zeApi;

        public MapSlashCommand(MapCache mapCache, ZEApi zeApi)
        {
            _mapCache = mapCache;
            _zeApi = zeApi;
        }

        [SlashCommand("mapinfo", "Gets the map info of the specified map")]
        public async Task GetMapCooldown([Summary("mapname", "The map you want to search"), Autocomplete(typeof(MapNameAutoCompleter))] string mapName)
        {
            if (!_mapCache.IsMapValid(mapName))
            {
                await RespondAsync($"The map name is invalid: `{mapName}`", ephemeral: true);
                return;
            }

            await Context.Interaction.DeferAsync();
            var result = await _zeApi.GetCooldownAsync(mapName);

            EmbedBuilder embedBuilder = new();
            embedBuilder.WithInformationColor();
            embedBuilder.WithDescription($"**{result.Map}**\n\n" +
                $"Cooldown: `{(result.Cooldown == "-1" ? "Not on cooldown." : $"{result.Cooldown}")} map{(int.Parse(result.Cooldown) > 1 ? "s" : "")}`\n" +
                $"Size: `{_mapCache.GetMap(result.Map)?.FileSize.ToReadableString() ?? "Unable to get map size"}`\n" +
                $"Download Link: `To be implemented.`");
            embedBuilder.WithRequestedByFooter(Context);

            await FollowupAsync(embed: embedBuilder.Build());
        }
    }
}
