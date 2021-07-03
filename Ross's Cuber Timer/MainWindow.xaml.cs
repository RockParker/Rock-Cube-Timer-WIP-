using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Ross_Cuber_Timer.MyClasses;
using System.IO;

namespace Ross_Cuber_Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /*Defining variables that are explained as they are used*/
        private System.Timers.Timer keyheldtimer;
        private Boolean canRelease = false;
        private Boolean isRunning = false;
        public TextBlock txtsolvingtimer;
        protected private Boolean ExtraMovesEnabled = false;
        private HandleMainTimer HandleMaintimer;
        public string currentscramble { get; set; } = "";

        public string CurrentCube { get; set; } = "3X3";

        //Here is the length of the scramble.. eventually will be dfined by the settings
        public int ScrambleLength = 20;

        //private ScrambleGenerator ScrambleGenerator;

        public TimeSpan ts { get { return HandleMaintimer.Time; } }
        public string timeOutput
        {
            get { return String.Format("{0:00}:{1:00}.{2:00}",
                    ts.Minutes, ts.Seconds, Math.Floor((double)ts.Milliseconds / 10));}
        }

        private Enum currentTheme;

        private WindowConfig Config;
        private ManageListofTimes ManageListofTimes;
        /* This is the end of variable declaration and the start of the actual coding*/

        public MainWindow()
        {
            InitializeComponent();
            this.Closed += SaveSettings;
            //ScrambleGenerator = new ScrambleGenerator(ScrambleLength);
            ManageListofTimes = new ManageListofTimes();
            HandleMaintimer = new HandleMainTimer(this);
            txtsolvingtimer = this.txtSolvingTimer;
            Startup();
            ManageListofTimes.startUp(CurrentCube);

            var temp = GetNewScramble();
            if (temp == "Sorry, we had a problem making a new scramble\nYou're just too good :P")
            {
                temp = GetNewScramble();
            }
            lblScramble.Content = temp;

           // MessageBox.Show(this.WindowState.ToString());
        }

        private void Startup()
        {
            Config = ReadWrite.ReadConfig();
            if (Config == null)
            {
                Config = new WindowConfig();
            }
            if (Config.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.Left = Config.Left;
                this.Top = Config.Top;
                this.Height = Config.Height;
                this.Width = Config.Width;            
            }
            if (App.theme != Config.Theme)
            {
                (App.Current as App).Changetheme(Config.Theme);
            }
        }

        public void SaveSettings(object sender, EventArgs e)
        {
            SaveSettings();
            //MessageBox.Show("here");
            this.Close();
        }
        public void SaveSettings()
        {
            Config.WindowState = this.WindowState;
            Config.Left = Convert.ToInt32(this.Left);
            Config.Top = Convert.ToInt32(this.Top);
            Config.Height = Convert.ToInt32(this.Height);
            Config.Width = Convert.ToInt32(this.Width);
            Config.Theme = App.theme;
            ReadWrite.WriteConfig(Config);    
        }

        private string GetNewScramble()
        {
            currentscramble = HandleMaintimer.GetNewScramble();
            return currentscramble;
        }
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (spSettings.Visibility == Visibility.Hidden) { spSettings.Visibility = Visibility.Visible; return; }
            spSettings.Visibility = Visibility.Hidden;
        }

        private void btnToggleDarkMode_Click(object sender, RoutedEventArgs e)
        {
            if (App.theme == Theme.Dark)
            {
                (App.Current as App).Changetheme(Theme.Light);
                return;
            }
            (App.Current as App).Changetheme(Theme.Dark);
        }

        private void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            var temp = GetNewScramble();
            if (temp == "Sorry, we had a problem making a new scramble\nYou're just too good :P")
            {
                lblScramble.Content = "Maybe a restart would help?";
            }
            lblScramble.Content = temp;
            RetryButton.Visibility = Visibility.Hidden;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            //if the key presses are repeated skip them so that
            //the code works....
            if (e.IsRepeat)
            {
                return;
            }


            //Checking if both the cntrl keys are held down
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.RightCtrl))
            {

                //if the timer is running the person likely
                //wants to stop the timer
                if (isRunning)
                {
                    /*Calling the timer handler method and telling it to stop timing
                     * this also triggers other things inside the handler
                     * it forces the window to get one last refresh.*/
                    HandleMaintimer.StopTimer();
                    try { (App.Current as App).Changetheme((Theme)currentTheme); }
                    catch (Exception ex) { GenerateLogs.MakeLog("There was a problem applying your theme!\n", ex); }
                    
                    //Thread.Sleep(30);
                    /*Resetting the bools that are used to verify certain states
                     -canRelease verifies if the person has held the buttons for long enough
                     -isRunning verifies if the timer is running*/
                    canRelease = false;
                    isRunning = false;
                    return;
                }


                //Starting a timer that pings at the 1 second mark to set canRelease to true
                keyheldtimer = new System.Timers.Timer(1000);
                keyheldtimer.Elapsed += OnTimedEvent;
                keyheldtimer.Start();
            }
        }

        //<BUG> resetting when pressing a single control button
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            //if the key lifted is either of the control keys
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                //stop the timer that will enable canRelease
               
                    if (keyheldtimer != null)
                    {
                        if (keyheldtimer.Interval == 1000)
                            keyheldtimer.Stop();
                    }
                
                //if canRelease is set to false, don't do anything else
                if (!canRelease)
                {
                    return;
                }
                if (isRunning)
                {
                    return;
                }
                HandleMaintimer.StartTimer();
                isRunning = true;
            }
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //just sets the bool so that checks can verify
            //if the user has held the keys for long enough
            currentTheme = App.theme;
            Console.WriteLine(currentTheme.ToString());
            (App.Current as App).Changetheme(Theme.Green);
            canRelease = true;
        }

        //this method is only used for generating new times with the button for testing
        //Note: is infact and overload of the above method
        public void UpdateTimesList()
        {

            ManageListofTimes.NewButton(timeOutput, currentscramble);

            ManageListofTimes.GeneratePanel(spTimeList);
        }
        public void UpdateTimesList(string buttonContent)
        {
            /*Generates a new label and defines its content to the time the user achieved
             it then adds the new label to a list of all times that have been saved*/
            ManageListofTimes.NewButton(buttonContent, "No Scramble");

            ManageListofTimes.GeneratePanel(spTimeList);
        }




        //-------------------------------------------------//
        //Settings and controls are below here

        private void TimeButton_Click(object sender, RoutedEventArgs e)
        {

            TimePopUp TimePopUp = new TimePopUp(this, (TimeButton)sender);

        }

        private void Generate_Scramble_Click(object sender, RoutedEventArgs e)
        {
            UpdateTimesList("00:00.00");
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ReadWrite.WriteJSON(CurrentCube, ManageListofTimes.ListofTimes);            
        }

        private void OpenFolder(string str)
        {
            var dir = Directory.GetCurrentDirectory();
            dir += str;
            //Directory.Open
            try
            {
                Directory.CreateDirectory(dir);
                System.Diagnostics.Process.Start(dir);
            }
            catch (Exception e)
            {
                GenerateLogs.MakeLog("Couldn't open the file at:\n" + dir, e);
            }
        }

        private void btnOpenLogs_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder("/Logs/");
        }

        private void btnOpenSolves_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder("/Solves/");
        }

        private void btnError_Click(object sender, RoutedEventArgs e)
        {
            GenerateLogs.MakeLog("This is a new Error",new ArgumentException(""));
        }

        private void btnFullscreen_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                return;
            }
            this.WindowState = WindowState.Maximized;
        }

        private void Cube_Change_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            CurrentCube = btn.Name.Replace("btn","");
            ManageListofTimes.LoadListofTimes(CurrentCube);
            ManageListofTimes.GeneratePanel(spTimeList);
        }
    }
}
