using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ross_Cuber_Timer
{
    /// <summary>
    /// I have no idea how this works... I have just looked it up and copied an online tutorial
    /// I will however do my best to explain in further comments...
    /// </summary>
    class ThemeResourceDictionary : ResourceDictionary
    {

        //init private variables that can't be influnced directly.
        private Uri darktheme, lighttheme;

        //defines a public uri that be be used to influence the private ones
        public Uri DarkTheme
        {
            get { return darktheme; }
            set { darktheme = value; UpdateSource(); }
        }

        public Uri LightTheme
        {
            get { return lighttheme; }
            set { lighttheme = value; UpdateSource(); }
        }


        //this method chenges the source of the skin to different libraries so that 
        //it can be reskinned on the fly.
        public void UpdateSource()
        {
            var value = App.theme == Theme.Dark ? DarkTheme : LightTheme;
            if (value != null && base.Source != value)
            {
                base.Source = value;
            }
        }
    }
}
