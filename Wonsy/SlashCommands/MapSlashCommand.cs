namespace Wonsy.SlashCommands
{
    public class MapSlashCommand : InteractionModuleBase
    {
        private readonly MapCache _mapCache;
        private readonly ZEApi _zeApi;
        private readonly Configuration _config;

        public MapSlashCommand(MapCache mapCache, ZEApi zeApi, IOptions<Configuration> config)
        {
            _mapCache = mapCache;
            _zeApi = zeApi;
            _config = config.Value;
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
            var cachedMap = _mapCache.GetMap(result.MapName);

            var size = cachedMap == null ? "Unable to get map size" : cachedMap.FileSize.ToReadableString();
            var fastdlLink = cachedMap == null ? "Unable to get fastdl link" : 
                cachedMap.IsMoreThan150MB ? $"{_config.ZEApi.FastDLUrl}{result.MapName}.bsp" : $"{_config.ZEApi.FastDLUrl}{result.MapName}.bsp.bz2";

            EmbedBuilder embedBuilder = new();
            embedBuilder.WithInformationColor();
            embedBuilder.WithDescription($"**{result.MapName}**\n\n" +
                $"Cooldown: `{(result.Cooldown == "-1" ? "Not on cooldown" : $"{result.Cooldown} map{(int.Parse(result.Cooldown) > 1 ? "s" : "")}")}`\n" +
                $"Size: `{size}`\n" +
                $"Download Link: [FastDL Link]({fastdlLink})");
            embedBuilder.WithRequestedByFooter(Context);

            await FollowupAsync(embed: embedBuilder.Build());
        }
    }
}
