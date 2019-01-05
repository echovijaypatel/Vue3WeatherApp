using Newtonsoft.Json;

namespace WeatherApi.Models.OpenWeatherMap
{
    public class City
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        // public Coord coord { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
    }
}