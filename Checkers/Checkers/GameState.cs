using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Checkers
{
    public class GameState
    {
        public GameState (string playerName)
        {
            this.player1Name = playerName;
            this._checkerBoard = new CheckerBoard();
        }

        private CheckerBoard _checkerBoard;
        public CheckerBoard cb
        {
            get
            {
                return _checkerBoard;
            }
            set
            {
                _checkerBoard = value;
            }
        }

        private string _player1Name;
        public string player1Name
        {
            get
            {
                return _player1Name;
            }
            set
            {
                _player1Name = value;
            }
        }

        private string _player2Name;

        public int getResult()
        {
            return this._checkerBoard.getResult();
        }

        public string player2Name
        {
            get
            {
                return _player2Name;
            }
            set
            {
                _player2Name = value;
            }
        }
 
        public int[,] getBoard()
        {
            return this._checkerBoard.getBoard();
        }

        public override string ToString() => toString(); // override base method that prints [Class GameState]

        public string toString()
        {
            return GameState.toString(this);
        }

        public static String toString(GameState gs)
        {
            List<String> output = new List<String>();
            for(int i=0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    output.Add(gs._checkerBoard.getBoard()[i, j].ToString());
            }
            output.Add(gs.player1Name);
            output.Add(gs.player2Name);
            String res = String.Join("|", output);
            return res;
        }

        public static GameState fromString(String gState)
        {
            GameState ret = new GameState("");
            List<String> ls = new List<String>(gState.Split('|'));
            ret.cb = new CheckerBoard(true);
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    int[] pos = new int[2];
                    pos[0] = i;
                    pos[1] = j;
                    ret.cb.placePiece(pos, Int32.Parse(ls[i * 8 + j])); 
                }
                ret.player1Name = ls[64];
                ret.player2Name = ls[65];
            }

            return ret;
        }

        public GameState applyMove(List<int> movePair, int playerID)
        {
            Debug.WriteLine("before:");
            this._checkerBoard.printBoard();
            if (this._checkerBoard.applyMove(movePair, playerID))
            {
                Debug.WriteLine("valid--after:");
                this._checkerBoard.printBoard();
                return this;
            }
            else
            {
                Debug.WriteLine("invalid move:");
                return null;
            }
        }

        public bool checkAvailableJump(int x, int y, int playerID)
        {
            int[] pos = new int[2];
            pos[0] = x;
            pos[1] = y;
            return this._checkerBoard.checkAnyJumpPossiblePiece(pos,playerID);
        }
    }
}
