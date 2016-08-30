using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.Series
{
    public class Team
    {
        public string teamid { get; set; }
        public string Name { get; set; }
    }

    public class Participant
    {
        public string mlevel { get; set; }
        public string mtype { get; set; }
        public List<Team> Team { get; set; }
    }

    public class Venue
    {
        public string venueid { get; set; }
        public string content { get; set; }
    }

    public class Team2
    {
        public string Team { get; set; }
        public string role { get; set; }
        public string teamid { get; set; }
    }

    public class Team3
    {
        public string id { get; set; }
        public string matchwon { get; set; }
    }

    public class Result
    {
        public string by { get; set; }
        public string how { get; set; }
        public string isdl { get; set; }
        public string isff { get; set; }
        public string isso { get; set; }
        public List<Team3> Team { get; set; }
        public string Date { get; set; }
    }

    public class Official
    {
        public string personid { get; set; }
        public string type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CountryCode { get; set; }
    }

    public class Match
    {
        public string group { get; set; }
        public string matchid { get; set; }
        public string mtype { get; set; }
        public string stage { get; set; }
        public string status { get; set; }
        public string MatchNo { get; set; }
        public Venue Venue { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string MatchTimeSpan { get; set; }
        public List<Team2> Team { get; set; }
        public Result Result { get; set; }
        public List<Official> Official { get; set; }
    }

    public class Schedule
    {
        public List<Match> Match { get; set; }
    }

    public class SeriesDTO
    {
        public string SeriesId { get; set; }
        public string SeriesName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Participant Participant { get; set; }
        public Schedule Schedule { get; set; }
    }

    public class Results
    {
        public List<SeriesDTO> Series { get; set; }
    }

    public class Query
    {
        public int count { get; set; }
        public string created { get; set; }
        public string lang { get; set; }
        public Results results { get; set; }
    }

    public class RootObject
    {
        public Query query { get; set; }
    }
}
