using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ross_Cuber_Timer
{
    public class IsValid
    {
         // defining variables to be used
        private List<string> _listofmoves;
        private List<string> probablyredundant { get { return _scramblegenerator.ViewableList; } }

        private ScrambleGenerator _scramblegenerator;
        public IsValid(ScrambleGenerator ScrambleGenerator)
        {
            _scramblegenerator = ScrambleGenerator;
        }


        public bool isValidMove(string str)
        {
            // copying the list that is storing the new scramble during its generation
            //and generating a substring that only holds the face that the new move is on
            _listofmoves = probablyredundant;
            str = str.Substring(0,1);

            //Changing what method we use based on what face we are checking
            if (str == "R" || str == "L") { return CheckLeftRight(str); }
            if (str == "U" || str == "D") { return CheckTopBottom(str); }
            if (str == "F") { return CheckFront(str); }

            //this is for debugging and catching unhandled things...
            //not sure how that would happen but it would suck to not account for it
            return false;
        }


        /* Everything in this first method applies to the following two 
           but to save comments I will not comment in them
           This method checks if left or right moves are valid
         */
        private bool CheckLeftRight(string str)
        {
           //this loop iterates through the strings that are in the currently generated moves.
            foreach (string listString in _listofmoves)
            {
                //defining a new substring to compare only the first Char in each move which 
                //lines up with  one of the faces
                var temp = listString.Substring(0,1);

                /*here I'm checking if the newly generated move is perpendicular to the current 
                  move from the list of moves....
                The way this filter works is that if there isn't a perpendicular move 
                since the last move of the same face it doesn't allow the newly generated move to be added
                if the new move is the opposite face, it won't be affected because there is no need to check
                opposite faces because it just needs to check if there has been a perpendicualr move since 
                the previous move of the same face*/
                if (temp == "U" || temp == "D" || temp == "F")
                {
                    Console.WriteLine("Allowed: "+str +" becuase prev was: "+temp);
                    return true;
                }

                //if the new move is on the same face as the move currently being checked, don't allow it
                if (str == temp)
                {
                    Console.WriteLine(str + " is invalid because of "+ temp);
                    return false;
                }                
            }
            //this return is just to catch any rogue things that may not work as intended
            return false;
        }

        //checks if newly generated top and bottom moves are valid
        private bool CheckTopBottom(string str)
        {
            foreach (string listString in _listofmoves)
            {
                var temp = listString.Substring(0, 1);
                if (temp == "R" || temp == "L" || temp == "F")
                {
                    Console.WriteLine("Allowed: " + str + " becuase prev was: " + temp);
                    return true;
                }
                if (str == temp)
                {
                    Console.WriteLine(str + " is invalid because of " + listString);
                    return false;
                }
            }
            return false;
        }

        //Checks if newly generated front moves are valid moves
        private bool CheckFront(string str)
        {
            foreach (string listString in _listofmoves)
            {
                var temp = listString.Substring(0, 1);
                if (temp == "U" || temp == "D" || temp == "L" || temp == "R")
                {
                    Console.WriteLine("Allowed: " + str + " becuase prev was: " + temp);
                    return true;
                }
                if (str == temp)
                {
                    Console.WriteLine(str + " is invalid because of " + listString);
                    return false;
                }
            }
            return false;
        }
    }
}
