using HtmlParser.DTO.Match;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HtmlParser.Managers
{
    public class MatchManager
    {

        private static string allMatchesDirectory;

        public static void Initialize()
        {
            checkedForCurrentMatch = DateTime.MinValue;
            checkedForUpcomingMatch = DateTime.MinValue.Date;
            checkedForMatchCalendar = DateTime.MinValue.Date;

            allMatchesDirectory = Path.Combine(ConfigurationManager.AppSettings["DataFolder"], ConfigurationManager.AppSettings["MatchesFolder"], "All");
            if (Directory.Exists(allMatchesDirectory) == false)
            {
                Directory.CreateDirectory(allMatchesDirectory);
            }

            allCurrentMatchesFilePath = Path.Combine(allMatchesDirectory, "Live.txt");
            if (File.Exists(allCurrentMatchesFilePath) == false)
            {
                File.Create(allCurrentMatchesFilePath);
            }

            allUpcomingMatchesFilePath = Path.Combine(allMatchesDirectory, "Upcoming.txt");
            if (File.Exists(allUpcomingMatchesFilePath) == false)
            {
                File.Create(allUpcomingMatchesFilePath);
            }

            matchCalendarFilePath = Path.Combine(allMatchesDirectory, "Calendar.txt");
            if (File.Exists(matchCalendarFilePath) == false)
            {
                File.Create(matchCalendarFilePath);
            }
        }

        #region Current Matches

        private static DateTime checkedForCurrentMatch;
        private static string allCurrentMatchesFilePath;
        private static string allCurrentMatchesContent = "";

        public static List<MatchListItemDTO> GetCurrentMatches()
        {
            if (IsReadFromFile_CurrentMatch(DateTime.Now))
            {
                var items = ReadFromFile_AllCurrent();
                return items ?? DownloadCurrentMatches();
            }
            else
            {
                return DownloadCurrentMatches();
            }
        }

        private static bool IsReadFromFile_CurrentMatch(DateTime date)
        {
            return checkedForCurrentMatch.AddSeconds(20) > date;
        }

        private static List<MatchListItemDTO> ReadFromFile_AllCurrent()
        {
            var content = File.ReadAllText(allCurrentMatchesFilePath);
            HtmlParser.DTO.Match.RootObject matchRootObj = JsonConvert.DeserializeObject<HtmlParser.DTO.Match.RootObject>(content);
            return matchRootObj == null ? new List<MatchListItemDTO>() : matchRootObj.Items;
        }

        private static List<MatchListItemDTO> DownloadCurrentMatches()
        {
            var content = API.Read(ConfigurationManager.AppSettings["CurrentMatchesInfoURL"]);
            checkedForCurrentMatch = DateTime.Now;

            if (allCurrentMatchesContent != content)
            {
                allCurrentMatchesContent = content;
                new Thread(CurrentMacthThreadStart).Start();
            }

            HtmlParser.DTO.Match.RootObject matchRootObj = JsonConvert.DeserializeObject<HtmlParser.DTO.Match.RootObject>(content);
            return matchRootObj.Items ?? new List<MatchListItemDTO>();
        }

        private static void CurrentMacthThreadStart()
        {
            WriteCurrentMatchContent();
            UpdateCurrentMatchList();

            Thread.CurrentThread.Abort();
        }

        private static void WriteCurrentMatchContent()
        {
            using (StreamWriter writer = File.CreateText(allCurrentMatchesFilePath))
            {
                writer.Write(allCurrentMatchesContent);
                writer.Flush();
                writer.Close();
            }
        }

        private static void UpdateCurrentMatchList()
        {

        }
        #endregion

        #region Upcoming Matches

        private static DateTime checkedForUpcomingMatch;
        private static string allUpcomingMatchesFilePath;
        private static string allUpcomingMatchesContent = "";

        public static List<UpcomingMatchListItemDTO> GetUpcomingMatches()
        {
            if (IsReadFromFile_UpcomingMatch(DateTime.Now))
            {
                var items = ReadFromFile_AllUpcoming();
                return items ?? DownloadUpcomingMatches();
            }
            else
            {
                return DownloadUpcomingMatches();
            }
        }

        private static bool IsReadFromFile_UpcomingMatch(DateTime date)
        {
            return checkedForUpcomingMatch.AddDays(1) > date;
        }

        private static List<UpcomingMatchListItemDTO> ReadFromFile_AllUpcoming()
        {
            var content = File.ReadAllText(allUpcomingMatchesFilePath);
            UpcomingMatchListRootObject matchRootObj = JsonConvert.DeserializeObject<UpcomingMatchListRootObject>(content);
            return matchRootObj == null ? new List<UpcomingMatchListItemDTO>() : matchRootObj.Items;
        }

        private static List<UpcomingMatchListItemDTO> DownloadUpcomingMatches()
        {
            var content = API.Read(ConfigurationManager.AppSettings["UpcomingMatchesInfoURL"]);
            checkedForUpcomingMatch = DateTime.Now;

            if (allUpcomingMatchesContent != content)
            {
                allUpcomingMatchesContent = content;
                new Thread(WriteUpcomingContentToDisk).Start();
            }

            UpcomingMatchListRootObject matchRootObj = JsonConvert.DeserializeObject<UpcomingMatchListRootObject>(content);
            return matchRootObj.Items ?? new List<UpcomingMatchListItemDTO>();
        }

        private static void WriteUpcomingContentToDisk()
        {
            using (StreamWriter writer = File.CreateText(allUpcomingMatchesFilePath))
            {
                writer.Write(allUpcomingMatchesContent);
                writer.Flush();
                writer.Close();

                Thread.CurrentThread.Abort();
            }
        }

        #endregion

        #region Matches Calendar

        private static DateTime checkedForMatchCalendar;
        private static string matchCalendarFilePath;
        private static string matchCalendarContent = "";

        public static List<MatchCalendarListItemDTO> GetMatchCalendar()
        {
            if (IsReadFromFile_MatchCalendar(DateTime.Now))
            {
                var items = ReadFromFile_MatchCalendar();
                return items ?? DownloadMatchCalendar();
            }
            else
            {
                return DownloadMatchCalendar();
            }
        }

        private static bool IsReadFromFile_MatchCalendar(DateTime date)
        {
            return checkedForMatchCalendar.AddDays(1) > date;
        }

        private static List<MatchCalendarListItemDTO> ReadFromFile_MatchCalendar()
        {
            var content = File.ReadAllText(matchCalendarFilePath);
            MatchCalendarRootObject matchRootObj = JsonConvert.DeserializeObject<MatchCalendarRootObject>(content);
            return matchRootObj == null ? new List<MatchCalendarListItemDTO>() : matchRootObj.Items;
        }

        private static List<MatchCalendarListItemDTO> DownloadMatchCalendar()
        {
            var content = API.Read(ConfigurationManager.AppSettings["MatchCalendarURL"]);
            checkedForMatchCalendar = DateTime.Now;

            if (matchCalendarContent != content)
            {
                matchCalendarContent = content;
                new Thread(WriteMatchCalendarContentToDisk).Start();
            }

            MatchCalendarRootObject matchRootObj = JsonConvert.DeserializeObject<MatchCalendarRootObject>(content);
            return matchRootObj.Items ?? new List<MatchCalendarListItemDTO>();
        }

        private static void WriteMatchCalendarContentToDisk()
        {
            using (StreamWriter writer = File.CreateText(matchCalendarFilePath))
            {
                writer.Write(matchCalendarContent);
                writer.Flush();
                writer.Close();

                Thread.CurrentThread.Abort();
            }
        }

        #endregion

        #region Match By Id
        private static Dictionary<string, string> CurrentMatches = null;

        public static void GetCurrentMatch(string uniqueId)
        {

        }
        #endregion
    }
}
