
using Newtonsoft.Json;

namespace WeatherApi.Models.OpenWeatherMap
{
    public class TempratureInformation
    {
        [JsonProperty("temp")]
        public double Temprature { get; set; }

        // [JsonProperty("temp_min")]
        // public double MinTemprature { get; set; }

        // [JsonProperty("temp_max")]
        // public double MaxTemprature { get; set; }

        // [JsonProperty("pressure")]
        // public double Pressure { get; set; }

        // [JsonProperty("sea_level")]
        // public double SeaLevel { get; set; }

        // [JsonProperty("grnd_level")]
        // public double GroundLevel { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        // [JsonProperty("temp_kf")]
        // public double temp_kf { get; set; }
    }
}