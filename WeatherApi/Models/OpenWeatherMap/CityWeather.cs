using Newtonsoft.Json;

namespace WeatherApi.Models.OpenWeatherMap
{
    public class CityWeather
    {
        [JsonProperty("dt")]
        public int TempratureTimeInUnix { get; set; }
        
        [JsonProperty("main")]
        public TempratureInformation TempratureInformation { get; set; }
        // public List<Weather> weather { get; set; }
        // public Clouds clouds { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }
        // public Snow snow { get; set; }
        // public Sys sys { get; set; }

        [JsonProperty("dt_txt")]
        public string TempratureTimeInText { get; set; }
    }
}