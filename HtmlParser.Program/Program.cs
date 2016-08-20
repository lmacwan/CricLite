using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            PlayerManager.Go();
            Parser.GetPlayerXML(36084);
            Parser.GetPlayerXML(36084);
        }
    }
}
