using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WeatherApi.Implementation.DataLayer;
using WeatherApi.Models;
using WeatherApi.Interfaces;
using WeatherApi.Models.OpenWeatherMap;
using WeatherApi.Models.WeatherApi;

namespace WeatherApi.Implementation.BusinessLayer
{
    /// <summary>
    /// Contains business logic for the weather information
    /// </summary>
    public class OpenWeatherMapService : IWeatherService
    {
        /// <summary>
        /// Object of ICacheService for caching operations
        /// </summary>
        private readonly ICacheService cacheService = null;
        
        /// <summary>
        /// Dependency injection for Object of ICacheService, so we can change from memory cache to anything in the future
        /// </summary>
        public OpenWeatherMapService(ICacheService cacheService) => this.cacheService = cacheService;
        
        /// <summary>
        /// Get t + 5 days weather information for the particular city
        /// </summary>
        /// <param name="cityId">City id</param>
        /// <returns>Returns t + 5 days weather information for the city</returns>
        public WeatherApiResponse GetWeatherDataForCity(string cityId)
        {
            var weatherApiResponse = new WeatherApiResponse();
            try{
                var cachedData = this.cacheService.Get(cityId);
                //Check data is exist in the cache or not
                if(cachedData != null)
                {
                    return cachedData;
                }
                else
                {
                    //Getting data from open weather and converting to our data structure
                    var openWeatherResponse = new OpenWeatherMapApi().GetWeatherDataForCity(cityId);
                    weatherApiResponse.Status = Convert.ToInt32(openWeatherResponse.StatusCode);
                    weatherApiResponse.Message = openWeatherResponse.Message;
                    //Check http response is ok from openweather than only process
                    if(weatherApiResponse.Status == 200){
                        weatherApiResponse.City = openWeatherResponse.City.Name;
                        weatherApiResponse.Country = openWeatherResponse.City.Country;
                        weatherApiResponse.WeatherInformation = 
                        openWeatherResponse.CityWeatherInformation.Select(x =>
                        new WeatherInformation(){
                            EpochTime = x.TempratureTimeInUnix,
                            Time = Utility.UnixTimeStampToDateTime(x.TempratureTimeInUnix),
                            Temprature = x.TempratureInformation.Temprature,
                            Humidity = x.TempratureInformation.Humidity,
                            Wind = x.Wind.Speed
                        }).ToList();
                        this.cacheService.Add(cityId, weatherApiResponse);
                    }
                    return weatherApiResponse;
                }
            }catch{
                weatherApiResponse.Status = 500;
                weatherApiResponse.Message = Utility.SERVER_ERROR;
                return weatherApiResponse;
            }
        }
    }
}