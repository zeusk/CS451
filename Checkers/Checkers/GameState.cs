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
            _player1Name = playerName;
            _player2Name = "";
            _playerTurn  = _player1Name;

            _checkerBoard = new CheckerBoard();
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

        public int[,] getBoard()
        {
            return _checkerBoard.getBoard();
        }

        private string _player1Name;
        public string player1Name
        {
            get
            {
                return _player1Name;
            }
        }

        private string _player2Name;
        public string player2Name
        {
            get
            {
                return _player2Name;
            }
        }

        private string _playerTurn;
        public string playerTurn
        {
            get
            {
                return _playerTurn;
            }
        }

        public int getResult()
        {
            return _checkerBoard.getResult();
        }

        public override string ToString() => toString(); // override base method that prints [Class GameState]

        public string toString()
        {
            return toString(this);
        }

        public static String toString(GameState gs)
        {
            List<String> output = new List<String>();

            output.Add(gs._playerTurn);
            output.Add(gs.player1Name);
            output.Add(gs.player2Name);

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    output.Add(gs._checkerBoard.getBoard()[i, j].ToString());

            return String.Join("|", output);
        }

        public static GameState fromString(String gState)
        {
            GameState ret = new GameState("");
            List<String> ls = new List<String>(gState.Split('|'));

            ret.cb = new CheckerBoard(true);
            ret._playerTurn = ls[0];
            ret._player1Name = ls[1];
            ret._player2Name = ls[2];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int[] pos = new int[2];
                    pos[0] = i;
                    pos[1] = j;
                    ret.cb.placePiece(pos, Int32.Parse(ls[2 + ((i * 8) + j)])); 
                }
            }

            return ret;
        }

        public GameState applyMove(List<int> movePair, int playerID)
        {
            return _checkerBoard.applyMove(movePair, playerID) ? this : null;
        }

        public bool checkAvailableJump(int x, int y, int playerID)
        {
            int[] pos = new int[2];

            pos[0] = x;
            pos[1] = y;

            return _checkerBoard.checkAnyJumpPossiblePiece(pos,playerID);
        }

        public bool amPlayer1(string name)
        {
            return _player1Name.Equals(name);
        }

        public bool amPlayer2(string name)
        {
            return _player2Name.Equals(name); // TODO: Redundant really, !amPlayer1()
        }

        public bool isMyTurn(string name)
        {
            return _playerTurn.Equals(name);
        }

        public void endTurn()
        {
            _playerTurn = _playerTurn.Equals(_player1Name) ? _player2Name : _player1Name;
        }
    }
}
