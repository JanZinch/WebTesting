using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;



using NUnit.Framework;




namespace WebTesting
{
    [TestFixture]
    internal class MetaWeather
    {

        private const string Reference = "https://www.metaweather.com/api/";

        private HttpClient _httpClient = new HttpClient();

        private const string City = "Minsk";

        

        [SetUp]
        public void Initialize() {

            

        }

        [Test]
        public async Task Search() {

            HttpResponseMessage response = await _httpClient.GetAsync(Reference + "location/search/?query=min");
            response.EnsureSuccessStatusCode();
            Stream jsonNotation = await response.Content.ReadAsStreamAsync();

            Location[] locations = await JsonSerializer.DeserializeAsync<Location[]>(jsonNotation);

            foreach (Location location in locations) {

                if (location.Name == City) {

                    Console.WriteLine(location);

                }
            }
        }



    }
}
