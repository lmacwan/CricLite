using HtmlParser.DTO.Player;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;

namespace HtmlParser
{
    public class PlayerManager
    {
        #region Data Members
        private static Dictionary<string, int> playersCache = new Dictionary<string, int>();
        private static Dictionary<string, string> backingList = new Dictionary<string, string>();

        private static string DIRTY_READ_LOCK = "Dirty";

        private static bool isStarted = false;
        private static string filePath = null;
        private static Thread fileThread = null;
        #endregion

        #region Methods
        public static void Freeze()
        {
            isStarted = false;
            filePath = "";
            if (fileThread != null)
            {
                fileThread.Abort();
                fileThread = null;
            }
        }

        public static void Go()
        {
            isStarted = true;
            filePath = Path.Combine(ConfigurationManager.AppSettings["DataFolder"].ToString(), "Players");
            if (fileThread != null)
            {
                fileThread.Abort();
            }
            fileThread = new Thread(PerformDiskIO);
            fileThread.Start();
        }

        public static PlayerDTO Get(string key)
        {
            PlayerDTO dto = null;
            if (isStarted)
            {
                int state = 0;
                lock (DIRTY_READ_LOCK)
                {
                    if (playersCache.ContainsKey(key))
                    {
                        state = playersCache[key];
                    }
                }
                switch (state)
                {
                    case (int)CacheState.MISS:
                        {
                            if (File.Exists(Path.Combine(filePath, key + ".txt")))
                            {
                                dto = ReadFromFile(key);
                            }
                            else
                            {
                                string data = API.Read(string.Concat(ConfigurationManager.AppSettings["PlayerInfoURL"].ToString(), key));
                                dto = JsonConvert.DeserializeObject<PlayerDTO>(data);

                                lock (DIRTY_READ_LOCK)
                                {
                                    backingList.Add(key, data);
                                    playersCache.Add(key, (int)CacheState.DIRTY);
                                }
                            }
                        }
                        break;
                    case (int)CacheState.DIRTY:
                        {
                            string data = "";
                            lock (DIRTY_READ_LOCK)
                            {
                                data = backingList[key];
                            }

                            dto = JsonConvert.DeserializeObject<PlayerDTO>(data);
                        }
                        break;
                    case (int)CacheState.CLEAN:
                        {
                            dto = ReadFromFile(key);
                        }
                        break;
                }
            }
            return dto;
        }
        #endregion

        #region Private Methods
        private static PlayerDTO ReadFromFile(string key)
        {
            string data = "";
            if (File.Exists(Path.Combine(filePath, key + ".txt")))
            {
                data = new StreamReader(Path.Combine(filePath, key + ".txt")).ReadToEnd();
            }

            return JsonConvert.DeserializeObject<PlayerDTO>(data);
        }

        private static void PerformDiskIO()
        {
            while (true)
            {
                while (playersCache.Any(k => k.Value == (int)CacheState.DIRTY))
                {
                    var list = playersCache.Where(k => k.Value == (int)CacheState.DIRTY).ToList();
                    StreamWriter writer = null;
                    foreach (var dirtyItem in list)
                    {
                        if (Directory.Exists(filePath) == false)
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        writer = File.CreateText(Path.Combine(filePath, dirtyItem.Key + ".txt"));
                        writer.Write(backingList[dirtyItem.Key]);
                        writer.Close();

                        lock (DIRTY_READ_LOCK)
                        {
                            playersCache.Remove(dirtyItem.Key);
                            playersCache.Add(dirtyItem.Key, (int)CacheState.CLEAN);
                            backingList.Remove(dirtyItem.Key);
                        }
                    }
                }
                Thread.Sleep(3600000);
                //Thread.Sleep(10000);
            }
        }
        #endregion

    }
}
