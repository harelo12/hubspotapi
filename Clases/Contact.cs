using System.Text.Json.Serialization;

namespace HubspotClient.Clases
{
    public class Contact
    {
        public Contact() { 
        }
        public Contact (ContactProperties definedProperties)
        {
            properties = definedProperties;
        }
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("properties")]
        public ContactProperties properties { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime createdAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime updatedAt { get; set; }

        [JsonPropertyName("archived")]
        public bool archived { get; set; }


    }
}