using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.DTO.Match
{
    public class UpcomingMatchListItemDTO
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
        [JsonProperty("unique_id")]
        public int UniqueId { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("team-2")]
        public string Team2 { get; set; }
        [JsonProperty("team-1")]
        public string Team1 { get; set; }
        [JsonProperty("matchStarted")]
        public bool IsMatchStarted { get; set; }
        [JsonProperty("squad")]
        public bool IsSquad { get; set; }
    }

    public class UpcomingMatchListRootObject
    {
        [JsonProperty("matches")]
        public List<UpcomingMatchListItemDTO> Items { get; set; }
    }
}
