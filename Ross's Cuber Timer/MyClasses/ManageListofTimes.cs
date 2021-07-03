using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ross_Cuber_Timer.MyClasses;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Ross_Cuber_Timer.MyClasses
{
    public class ManageListofTimes
    {
        //Here is my private field that will be used to change all our stuff.
        private List<TimeButton> ListofAllTimes = new List<TimeButton>();

        //this is the public readonly     (I'm hoping I can't write to it but it shouldn't matter)
        //version that I can use to access the information wihtout a direct connection to the list that actually holds the values.
        public List<TimeButton> ListofTimes { get { return ListofAllTimes; } }

        private string CurrentCube;

        //this method is so the main class can indirectly set the current cube
        //could be used to add more functionality later on.
        public void startUp(string str)
        { CurrentCube = str; }


        //This method takes the type of cube and loads a list that has all the values from the save files.
        public void LoadListofTimes(string _currentcube)
        {
            //if the current cube isn't null. that means there is a currently active cube
            //before i change the type of cube I want to save the solves that are currently loaded
            if (CurrentCube != null)
            {
                ReadWrite.WriteJSON(CurrentCube, ListofAllTimes);
            }

            //now we start loading the list of buttons for the requested type of cube
            ListofAllTimes = ReadWrite.ReadJSON(_currentcube);

            //sets the current cube to the newly activated one.
            CurrentCube = _currentcube;
        }


        //This method generates a button with the correct values for the variables.
        public void NewButton(string time, string scramble)
        {
            try
            {
                //makes a new template version of the timebutton class.
                TimeButton tb = new TimeButton
                {
                    //defining the 
                    Content = time,
                    Time = time,
                    Scramble = scramble
                };
                //defining the click function
                tb.Click += ButtonFunctions.btnClick;
                //adding the button to the list of buttons
                ListofAllTimes.Add(tb);
            }
            catch (Exception e)
            {
                //this just calls another method I made that shows a popup with the error, and then saves it to a log.
                GenerateLogs.MakeLog("We had a problem generating the solve into a button\n", e);
            }
        }

        public void GeneratePanel(StackPanel sp)
        {
            //this is the method that does the graphical part of the loading process, 
            //the first step is to clear the stack panel of the currently listed times. 
            sp.Children.Clear();
            
            // making new list that I can manipulate
            var temp = ListofTimes;

            //reverses the list so that I can read forward but have the most recent first
            temp.Reverse();


            /* I can't honestly remember why I chose a foreach with a counter 
               over a for loop while iterating with the standard "[]" style of iteration
               it just seemed better although it does feel a bit patchy */
            //resetting the counter so I cap at 10 times shown at a "time" :P
            int count = 0;
            foreach (TimeButton tb in temp)
            {
                count++;
                //if the counter is over 10, stop looping. 
                if ( count >= 11) { break; }

                //if the solve isn't marked as plus 2, just add the time
                if (tb.Plus2 == false)
                { tb.Content = tb.Time; }
                //if the solve is marked with a plus 2, add 2 to the time
                else if (tb.Plus2 == true)
                {
                    var time = ButtonFunctions.GenerateTimeSpan(tb.Time);
                    tb.Content = new TimeSpan(0,time.Minutes, time.Seconds+2, time.Milliseconds).ToString();
                }
                //if the button is marked as a DNF, add DNF to the content
                if (tb.DNF == true)
                {
                    tb.Content += "(DNF)";
                }


                //this is just a visual thing, changing the border colour to easily show the seperation between buttons.
                tb.BorderBrush = new SolidColorBrush(Colors.PaleVioletRed);
                if (count % 2 == 0) { tb.BorderBrush = new SolidColorBrush(Colors.Black); }
                tb.Clicked = false;

                //adding the button to the stackpanel
                sp.Children.Add(tb);
            }
            //re-reversing the list because I have to for some reason..... IDK WHY
            temp.Reverse();
        }
    }
}
