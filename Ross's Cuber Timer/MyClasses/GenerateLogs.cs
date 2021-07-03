using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ross_Cuber_Timer.MyClasses
{
    public static class GenerateLogs
    {
        public static void MakeLog(string str, Exception e)
        {
            //taking the error and turning it into an easily manipulated string
            var error = e.ToString();

            /* 
               showing a messagebox with the error and some text that I customize in the calling class.
               an example call would be MakeLog("Sorry we couldn't do this action for you!", e)
               this just means I can use this function to make errors for everypart of the code while it always looks
               personalized to each class.
            */
            MessageBox.Show(str+"\nIf this Continues despite a restart please open the logs\n"+error);

            //generates a new URI that I can use to make a new and importantly, a uniquely named file for all the errors.
            string uri = "/Logs/"+DateTime.Now.Day.ToString()+"_"+ DateTime.Now.Minute.ToString()+"_"+ DateTime.Now.Second.ToString()+".txt";

            //calls the function that will write the file.
            ReadWrite.WriteTXT(uri, e.ToString());
        }
    }
}
