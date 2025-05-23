using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HubspotClient.Clases
{
    public class Contacts
    {
        [JsonPropertyName("results")]
        public Contact[] contactResults { get; set; }

        [JsonPropertyName("paging")]
        public Paging? paging { get; set; }
    }

    public class Paging
    {
        public Next next { get; set; }
    }

    public class Next
    {
        public string after { get; set; }
        public string link { get; set; }
    }
}