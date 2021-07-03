using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Timers;

namespace Ross_Cuber_Timer
{
    class HandleMainTimer
    {

        //init a new main window with no instance because the constructor requests the currently running instance
        private MainWindow _mainwindow;
        public int ScrambleLength 
        { 
            get { return _mainwindow.ScrambleLength; }
        }
        //inits a new instance of the scramble generating class
        private ScrambleGenerator ScrambleGenerator;

        //init the stop watch that actually tracks the solve time
        private Stopwatch solvingtimer;

        //init a new timer which forces the gui update for the big timer
        private System.Timers.Timer updatetxttimer;

        //gets the current value of the most recent time from the main window
        //this is because the update timer method uses the main window's value 100% of the time this way.
        //this is to prevent the list of times and the stopwatch from having a discrepency
        private string _timeoutput { get { return _mainwindow.timeOutput; } }

        // allows public access to the timespan so that the main window can add it to solve list.
        public TimeSpan Time { get { return solvingtimer.Elapsed; } }


        //constructor
        public HandleMainTimer(MainWindow window)
        {            
            _mainwindow = window;
            ScrambleGenerator = new ScrambleGenerator(ScrambleLength);
        }

        public string GetNewScramble()
        {
            return ScrambleGenerator.NewScramble;
        }

        public void StartTimer()
        {
            //init the stopwatch that will actually record the time it takes to solve
            _mainwindow.currentscramble = _mainwindow.lblScramble.Content.ToString();
            solvingtimer = new Stopwatch();
            solvingtimer.Start();

            // initializing the timer that will trigger the event
            // which updates the on screen timer settings the update interval to 3ms
            updatetxttimer = new System.Timers.Timer(3);
            updatetxttimer.Elapsed += OnTimedEvent;
            updatetxttimer.Start();
        }

        //handles all the tasks that start happening when the timer is stopped.
        public void StopTimer()
        {
            //this allows the owner thread to do it to prevent errors.
            _mainwindow.Dispatcher.InvokeAsync(() =>
            {
                //stops the on screen timer
                solvingtimer.Stop();

                //stops the timer that forces a gui update
                updatetxttimer.Stop();

                //calls the class that updates the main timer on last time
                UpdateTimer();

                //tells the main window to update the list of times so that the new one is listed
                _mainwindow.UpdateTimesList();

                //tells the main window to show a new scramble with a freshly generated one
                var scramble = GetNewScramble();
                if (scramble == "Sorry, we had a problem making a new scramble\nYou're just too good :P")
                {
                    _mainwindow.RetryButton.Visibility = System.Windows.Visibility.Visible;
                }
                _mainwindow.lblScramble.Content = scramble;

            });
        }

        private void UpdateTimer()
        {
            //updates the onscreen timer using it's owner thread
             _mainwindow.Dispatcher.InvokeAsync(() =>
            {
                _mainwindow.txtsolvingtimer.Text = _timeoutput;
            }
            );
        }


        //this is the elapsed event handler that forces the big timer to update
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
                UpdateTimer();
        }
    }
}
