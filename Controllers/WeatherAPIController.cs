﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using WeatherApi.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WeatherApi.Controllers
{
    #region snippet_Controller
    /// <summary>
    ///     Represents Weather controler
    /// </summary>
    [Route("api/[controller]")]
    class WeatherAPIController : ControllerBase
    {
        #endregion

        #region snippet_GetWeather
        /// <summary>
        /// Get current weather for specific city
        /// </summary>
        /// <param name="city"></param> 
        [HttpGet]
        [Route("api/getweather")]
        public async Task<IActionResult> GetWeather(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid=f1fc6c59c426736623261ddb6fb0d5cd&units=metric");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    JObject fullWeather = JObject.Parse(stringResult);
                    Weather weather = new Weather
                    {
                        Date = Convert.ToDateTime(fullWeather["dt"]),
                        Temp = (double)fullWeather["dt"]["temp"],
                        WindSpeed = (double)fullWeather["wind"]["speed"],
                        Clouds = (string)fullWeather["clouds"]["all"]
                    };
                    return Ok(weather);
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }
        #endregion

        #region snippet_GetWeatherForecast
        /// <summary>
        /// Get forecast for specific city
        /// </summary>
        /// <param name="city"></param> 
        [HttpGet]
        [Route("api/getforecast")]
        public async Task<IActionResult> GetForecast(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid=f1fc6c59c426736623261ddb6fb0d5cd&units=metric");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    JObject fullWeather = JObject.Parse(stringResult);
                    List<Weather> weatherForecast = new List<Weather>();
                    foreach (var timePeriod in fullWeather["list"])
                    {
                        Weather weather = new Weather
                        {
                            Date = Convert.ToDateTime(fullWeather["dt"]),
                            Temp = (double)fullWeather["dt"]["temp"],
                            WindSpeed = (double)fullWeather["wind"]["speed"],
                            Clouds = (string)fullWeather["clouds"]["all"]
                        };
                        weatherForecast.Add(weather);
                    }
                    return Ok(weatherForecast);
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }
        #endregion
    }
}