using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace WebTesting
{
    public struct Location
    {
        [JsonPropertyName("title")]
        public string Name { get; set; }

        [JsonPropertyName("location_type")]
        public string LocationType { get; set; }

        [JsonPropertyName("woeid")]
        public int Id { get; set; }


        [JsonPropertyName("latt_long")]
        [JsonConverter(typeof(LatLongConverter))]
        public LatLong LatLongData { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}, Location type: {1}, Id: {2}, Latitude: {3}, Longitude: {4}",
                Name, LocationType, Id, LatLongData.Latitude, LatLongData.Longitude);
        }

    }

    public struct LatLong {

        public double Latitude { get; set; }
        public double Longitude { get; set; }
           
    }

    public class LatLongConverter : JsonConverter<LatLong>
    {

        private NumberFormatInfo _numberFormat = new NumberFormatInfo() { NumberDecimalSeparator = "." };

        public override LatLong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            LatLong result = new LatLong();

            string jsonNotation = reader.GetString();
            string buf = string.Empty;

            Console.WriteLine("String: " + jsonNotation);

            for (int i = 0; i < jsonNotation.Length; i++)
            {

                if (jsonNotation[i] == ',')
                {

                    result.Latitude = Convert.ToDouble(buf, _numberFormat);

                    buf = string.Empty;

                    for (int j = i + 1; j < jsonNotation.Length; j++)
                    {

                        buf += jsonNotation[j];
                    }

                    result.Longitude = Convert.ToDouble(buf, _numberFormat);

                }

                buf += jsonNotation[i];

            }




            return result;
        }

        public override void Write(Utf8JsonWriter writer, LatLong value, JsonSerializerOptions options)
        {
            writer.WriteStringValue("null");
        }
    }


}
