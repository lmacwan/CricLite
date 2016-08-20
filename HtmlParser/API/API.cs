using System.Net;

namespace HtmlParser
{
    public class API
    {
        public static string Read(string url)
        {
            return new WebClient().DownloadString(url);
        }
    }
}
