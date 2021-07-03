using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Ross_Cuber_Timer
{
    public class ScrambleGenerator
    {
        //this is the string that will eventually be output to the label
        private string FinalString;

        //this timer is used to make sure the generation of a new scramble doesn't exceed a set time
        private Timer preventHalting;

        //this is what the program uses to tell if it should stop trying to generate a new scramble
        private bool shouldHalt = false;

        //this is a list of possible moves that can be added to a scramble
        protected List<string> MoveList = new List<string> { "R", "R'", "R2", "F", "F'", "F2", "L", "L'", "L2", "D", "D'", "D2", "U", "U'","U2"};

        //this list is used to build the scramble while remaining simple to iterate through
        protected List<string> listofmoves;

        //Here I set the length of the scramble
        public int ScrambleLength = 10;

        //this give public access to read the listofmoves list without allowing writing
        public List<string> ViewableList { get { return listofmoves; } }

        //this is how an outside source can ask for a new scramble without calling the method directly
        public string NewScramble
        {
            get { return GenerateScramble(); }
        }

        //this is used to keep track of the spot thats being checked. probably doesn't need to be private
        //or initialied up here but w/e
        private int spot;

        //initializing a new instance of the Random generation class
        private Random rand = new Random();

        // initializing an instance of the isvalid class
        public IsValid IsValid;

        //here is my constructor....
        public ScrambleGenerator(int scramblelength)
        {
            IsValid = new IsValid(this);
            ScrambleLength = scramblelength;
        }



        private string GenerateScramble()
        {
            //init the timer that prevents freezing
            TimerStarter();

            //setting the list to be a new blank list
            listofmoves = new List<string>();

            //iterating the loop for an amount of times equal to the desired scramble length
            for (int i = 0; i < ScrambleLength; i++)
            {

                //this is a checkpoint for failed attempts at generating a new move
                Retry:

                //this method checks if the timer has gone off and cancels generation of a new
                //scramble if it has gone off
                if (shouldHalt)
                {                    
                    return "Sorry, we had a problem making a new scramble\nYou're just too good :P";
                }

                //generates a new move based off the length of the list of possible moves.
                spot = rand.Next(0, MoveList.Count);


                //this calls the isvalid class and if it recieves a false return it sends the loop
                //back to the checkpoint. it also checks if its the first move in a scramble
                //because all moves are valid as a first move.
                if (!IsValid.isValidMove(MoveList[spot]) && listofmoves.Count > 0)
                {
                    goto Retry;
                }

                /*Because its easier to iterate through a list which changes size with a foreach.
                 I'm adding the new moves to the front of the list. to do this I reverse it, add,
                and then reverse again. this is effectively the same as adding it to the front*/
                listofmoves.Reverse();
                listofmoves.Add(MoveList[spot]+" ");
                listofmoves.Reverse();
            }
            
            //stop the timer and return the scramble
            preventHalting.Stop();
            return BuildString();
        }

        //this method iterates through the list that has been built with valid moves and turns it into
        //a single string and then returns the string to be printed in the window
        public string BuildString()
        {
            FinalString = "";
            foreach (string str in listofmoves)
            { 
                FinalString+=str+" ";
            }
            return FinalString;
        }

        //handles starting the timer
        private void TimerStarter()
        {
            preventHalting = new Timer(1500);
            preventHalting.Elapsed += OnTimedEvent;
            shouldHalt = false;
        }

        //this is the trigger that makes the while loop stop if elapsed time goes over 1.5 seconds
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            shouldHalt = true;
        }


    }
}
