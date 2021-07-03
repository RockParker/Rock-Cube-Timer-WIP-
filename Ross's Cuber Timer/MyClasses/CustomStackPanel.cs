using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ross_Cuber_Timer.MyClasses
{
    class CustomStackPanel : StackPanel
    {
        public TimeButton _tb;
        public Button Plus2 = new Button();
        public Button DNF = new Button();
        public Label Scramble = new Label();
        public Label Time = new Label();
        private TimeSpan ts;
        public CustomStackPanel(TimeButton tb)
        {
            _tb = tb;
            Time.Content = _tb.Time;
            Scramble.Content = tb.Scramble;
            GenerateButtons();

            this.Children.Add(Time);
            this.Children.Add(Scramble);
            this.Children.Add(Plus2);
            this.Children.Add(DNF);
        }

        private void GenerateButtons()
        {
            Plus2.Content = "+2";
            Plus2.Click += btnPlus2_Click;

            DNF.Content = "DNF";
            DNF.Click += btnDNF_Click;

        }

        private void btnPlus2_Click(object sender, RoutedEventArgs e)
        {
            _tb.Content = ButtonFunctions.Plus2(_tb, GenerateTimeSpan(_tb.Content.ToString()));
            Time.Content = _tb.Content.ToString();
            Update();
        }

        private void btnDNF_Click(object sender, RoutedEventArgs e)
        {
            _tb.Content = ButtonFunctions.DNF(_tb);
            Update();
        }
        private TimeSpan GenerateTimeSpan(string str)
        {
            try
            {
                str = str.Replace(":", ".");
                str = str.Replace("(DNF)", "");
                string[] times = str.Split('.');
                ts = new TimeSpan(0, 0, Convert.ToInt32(times[0]), Convert.ToInt32(times[1]), Convert.ToInt32(times[2]));
            }
            catch (Exception e) { MessageBox.Show("Couldn't generate a TimeSpan" + e.ToString()); }

            return ts;
        }
        private void Update()
        {
            _tb.Content = _tb.Content;
            Time.Content = _tb.Content;
        }
    }
}
