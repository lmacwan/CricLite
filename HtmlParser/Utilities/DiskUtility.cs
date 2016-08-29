using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser
{
    public class DiskUtility
    {
        public static void WriteToDisk(string path, string content) 
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.Write(content);
                writer.Close();
            }
        }
    }
}
