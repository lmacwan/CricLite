using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.DTO.Match
{
    public class Venue
    {
        [JsonProperty("venueid")]
        public string Id { get; set; }
        [JsonProperty("content")]
        public string Name { get; set; }
    }

    public class Team
    {
        [JsonProperty("Team")]
        public string Name { get; set; }
        [JsonIgnore]
        public string role { get; set; }
        [JsonProperty("teamid")]
        public string Id { get; set; }
    }

    public class MatchDTO
    {
        public string group { get; set; }
        [JsonProperty("matchid")]
        public string Id { get; set; }
        [JsonProperty("mtype")]
        public string Type { get; set; }
        [JsonProperty("series_id")]
        public string SeriesId { get; set; }
        [JsonProperty("series_name")]
        public string SeriesName { get; set; }
        public string stage { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("MatchNo")]
        public string MatchNo { get; set; }
        [JsonProperty("Venue")]
        public Venue Venue { get; set; }
        [JsonProperty("StartDate")]
        public string StartDate { get; set; }
        [JsonProperty("EndDate")]
        public string EndDate { get; set; }
        [JsonProperty("MatchTimeSpan")]
        public string MatchTimeSpan { get; set; }
        [JsonProperty("Team")]
        public List<Team> Team { get; set; }
    }

    public class Results
    {
        public List<MatchDTO> Match { get; set; }
    }

    public class Query
    {
        public int count { get; set; }
        public string created { get; set; }
        public string lang { get; set; }
        public Results results { get; set; }
    }

    public class MatchQuery
    {
        public Query query { get; set; }
    }
}
