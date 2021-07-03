﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ross_Cuber_Timer.MyClasses
{
    public static class ButtonFunctions
    {
        // This method returns a string that is applied to the button content
        public static string DNF(TimeButton _button)
        {
            //grabs the current content in the button
            string str = _button.Content.ToString();

            //if the button's DNF property is true, it removes the "(DNF)" otherwise it adds it
            switch (_button.DNF)
            {
                case true:
                     str = str.Replace("(DNF)", "");
                    _button.DNF = false;
                    break;
                case false:
                     str += "(DNF)";
                    _button.DNF = true;
                    break;
            }
            return str;
        }

        // This method returns a string that is applied to the button content
        public static string Plus2(TimeButton _button, TimeSpan ts)
        {
            //didn't want to set equal to but was getting an error about unassigned local variable otherwise
            TimeSpan newTime = ts;
            string str;

            //if the button's Plus2 property is false, it adds 2 to the time otherwise it removes 2
            switch (_button.Plus2)
            {
                case (false):
                    newTime = new TimeSpan(0, 0, ts.Minutes, ts.Seconds + 2, ts.Milliseconds);
                    _button.Plus2 = true;
                    break;
                case (true):
                    newTime = new TimeSpan(0, 0, ts.Minutes, ts.Seconds - 2, ts.Milliseconds);
                    _button.Plus2 = false;
                    break;
            }
            //formats the string properly, so that it looks good.
            str = String.Format("{0:00}:{1:00}.{2:00}", newTime.Minutes, newTime.Seconds, newTime.Milliseconds);
            //adds the DNF part of the button label if it should be there.
            if (_button.DNF) { str += "(DNF)"; }
            return str;
        }

        //this click function is the function that every timebutton's click funtion is set to.
        public static void btnClick(object sender, RoutedEventArgs e)
        {
            //casting the sender to a timebutton
            var tb = (TimeButton)sender;

            //casting the parent object as a stackpanel
            StackPanel parent = (StackPanel)tb.Parent;

            //gets the index of the child inside the parent
            int index = parent.Children.IndexOf(tb);
            try
            {
                //checks if the button has been clicked already
                if (tb.Clicked == false)
                {
                    //if it hasn't been clicked open a custom stackpanel in the spot just after the button that was clicked.
                    tb.Clicked = true;
                    parent.Children.Insert(index + 1, new CustomStackPanel(tb));
                }

                else if (tb.Clicked == true)
                {
                    //if the button has been clicked remove the object that is right after the button that sent the click
                    parent.Children.RemoveAt(index + 1);
                    tb.Clicked = false;
                }
            }
            //useless shit is useless
            catch {}

        }

        //This Method takes a string, specifically the time that the button has, and returns a timespan version of the time.
        public static TimeSpan GenerateTimeSpan(string str)
        {
            TimeSpan ts;
            try
            {
                //formatting the string to be split into an array
                str = str.Replace(":", ".");
                str = str.Replace("(DNF)", "");
                string[] times = str.Split('.');

                //generates a new timespan with the array that was made.
                ts = new TimeSpan(0, 0, Convert.ToInt32(times[0]), Convert.ToInt32(times[1]), Convert.ToInt32(times[2]));                
            }

            catch (Exception e) { MessageBox.Show("Couldn't generate a TimeSpan" + e.ToString()); return default; }
            
            return ts;
        }

        //this is the click function for the +2 button
        private static void btnPlus2_Click(object sender, RoutedEventArgs e)
        {
            //casts the type of the sender to a TimeButton
            var _button = (TimeButton)sender;

            //sets the button content to the new time, generated by the plus2 function
            _button.Content = ButtonFunctions.Plus2(_button, GenerateTimeSpan(_button.Content.ToString()));
        }

    }
}
