using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class CheckerBoard
    {
        private int [,] board;
        public CheckerBoard ()
        {
        	//board = new int [8][8];
        	//Populate board
        }


        public int[,] applyMove (List<int> movePair)
        {
            //Apply Move and return true if move successfully moved
            return null;
        }

        private bool validateMove ()
        {
            //Validate a Given Move
            return false;
        }

        private bool getResult(int Player)
        {
            // return win status for 
            return true;
        }
    }
}
