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

        public float Latitude { get; set; }
        public float Longitude { get; set; }
           
    }

    public class LatLongConverter : JsonConverter<LatLong>
    {

        private NumberFormatInfo _numberFormat = new NumberFormatInfo() { NumberDecimalSeparator = "." };

        public override LatLong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            LatLong result = new LatLong();
            string jsonData = reader.GetString();
            string buf = string.Empty;

            for (int i = 0; i < jsonData.Length; i++)
            {
                if (jsonData[i] == ',')
                {
                    result.Latitude = Convert.ToSingle(buf, _numberFormat);

                    buf = string.Empty;

                    for (int j = i + 1; j < jsonData.Length; j++)
                    {
                        buf += jsonData[j];
                    }

                    result.Longitude = Convert.ToSingle(buf, _numberFormat);

                    break;
                }

                buf += jsonData[i];
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, LatLong value, JsonSerializerOptions options)
        {
            writer.WriteStringValue("null");
        }
    }


}
