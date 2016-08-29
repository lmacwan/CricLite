using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.DTO.Match
{
    public class MatchCalendarListItemDTO
    {
        [JsonProperty("unique_id")]
        public string UniqueId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("date")]
        public string DateString { get; set; }
    }

    public class MatchCalendarRootObject
    {
        [JsonProperty("data")]
        public List<MatchCalendarListItemDTO> Items { get; set; }
    }

}
