using Ross_Cuber_Timer.MyClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ross_Cuber_Timer
{
    /// <summary>
    /// Interaction logic for TimePopUp.xaml
    /// </summary>
    public partial class TimePopUp : Window
    {
        private MainWindow _mainwindow;
        private TimeButton _button;
        private string btnText { get { return (string)_button.Content; } }
        TimeSpan ts;

        public TimePopUp(MainWindow window, TimeButton button)
        {
            InitializeComponent();
            _mainwindow = window;
            _button = button;
            lblScramble.Content = button.Scramble;
            
            this.Show();
            this.Focus();
        }

        private void Update()
        {
            _button.Content = _button.Content;
            lblTime.Content = btnText;
        }


        private TimeSpan GenerateTimeSpan(string str)
        {
            try
            {
                str = str.Replace(":", ".");
                str = str.Replace("(DNF)","");
                string[] times = str.Split('.');
                ts = new TimeSpan(0, 0, Convert.ToInt32(times[0]), Convert.ToInt32(times[1]), Convert.ToInt32(times[2]));
            }
            catch (Exception e) { MessageBox.Show("Couldn't generate a TimeSpan" + e.ToString()); this.Close(); }

            return ts;
        }

        private void btnPlus2_Click(object sender, RoutedEventArgs e)
        {
            _button.Content = ButtonFunctions.Plus2(_button, GenerateTimeSpan(btnText));
            Update();
        }

        private void btnDNF_Click(object sender, RoutedEventArgs e)
        {
            _button.Content = ButtonFunctions.DNF(_button);
            Update();
        }

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
