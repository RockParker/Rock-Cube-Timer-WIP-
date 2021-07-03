using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;


namespace Ross_Cuber_Timer.MyClasses
{
    /// <summary>
    /// I'm using this static class use hold methods that can be called for the purpose of reading and writing to a JSON file
    /// The reason that I chose JSON file types specifically, is because easch object should be getting saved as it's own object in JSON.
    /// This means that the deserializing and serializing process shouldn't be too diffcult.
    /// </summary>
    public static class ReadWrite 
    {
        //Defining reader/writer to be used later
        public static StreamWriter Writer;
        public static StreamReader Reader;


        //This is the method that will called when I want to write all the current solve times into a file.
        public static void WriteJSON(string CurrentCube, List<TimeButton> _list)
        {
            try
            {
                //here I'm passing the URI builder the current cube type and the value true
                // the false value is declaring the variable "writing" so true means writing
                Writer = new StreamWriter(GetUri(CurrentCube, true));

                //Defining a new Json object that will be used to serialize the values into a JSON File
                JSONObject temp;
                foreach (TimeButton tb in _list)
                {
                    // taking the time button and serializing it into a JSON object that only has fields for the thing si care about
                    temp = new JSONObject
                    {
                        Time = tb.Content.ToString(),
                        DNF = tb.DNF,
                        Plus2 = tb.Plus2,
                        Scramble = tb.Scramble
                    };

                    //Seriliazing the object into JSON format and then writing it into a file
                    var jsonObject = JsonConvert.SerializeObject(temp);
                    Writer.WriteLine(jsonObject);
                }

                // closing the writer in the right way
                Writer.Flush();
                Writer.Close();
            }
            catch (Exception e)
            {
                //makes a log of any errors that are had while also making a messagebox that notifies the user
                GenerateLogs.MakeLog("We had a problem serializing and writing to the file\n",e);
                return;
            }
        }


        //This method takes a custom object and serializes it, to be written to a file. 
        public static void WriteConfig(WindowConfig Config)
        {
            Writer = new StreamWriter(GetUri()+"/Settings.txt");
            var str = JsonConvert.SerializeObject(Config);
            Writer.Write(str);
            Writer.Flush();
            Writer.Close();
        }

        //This method returns a custom object that holds the information needed to
        //instantiate a window with the same properties as the one that left
        //TL;DR reads a settings file into a custom object
        public static WindowConfig ReadConfig()
        {
            //initiating the object to be written to
            WindowConfig temp = new WindowConfig();//this is to stop errors....
            //initiate the reader
            Reader = new StreamReader(GetUri() + "/Settings.txt");
            string line;

            try
            {
                //its all on one line so I don't need a while loop
                if ((line = Reader.ReadLine()) == null)
                {
                    /*basically, if the settings file is empty. it makes a new template.
                    this is just redundancy incase someone were to delete the file that
                    actually stores the previous settings *The settings aren't saved with any of the other data*  */
                    return new WindowConfig();
                }

                //If the line wasn't null, deserialize the object into the object that stores the information
                temp = JsonConvert.DeserializeObject<WindowConfig>(line);

                //dispose of the reader and close it... then return the result
                Reader.Dispose();
                Reader.Close();
                return temp;
            }

            //catches any exceptions
            catch (Exception e)
            { GenerateLogs.MakeLog("There was a problem deserializing the settings object", e);  }
            //if all else fails, returns the template.
            return new WindowConfig();
        }


        //This method returns a list of all the times that have been saved in the file for the respective cube
        public static List<TimeButton> ReadJSON(string CurrentCube)
        {
            //init a new list to put times into that will eventually be returned to the calling class.
            List<TimeButton> list = new List<TimeButton>();
            try
            {
                //getting the uri of the file.
                //here I'm passing the URI builder the current cube type and the value false
                // the false value is declaring the variable "writing" so false means not writing
                string uri = GetUri(CurrentCube, false);

                //if the file doesn't exist create it
                if (!File.Exists(uri))
                {
                    Reader = new StreamReader(File.Create(uri));
                }
                //if the file exists the reader is set to read that file.
                else { Reader = new StreamReader(uri); }

                //2 temp objects to store values.
                string line;
                JSONObject temp;

                //this loop iterates through all the lines in the file
                //as long as the line isn't empty, line = the current line in the file
                while ((line = Reader.ReadLine()) != null)
                {
                    //gets rid of non-data storing characters to prevent problems
                    line = line.Replace("\n", "");

                    //init a new timebutton to save the info into
                    TimeButton tb = new TimeButton();

                    //defining the click function so the buttons act as I expect
                    tb.Click += ButtonFunctions.btnClick;

                    //here I take the JSON object and move the information into a timebutton that can be added to the list.
                    temp = JsonConvert.DeserializeObject<JSONObject>(line);
                    tb.Time = temp.Time;
                    tb.Scramble = temp.Scramble;
                    tb.DNF = temp.DNF;
                    tb.Plus2 = temp.Plus2;

                    //add the button to the list.
                    list.Add(tb);
                }

                //closing the reader nicely, and then returning the list of all the buttons
                Reader.Dispose();
                Reader.Close();
                return list;
            }
            catch (Exception e)
            {
                GenerateLogs.MakeLog("We had a problem deserializing and reading from the file\n", e);
                return null;
            }
        }


        //This method juat takes a string and writes it to a path that is also handed to the method.
        //I think it only does logs for now, which is why it doesn't look for a file with the name first.
        public static void WriteTXT(string dir, string write)
        {
            var path = GetUri() + dir;
            Writer = new StreamWriter(File.Create(path));
            Writer.Write(write);
            Writer.Flush();
            Writer.Close();
        }

        //this just returns the string which is the current directory the app is running in
        private static string GetUri() 
        {
            var basepath = Directory.GetCurrentDirectory();
            return basepath;
        }

        //this is an overload of the previous that takes a few arguements and alters the file path accordingly
        //CurrentCube stores the cube that was or is active, I use this to define the file to save it to
        //writing is used becuase if i'm writing I want to make a new version of the file, if i'm not writing, I just want the directory.
        private static string GetUri(string CurrentCube, bool writing)
        {
            //sets the directory to be the solves folder. and then creates the folder, if it doesn't exist.
            var basepath = GetUri() + "/Solves/";
            Directory.CreateDirectory(basepath);

            //makes the filepath itself be the basepath plus the cubetype to easily identify the type of cube.
            string uri = basepath+ CurrentCube+"_Solves.json";


            //here i'm using the "writing" boolean to determine if I should delete the file and make a new one
            //or just pass the path back to the calling method.
            if (File.Exists(uri) && writing == true)
            {
                File.Delete(uri);                
            }
            try
            {
                return uri;
            }
            catch (Exception e)
            {
                GenerateLogs.MakeLog("Failed to Generate the URI", e);
                return null;
            }            
        }

    }
}
