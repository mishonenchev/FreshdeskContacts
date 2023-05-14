using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshdeskContacts.Models.Dtos
{
    public class FreshdeskContactDTO
    {
        [JsonProperty("id")]
        public string FreshdeskId { get; set; }

        public bool ShouldSerializeFreshdeskId()
        {
            // Exclude FreshdeskId property if it is 0
            return !string.IsNullOrEmpty(FreshdeskId);
        }

        [JsonProperty("job_title")]
        public string Login { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string HtmlUrl { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
