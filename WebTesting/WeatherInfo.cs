using System;

using Newtonsoft.Json;

namespace WebTesting
{
    struct WeatherInfo
    {

        [JsonProperty("weather_state_name")]
        public string WeatherStateName { get; set; }

        [JsonProperty("applicable_date")]
        public DateTime ApplicableDate { get; set; }

        [JsonProperty("min_temp")]
        public float MinTemp { get; set; }


        [JsonProperty("max_temp")]
        public float MaxTemp { get; set; }

        [JsonProperty("the_temp")]
        public float CurrentTemp { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        [JsonProperty("wind_speed")]
        public double WindSpeed { get; set; }


        public bool CheckTemperature(Season season)
        {
            switch (season)
            {
                case Season.SUMMER:

                    return CurrentTemp > 0.0f;
                   
                case Season.WINTER:

                    return CurrentTemp < 0.0f;

                case Season.SPRING:
                case Season.AUTUMN:

                    return CurrentTemp <= MaxTemp && CurrentTemp >= MinTemp;

                default:

                    return false;
            }
        }

        public override string ToString()
        {
            return string.Format("\nWeatherState: {0} \nDate: {1:d} \nMinTemp: {2} \nMaxTemp: {3} \nCurrentTemp: {4} \nHumidity: {5} \nWindSpeed: {6:f4}",
                WeatherStateName, ApplicableDate, MinTemp, MaxTemp, CurrentTemp, Humidity, WindSpeed);
        }

    }

    public enum Season : byte { 
    
        SUMMER, WINTER, SPRING, AUTUMN
    }


}
