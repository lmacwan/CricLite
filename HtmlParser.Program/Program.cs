using HtmlParser.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HtmlParser.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            MatchManager.Initialize();
            var matches = MatchManager.GetMatchCalendar();

            var matches1 = MatchManager.GetMatchCalendar();

            var matches2 = MatchManager.GetMatchCalendar();
        }
    }
}
