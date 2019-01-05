using Newtonsoft.Json;

namespace WeatherApi.Models.OpenWeatherMap
{
    public class Wind
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }
        // [JsonProperty("deg")]     
        // public double Degree { get; set; }
    }
}