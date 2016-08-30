using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlParser.Series;

namespace HtmlParser
{
    public class SeriesManager
    {
        private static string WRITER_LOCK_LIVE = "Lock";

        private static Thread fileWriterLive = null;

        private static Dictionary<string, string> backingList_live = null;
        private static Dictionary<string, string> backingList_post = null;
        private static Dictionary<string, string> backingList_pre = null;

        //public static void Start()
        //{
        //    fileWriterLive = new Thread(FileWriteThreadStart);
        //    fileWriterLive.Start();
        //}

        //public static void Stop()
        //{
        //    fileWriterLive.Abort();
        //    FileWriteThreadStart();
        //}

        public static List<SeriesDTO> GetAllLive()
        {
            var content = API.Read(ConfigurationManager.AppSettings["OngoingSeriesURL"]);
            var list = GetAllFromString(content);

            DateTime threadStartDate = DateTime.Now;
            var path = Path.Combine(ConfigurationManager.AppSettings["DataFolder"], ConfigurationManager.AppSettings["SeriesFolder"], "live.txt");

            new Thread(new ThreadStart(() =>
            {
                lock (WRITER_LOCK_LIVE)
                {
                    if (File.Exists(path) == false || (File.Exists(path) && File.GetLastWriteTime(path) <= threadStartDate && File.ReadAllText(path) != content))
                    {
                        WriteContent(path, content);

                        UpdateBackingListLive(list);
                    }
                }

                CreateSeriesStructureOnDisk(list);

                Thread.CurrentThread.Abort();
            })).Start();
            return list;
        }

        private static void UpdateBackingListLive(List<SeriesDTO> list)
        {
            foreach (var series in list)
            {
                var seriesContent  = JsonConvert.SerializeObject(series);
                if (backingList_live.ContainsKey(series.SeriesId))
                {
                    var listContent = backingList_live[series.SeriesId];

                    if (listContent != seriesContent)
                    {
                        using (StreamWriter writer = File.CreateText(Path.Combine(ConfigurationManager.AppSettings["DataFolder"], ConfigurationManager.AppSettings["SeriesFolder"], series.SeriesId + ".txt")))
                        {
                            writer.Write(seriesContent);
                            writer.Flush();
                            writer.Close();
                        }
                    }
                }
                else
                {
                    backingList_live.Add(series.SeriesId, seriesContent);
                }
            }
            backingList_live.ToList().RemoveAll(i => list.Any(j => j.SeriesId == i.Key) == false);
        }

        private static void CreateSeriesStructureOnDisk(List<SeriesDTO> list)
        {
            var seriesDirectoryPath = Path.Combine(ConfigurationManager.AppSettings["DataFolder"], ConfigurationManager.AppSettings["SeriesFolder"]);
            if (Directory.Exists(seriesDirectoryPath) == false)
            {
                Directory.CreateDirectory(seriesDirectoryPath);
            }
            foreach (var series in list)
            {
                using (StreamWriter writer = File.CreateText(Path.Combine(seriesDirectoryPath, series.SeriesId + ".txt")))
                {
                    writer.Write(JsonConvert.SerializeObject(series));
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        private static void WriteContent(string path, string content)
        {
            using (StreamWriter streamWriter = File.CreateText(path))
            {
                streamWriter.Write(content);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }

        private static List<SeriesDTO> GetAllFromString(string content)
        {
            return JsonConvert.DeserializeObject<List<SeriesDTO>>(content);
        }

        //private static void FileWriteThreadStart()
        //{
        //    while (true)
        //    {
        //        lock (WRITER_LOCK_LIVE)
        //        {
        //            WriteToFile();
        //        }
        //        Thread.Sleep(3600000);
        //    }
        //}

        //private static void WriteToFile()
        //{
        //    StreamWriter streamWriter = null;
        //    foreach (var item in backingList_live)
        //    {
        //        var path = Path.Combine(ConfigurationManager.AppSettings["DataFolder"], ConfigurationManager.AppSettings["SeriesFolder"], ConfigurationManager.AppSettings["LiveSeriesFolder"], item.Key);
        //        if (Directory.Exists(path) == false)
        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        streamWriter = File.CreateText(Path.Combine(path, "current.txt"));
        //        streamWriter.Write(item.Value);
        //        streamWriter.Flush();
        //        streamWriter.Close();
        //    }
        //    backingList_live.Clear();
        //}
    }
}
