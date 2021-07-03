using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ross_Cuber_Timer.MyClasses;

namespace Ross_Cuber_Timer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public enum Theme { Light, Dark, Green}
    public partial class App : Application
    {

        public static Theme theme = Theme.Light;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Changetheme(theme);
        }
        public void Changetheme(Theme newTheme)
        {
            try
            {
                if (Resources.MergedDictionaries != null)
                {
                    Resources.Clear();
                    Resources.MergedDictionaries.Clear();
                }
            }
            catch (Exception e) { GenerateLogs.MakeLog("Error Changing text colour:\n",e); }

            if (newTheme == Theme.Green)
            {
                ApplyGreentheme();
                return;
            }

            if (newTheme == Theme.Dark)
            {
                ApplyDarkTheme();
                return;
            }

            if (newTheme == Theme.Light)
            {
                ApplylightTheme();
                
            }
        }

        private void ApplyGreentheme()
        {
            if (theme == Theme.Dark)
            {
                ApplyDarkTheme();
            }
            if (theme == Theme.Light)
            {
                ApplylightTheme();
            }
            AddResourceDictionary("Dictionaries/GreenMeansGoDictionary.xaml");
        }
        private void ApplyDarkTheme()
        {
            theme = Theme.Dark;
            AddResourceDictionary("Dictionaries/DarkThemeDictionary.xaml");
        }

        private void ApplylightTheme()
        {
            theme = Theme.Light;
            AddResourceDictionary("Dictionaries/LightThemeDictionary.xaml");
        }

        private void AddResourceDictionary(string newResource)
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(newResource, UriKind.Relative) });
        }
    }
}