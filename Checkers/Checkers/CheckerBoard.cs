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

        public int[,] getBoard()
        {
            int[,] board = new int[8, 8];
            return board;
        }

        public bool applyMove ()
        {
        	//Apply Move and return true if move successfully moved
        }

        private validateMove ()
        {
        	//Validate a Given Move
        }
    }
}
