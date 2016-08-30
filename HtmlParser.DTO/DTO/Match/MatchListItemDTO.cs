using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.DTO.Match
{
    public class MatchListItemDTO
    {
        [JsonProperty("unique_id")]
        public string Id { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("title")]
        public string Name { get; set; }
    }

    public class RootObject
    {
        [JsonProperty("data")]
        public List<MatchListItemDTO> Items { get; set; }
    }
}
