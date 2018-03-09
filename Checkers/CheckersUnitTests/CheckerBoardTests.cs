using System.Collections.Generic;
using Checkers;
using NUnit.Framework;
namespace CheckersUnitTests
{
    [TestFixture]
    class CheckerBoardTests
    {
        private CheckerBoard _board;

        [Test]
        public void CheckRegularSetupInConstructor()
        {
            _board = new CheckerBoard();
            Assert.IsNotNull(_board, "CheckerBoard default constructor does not initialize correctly");
            var madeBoard = _board.getBoard();
            Assert.IsNotEmpty(madeBoard, "Checker Board Array not initialized correctly");
            var isEqual = true;
            for (var i = 0; i <= 2 && isEqual; i++)
            {
                for (var j = 0; j <= 7 && isEqual; j++)
                {
                    if (i % 2 == 0 && j % 2 == 1)
                        isEqual = madeBoard[i, j] == 1;
                    else if (i % 2 == 1 && j % 2 == 0)
                        isEqual = madeBoard[i, j] == 1;
                }
            }

            Assert.IsTrue(isEqual, "Player 1 not initialized correctly");

            for (var i = 5; i <= 7 && isEqual; i++)
            {
                for (var j = 0; j <= 7 && isEqual; j++)
                {
                    if (i % 2 == 0 && j % 2 == 1)
                        isEqual = madeBoard[i, j] == 2;
                    else if (i % 2 == 1 && j % 2 == 0)
                        isEqual = madeBoard[i, j] == 2;
                }
            }

            Assert.IsTrue(isEqual, "Player 2 not initialized correctly");
        }

        [Test]
        public void CheckAlternateConstructor()
        {
            _board  = new CheckerBoard(true);
            Assert.IsNotNull(_board);
            //Assert.IsNotEmpty(board);
        }

        [Test]
        public void PlacePieceSuccessfully()
        {
            _board = new CheckerBoard(true);
            int[] pPos = new int [2];
            pPos[0] = 2;
            pPos[1] = 3;
            _board.placePiece(pPos, 1);
            Assert.IsTrue(_board.getBoard()[pPos[0], pPos[1]] == 1, "Piece not placed correctly on board"); 
        }

        [Test]
        public void RetrieveBoard()
        {
            _board = new CheckerBoard();
            Assert.IsNotNull(_board.getBoard(), "Board array not retrieved successfully");
        }

        [Test]
        public void ApplyBasicMove()
        {
            _board = new CheckerBoard();
            var pos = new List<int>();
            pos.Add(2);
            pos.Add(1);
            pos.Add(3);
            pos.Add(2);
            Assert.IsTrue(_board.applyMove(pos, 1), "Player 1 move not executed successfully");
        }

        private int[] genPos(int x, int y)
        {
            int[] pos = new int [2];
            pos[0] = x;
            pos[1] = y;
            return pos;
        }

        private List<int> genCoords(int x1, int y1, int x2, int y2)
        {
            List<int> pos = new List<int>();
            pos.Add(x1);
            pos.Add(y1);
            pos.Add(x2);
            pos.Add(y2);
            return pos;
        }


        [Test]
        public void ApplyForwardLeft()
        {
            _board = new CheckerBoard();
            Assert.IsTrue(_board.applyMove(genCoords(2, 1, 3, 2), 1),
                "Forward Left Move Not Being Applied Correctly For Player 1");

            Assert.IsTrue(_board.applyMove(genCoords(5, 2, 4, 1), 2),
                "Forward Left Move Not Being Applied Correctly For Player 2");
        }
        [Test]
        public void ApplyForwardRight()
        {
            _board = new CheckerBoard();
            Assert.IsTrue(_board.applyMove(genCoords(2, 1, 3, 0), 1),
                "Forward Right Move Not Being Applied Correctly For Player 1");

            Assert.IsTrue(_board.applyMove(genCoords(5, 0, 4, 1), 2),
                "Forward Right Move Not Being Applied Correctly For Player 2");
        }

        [Test]
        public void ApplyForwardLeftJump()
        {
            _board = new CheckerBoard(true);
            
            _board.placePiece(genPos(0,0), 1);
            _board.placePiece(genPos(1,1), 2);
            Assert.IsTrue(_board.applyMove(genCoords(0, 0, 2, 2), 1), "Jump Left not Executing Correctly for Player 1");

            _board = new CheckerBoard(true);
            _board.placePiece(genPos(1, 1), 1);
            _board.placePiece(genPos(2, 2), 2);
            Assert.IsTrue(_board.applyMove(genCoords(2, 2, 0, 0), 2), "Jump Left not Executing Correctly for Player 2");
        }

        [Test]
        public void ApplyForwardRightJump()
        {
            _board = new CheckerBoard(true);

            _board.placePiece(genPos(0, 2), 1);
            _board.placePiece(genPos(1, 1), 2);
            Assert.IsTrue(_board.applyMove(genCoords(0, 2, 2, 0), 1), "Jump Right not Executing Correctly for Player 1");

            _board = new CheckerBoard(true);
            _board.placePiece(genPos(1, 1), 1);
            _board.placePiece(genPos(2, 0), 2);
            Assert.IsTrue(_board.applyMove(genCoords(2, 0, 0, 2), 2), "Jump Left not Executing Correctly for Player 2");
        }

        [Test]
        public void ApplyBackwardLeftJump()
        {
            _board = new CheckerBoard(true);

            _board.placePiece(genPos(2, 0), 3);
            _board.placePiece(genPos(1, 1), 2);
            Assert.IsTrue(_board.applyMove(genCoords(2, 0, 0, 2), 1), "Backward Left Jump not Executing Correctly for Player 1");
            
            _board = new CheckerBoard(true);
            _board.placePiece(genPos(0, 2), 4);
            _board.placePiece(genPos(1, 1), 1);
            Assert.IsTrue(_board.applyMove(genCoords(0, 2, 2, 0), 2), "Backward Left Jump not Executing Correctly for Player 1");
            
        }

        [Test]
        public void ApplyBackwardRightJump()
        {
            _board = new CheckerBoard(true);

            _board.placePiece(genPos(2, 2), 3);
            _board.placePiece(genPos(1, 1), 2);
            Assert.IsTrue(_board.applyMove(genCoords(2, 2, 0, 0), 1), "Backward Right Jump not Executing Correctly for Player 1");
            
            _board = new CheckerBoard(true);
            _board.placePiece(genPos(0, 0), 4);
            _board.placePiece(genPos(1, 1), 1);
            Assert.IsTrue(_board.applyMove(genCoords(0, 0, 2, 2), 2), "Backward Left Jump not Executing Correctly for Player 2");
            
        }

        [Test]
        public void ApplyBackwardLeft()
        {
            _board = new CheckerBoard(true);

            _board.placePiece(genPos(1, 1), 3);
            Assert.IsTrue(_board.applyMove(genCoords(1, 1, 0, 2), 1), "Backward Left Movement not Executing Correctly for Player 1");
            
            _board = new CheckerBoard(true);
            _board.placePiece(genPos(0, 2), 4);
            Assert.IsTrue(_board.applyMove(genCoords(0, 2, 1, 1), 2), "Backward Left Movement not Executing Correctly for Player 2");
  
        }

        [Test]
        public void ApplyBackwardRight()
        {
            _board = new CheckerBoard(true);

            _board.placePiece(genPos(1, 1), 3);
            Assert.IsTrue(_board.applyMove(genCoords(1, 1, 0, 0), 1), "Backward Right Movement not Executing Correctly for Player 1");

            _board = new CheckerBoard(true);
            _board.placePiece(genPos(0, 2), 4);
            Assert.IsTrue(_board.applyMove(genCoords(0, 2, 1, 3), 2), "Backward Right Movement not Executing Correctly for Player 2");

        }

        [Test]
        public void AnyForwardPossibleForPlayer()
        {
            _board = new CheckerBoard();

            Assert.IsTrue(_board.checkAnyForwardPossible(1), "Available Forward Moves not detected for Player 1");

            Assert.IsTrue(_board.checkAnyForwardPossible(2), "Available Forward Moves not detected for Player 2");

        }

        [Test]
        public void AnyPossibleForPlayer()
        {
            _board = new CheckerBoard();

            Assert.IsTrue(_board.checkAnyPossibleMoves(1), "Available Moves not detected for Player 1");

            Assert.IsTrue(_board.checkAnyPossibleMoves(2), "Available Moves not detected for Player 2");

        }

        [Test]
        public void AnyBackwardPossible()
        {
            _board = new CheckerBoard();

            Assert.IsFalse(_board.checkAnyBackwardPossible(1), "Invalid backward moves being detected for Player 1");

            Assert.IsFalse(_board.checkAnyBackwardPossible(2), "Invalid backward moves being detected for Player 2");

        }

        [Test]
        public void BackwardPossibleForPiece()
        {
            _board = new CheckerBoard(true);
            _board.placePiece(genPos(2,2),3);

            Assert.IsTrue(_board.checkBackwardPossible(genPos(2, 2),1), "Valid backward move  not being detected for Player 1");

            _board = new CheckerBoard(true);
            _board.placePiece(genPos(2, 2), 4);

            Assert.IsTrue(_board.checkBackwardPossible(genPos(2, 2), 2), "Valid backward moves not being detected for Player 2");

        }


        /*
        [Test]
        public void CheckValidationForwardLeft()
        {
            board = new CheckerBoard();
            var prev = genPos(2, 1);
            var now = genPos(3, 2);
            Assert.IsTrue(board.validateForwardLeft(now, prev, 1, 1), "Forward Validation not validating valid move for Player 1");
            prev = genPos(1, 0);
            now = genPos(2, 1);
            Assert.IsFalse(board.validateForwardLeft(now, prev, 1, 1), "Forward Left Validation is validating invalid move for Player 1");
            prev = genPos(5, 2);
            now = genPos(4, 1);
            Assert.IsTrue(board.validateForwardLeft(now, prev, 2, 2), "Forward Validation not validating valid move for Player 2");
        }

        [Test]
        public void CheckValidationForwardRight()
        {
            board = new CheckerBoard();
            var prev = genPos(2, 1);
            var now = genPos(3, 0);
            Assert.IsTrue(board.validateForwardRight(now, prev, 1, 1), "Forward Right Validation not validating valid move for Player 1");
            prev = genPos(1, 0);
            now = genPos(2, 1);
            Assert.IsFalse(board.validateForwardRight(now, prev, 1, 1), "Forward Right Validation is validating invalid move for Player 1");
            prev = genPos(5, 0);
            now = genPos(4, 1);
            Assert.IsTrue(board.validateForwardRight(now, prev, 2, 2), "Forward Right Validation not validating valid move for Player 2"); 
        }
        */


        public static void Main()
        {
            //Placeholder
        }
    }
}
