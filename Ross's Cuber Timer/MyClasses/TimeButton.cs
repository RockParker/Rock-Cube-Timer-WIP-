using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ross_Cuber_Timer.MyClasses
{
    public class TimeButton : Button
    {
        public TimeButton()
        {
            this.FontSize = 25;
            this.Margin = new Thickness(0, 3, 0, 0);
            this.BorderThickness = new Thickness(3, 3, 3, 3);
        }
        public string Time { get; set; }
        public bool DNF { get; set; } = false;
        public bool Plus2 { get; set; } = false;

        public string Scramble { get; set; }

        public bool Clicked { get; set; } = false;
    }

}
