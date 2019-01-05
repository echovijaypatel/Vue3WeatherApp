using System; 
using System.Collections.Generic; 
using WeatherApi.Models.OpenWeatherMap; 

namespace WeatherApi.Interfaces {

/// <summary>
/// Contains business logic for the city information
/// </summary>
public interface ICityService {
/// <summary>
/// Search city by name
/// </summary>
/// <returns>Returns city object</returns>
IEnumerable < City > SearchCityByName(string name,int maxResult = 10); 
}
}