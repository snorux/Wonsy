using Newtonsoft.Json;

namespace Wonsy.Services
{
    public class ZEApi
    {
        private readonly Configuration _config;
        private readonly HttpClient _httpClient;

        public ZEApi(IOptions<Configuration> config, HttpClient httpClient) 
        {
            _config = config.Value;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(_config.ZEApi.APIWebsite);
        }

        private async Task<string> CallApiAsync(string endpoint)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add(_config.ZEApi.AuthorizationHeader, _config.ZEApi.AuthorizationToken);

            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<TimeleftModel> GetTimeleftAsync()
        {
            var responseData = await CallApiAsync("GetTimeleft");
            return JsonConvert.DeserializeObject<TimeleftModel>(responseData);
        }

        public async Task<CooldownModel> GetCooldownAsync(string mapName)
        {
            var responseData = await CallApiAsync($"GetMapCooldown?map={mapName}");
            return JsonConvert.DeserializeObject<CooldownModel>(responseData);
        }
    }
}
