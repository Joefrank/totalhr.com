﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Models
{
    public class CalendarHTML
    {
        public string GridHTML { get; set; }

        public string NextRequest { get; set; }

        public string PreviousRequest { get; set; }

        public int ViewType { get; set; }

        public string Javascript { get; set; }
    }
}
