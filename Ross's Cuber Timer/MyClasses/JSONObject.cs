using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ross_Cuber_Timer.MyClasses
{
    class JSONObject
    {
        public string Time { get; set; }
        public bool DNF { get; set; } = false;
        public bool Plus2 { get; set; } = false;

        public string Scramble { get; set; }
    }
}
