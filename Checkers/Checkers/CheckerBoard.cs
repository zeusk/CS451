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
        	board = new int [8,8];

            for (int i = 0; i<= 2; i++)
            {
                for (int j=0; j <= 7; j++)
                {
                    if(i%2 == 0 && j % 2 == 1)
                    {
                        board[i,j] = 1;
                    }
                    else if(i%2 == 1 && j % 2 == 1)
                    {
                        board[i, j] = 1;
                    }
                }
            }

            for (int i = 5; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (i % 2 == 0 && j % 2 == 1)
                    {
                        board[i, j] = 2;
                    }
                    else if (i % 2 == 1 && j % 2 == 1)
                    {
                        board[i, j] = 2;
                    }
                }
            }

        }

        public void printBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    Console.Write(board[i, j]);
                Console.WriteLine();
            }
        }
        public CheckerBoard getBoard()
        {
            return this;
        }

        public bool applyMove (String mv)
        {
            String [] pos = mv.Split(',');
            int[] cleanPos = new int[2];
            cleanPos[0] = Int32.Parse(pos[0]);
            cleanPos[1] = Int32.Parse(pos[1]);


            //Apply Move and return true if move successfully moved
        }

        private bool validateMove (int[] move)
        {

            return false;
        }

        private bool getResult(int Player)
        {
            // return win status for 

        }
    }
}
