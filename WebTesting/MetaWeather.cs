using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

using Newtonsoft.Json.Linq;

using NUnit.Framework;




namespace WebTesting
{
    [TestFixture]
    internal class MetaWeather
    {
        private const string Reference = "https://www.metaweather.com/api/";
        private HttpClient _httpClient = new HttpClient();
        private const string CurrentCityName = "Minsk";

        [SetUp]
        public void Initialize()
        {



        }

        public string GetFormattedDay(int offset)
        {
            string today = DateTime.Today.AddYears(-offset).ToString("yyyy-MM-dd");                        
            return today.Replace('-', '/');
        }


        [Test]
        public async Task Search()
        {
            Location currentCity = new Location();
            WeatherInfo currentDay = new WeatherInfo();


            HttpResponseMessage response = await _httpClient.GetAsync(Reference + "location/search/?query=min");
            response.EnsureSuccessStatusCode();
            Stream jsonStream = await response.Content.ReadAsStreamAsync();

            Location[] locations = await JsonSerializer.DeserializeAsync<Location[]>(jsonStream);            

            foreach (Location location in locations)
            {
                if (location.Name == CurrentCityName)
                {
                    currentCity = location;
                    Console.WriteLine(location);
                    break;
                }
            }


            response = await _httpClient.GetAsync(Reference + "location/" + currentCity.Id + "/");
            response.EnsureSuccessStatusCode();
            string jsonNotation = await response.Content.ReadAsStringAsync();

            JObject mainObject = JObject.Parse(jsonNotation);
            JArray consolidatedWeather = mainObject["consolidated_weather"] as JArray;

            WeatherInfo[] days = consolidatedWeather.ToObject<WeatherInfo[]>();

            foreach (WeatherInfo day in days)
            {
                Console.WriteLine(day);

                if (!day.CheckTemperature(Season.SUMMER))
                {
                    throw new ArgumentOutOfRangeException(nameof(day.CurrentTemp), "Incorrect temperature.");
                }                
            }

            currentDay = days[0];


            response = await _httpClient.GetAsync(Reference + "location/" + currentCity.Id + "/" + GetFormattedDay(3) + "/");
            response.EnsureSuccessStatusCode();
            jsonNotation = await response.Content.ReadAsStringAsync();

            consolidatedWeather = JArray.Parse(jsonNotation);
            days = consolidatedWeather.ToObject<WeatherInfo[]>();

            bool coincidence = false;

            foreach (WeatherInfo day in days)
            {                
                if (day.WeatherStateName == currentDay.WeatherStateName)
                {
                    Console.WriteLine(day);
                    coincidence = true;
                    break;
                }                
            }

            if (!coincidence) throw new Exception("Ни одно значение не соответствует сегодняшнему дню.");
        
        }

    }
}