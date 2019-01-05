using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Net; 
using System.Net.Http; 
using System.Text; 
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Mvc; 
using WeatherApi.Implementation; 
using WeatherApi.Implementation.BusinessLayer; 
using WeatherApi.Interfaces; 
using WeatherApi.Models; 
using WeatherApi.Models.OpenWeatherMap; 
using WeatherApi.Models.WeatherApi; 

namespace WeatherApi.Controllers {
    /// <summary>
    /// Contains api for city information
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CityController:ControllerBase {
        /// <summary>
        /// Object of ICityService to fetch city information
        /// </summary>
        private readonly ICityService cityService = null; 

        /// <summary>
        /// Dependency injection for Object of ICityService service, so we can search from list of cities
        /// </summary>
        public CityController(ICityService cityService) => this.cityService = cityService; 

        /// <summary>
        /// Get city information
        /// </summary>
        /// <param name="name">City name to be search</param>
        /// <returns>City information</returns>
        [HttpGet("{name}")]
        public IActionResult Get(string name) {
            try {
                //Validating the input
                if (string.IsNullOrEmpty(name))return NotFound(Utility.SERVER_NOTFOUND); 

                //Search for city
                var city = cityService.SearchCityByName(name); 
                if (city != null) {
                    return Ok(city); 
                }else {
                    return BadRequest(); 
                }
            }catch {
                return StatusCode((int)HttpStatusCode.InternalServerError, Utility.SERVER_ERROR); 
            }
        }
    }
}
