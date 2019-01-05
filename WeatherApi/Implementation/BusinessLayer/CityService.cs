using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using WeatherApi.Implementation.DataLayer;
using WeatherApi.Interfaces;
using WeatherApi.Models;
using WeatherApi.Models.OpenWeatherMap;
using WeatherApi.Models.WeatherApi;

namespace WeatherApi.Implementation.BusinessLayer
{
    /// <summary>
    /// Contains business logic for the city information
    /// </summary>
    public class CityService : ICityService
    {
        //Local cities structure to search city data
        private readonly IEnumerable<City> cities = null;

        //This will only called once as we are creating singleton service
        public CityService()
        {
            //Reading cities json data from the file
            using (FileStream fs = new FileStream(@"..\WeatherApi\App_Data\city.list.json", FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    cities = serializer.Deserialize<List<City>>(reader);
                }
            }
        }

        /// <summary>
        /// Search city by name
        /// </summary>
        /// <returns>Returns city object</returns>
        public IEnumerable<City> SearchCityByName(string name, int maxResult = 10)
        {
            return cities.Where(x => x.Name.ToLower().Contains(name.ToLower())).Take(maxResult);
        }
    }
}