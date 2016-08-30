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

namespace HtmlParser
{
    public class MatchManager
    {
        private static string WRITER_LOCK = "Lock";
        private static Thread fileWriter = null;
        private static Dictionary<string, string> backingList = null;

        public static void Start()
        {
            fileWriter = new Thread(FileWriteThreadStart);
            fileWriter.Start();
        }

        public static void Stop()
        {
            fileWriter.Abort();
            FileWriteThreadStart();
        }

        public static MatchDTO GetLiveMatch(int id)
        {
            var path = Path.Combine(ConfigurationManager.AppSettings["DataFolder"], ConfigurationManager.AppSettings["MatchesFolder"], id.ToString(), "current.txt");
            if (File.Exists(path))
            {
                return GetFromString(File.ReadAllText(path));
            }
            else
            {
                //var content = API.Read(ConfigurationManager.AppSettings["OngoingSeriesURL"])
            }
        }

        private static MatchDTO GetFromString(string content)
        {
            return JsonConvert.DeserializeObject<MatchDTO>(content);
        }

        private static void FileWriteThreadStart()
        {
            while (true)
            {
                lock (WRITER_LOCK)
                {
                    WriteToFile();
                }
                Thread.Sleep(3600000);
            }
        }

        private static void WriteToFile()
        {
            StreamWriter streamWriter = null;
            foreach (var item in backingList)
            {
                var path = Path.Combine(ConfigurationManager.AppSettings["DataFolder"], ConfigurationManager.AppSettings["MatchesFolder"],item.Key);
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
                streamWriter = File.CreateText(Path.Combine(path, item.Value));
                streamWriter.Write(item.Value);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
    }
}
