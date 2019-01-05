using System.Collections.Generic;
using Newtonsoft.Json;

//Generate using http://json2csharp.com/#
namespace WeatherApi.Models.OpenWeatherMap
{
    public class OpenWeatherResponse
    {
        [JsonProperty("cod")]
        public string StatusCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("cnt")]
        public int Count { get; set; }
        
        [JsonProperty("list")]
        public List<CityWeather> CityWeatherInformation { get; set; }

        [JsonProperty("city")]
        public City City { get; set; }
    }
    
    // public class Weather
    // {
    //     public int id { get; set; }
    //     public string main { get; set; }
    //     public string description { get; set; }
    //     public string icon { get; set; }
    // }

    // public class Clouds
    // {
    //     public int all { get; set; }
    // }

    // public class Snow
    // {
    //     public double __invalid_name__3h { get; set; }
    // }

    // public class Sys
    // {
    //     public string pod { get; set; }
    // }

    // public class Coord
    // {
    //     public double lat { get; set; }
    //     public double lon { get; set; }
    // }
}