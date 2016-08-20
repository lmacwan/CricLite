using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser
{
    public class Player
    {
        public static void Get(int unique_id)
        {
            var url = ConfigurationManager.AppSettings["PlayerInfoURL"].ToString();
            url = url + unique_id;
            var obj = PlayerManager.Get(unique_id.ToString());
        }
    }
}
