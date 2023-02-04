namespace Wonsy.AutoCompleters
{
    public class MapNameAutoCompleter : AutocompleteHandler
    {
        private readonly MapCache _mapCache;

        public MapNameAutoCompleter(MapCache mapCache) 
        {
            _mapCache = mapCache;
        }

        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            List<string> suggestions = new();

            if (autocompleteInteraction?.Data?.Current?.Value == null || string.IsNullOrWhiteSpace(autocompleteInteraction?.Data?.Current?.Value.ToString()))
            {
                suggestions.Add("Start typing to search through map names...");
                return await Task.FromResult(AutocompletionResult.FromSuccess(suggestions.Select(x => new AutocompleteResult(x, x))));
            }

            suggestions = _mapCache.GetSimilarMapName(autocompleteInteraction.Data.Current.Value.ToString());
            return await Task.FromResult(AutocompletionResult.FromSuccess(suggestions.Select(x => new AutocompleteResult(x, x))));
        }
    }
}
