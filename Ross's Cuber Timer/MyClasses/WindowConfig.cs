using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ross_Cuber_Timer.MyClasses
{
    public class WindowConfig
    {
        // This is a empty class designed to hold some elements of the window that is used to save the last configuratioon of the window
        public WindowConfig()
        {      
        
        }
        public int Height { get; set; } = 940;
        public int Width { get; set; } = 1280;
        public WindowState WindowState { get; set; } = WindowState.Normal;
        public int Top { get; set; } = 100;
        public int Left { get; set; } = 100;

        public string LastCube { get; set; } = "3X3";

        public Theme Theme { get; set; } = Theme.Light;
    }
}
