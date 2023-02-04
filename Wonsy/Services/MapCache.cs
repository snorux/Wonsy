using Newtonsoft.Json;

namespace Wonsy.Services
{
    public class MapCache
    {
        private readonly HttpClient _httpClient;
        private readonly Configuration _config;
        private Timer _timer;

        private List<MapModel> _maps;

        public MapCache(IOptions<Configuration> config, HttpClient httpClient)
        {
            _config = config.Value;
            _httpClient = httpClient;

            Log.Information("Starting timer for map cache");
            _timer = new Timer(GetMapList, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
        }

        private async void GetMapList(object state)
        {
            Log.Information("Performing request to map list url");

            var response = await _httpClient.GetAsync(_config.ZEApi.MapListUrl);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var retrievedMaps = JsonConvert.DeserializeObject<List<MapModel>>(result);

            if (_maps?.SequenceEqual(retrievedMaps) ?? false)
            {
                Log.Debug("Current map list is equal to retreived maps, not updating cache");
                return;
            }

            _maps = retrievedMaps;
            Log.Information($"Retrieved {_maps.Count} maps");
        }

        public List<string> GetSimilarMapName(string input)
            => _maps?.Where(x => x.MapName.Contains(input, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(y => y.MapName[3..].StartsWith(input, StringComparison.OrdinalIgnoreCase))
                .Take(25)
                .Select(z => z.MapName)
                .ToList() ?? new List<string>() { "Not cached, please try again later" };

        public bool IsMapValid(string mapName)
            => _maps?.Exists(x => x.MapName == mapName) ?? false;

        public MapModel GetMap(string mapName)
            => _maps?.Find(x => x.MapName == mapName);
    }
}
