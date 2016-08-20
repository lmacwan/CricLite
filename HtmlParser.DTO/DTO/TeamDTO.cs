﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.DTO
{
    public class TeamDTO
    {
        public string Country { get; set; }
        public string Gender { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Country, this.Gender);
        }
    }
}
