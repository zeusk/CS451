﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkers;
using NUnit.Framework;
namespace CheckersUnitTests
{
    [TestFixture]
    class CheckerBoardTests
    {
        private GameState gs;
        private CheckerBoard board;

        [Test]
        public void setTests()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void CheckRegularSetupInConstructor()
        {
            board = new CheckerBoard();
            Assert.IsNotNull(board, "CheckerBoard default constructor does not initialize correctly");
            var madeBoard = board.getBoard();
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
            board  = new CheckerBoard(true);
            Assert.IsNotNull(board);
            //Assert.IsNotEmpty(board);
        }

        [Test]
        public void PlacePieceSuccessfully()
        {
            board = new CheckerBoard(true);
            int[] pPos = new int [2];
            pPos[0] = 2;
            pPos[1] = 3;
            board.placePiece(pPos, 1);
            Assert.IsTrue(board.getBoard()[pPos[0], pPos[1]] == 1, "Piece not placed correctly on board"); 
        }

        [Test]
        public void RetrieveBoard()
        {
            board = new CheckerBoard();
            Assert.IsNotNull(board.getBoard(), "Board array not retrieved successfully");
        }

        [Test]
        public void ApplyBasicMove()
        {
            board = new CheckerBoard();
            var pos = new List<int>();
            pos.Add(2);
            pos.Add(1);
            pos.Add(3);
            pos.Add(2);
            Assert.IsTrue(board.applyMove(pos, 1), "Player 1 move not executed successfully");
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
        public void ApplyForwardRight()
        {
            board = new CheckerBoard();
            Assert.IsTrue(board.applyMove(genCoords(2, 1, 3, 0), 1),
                "Forward Right Move Not Being Applied Correctly For Player 1");

            Assert.IsTrue(board.applyMove(genCoords(5, 0, 4, 1), 2),
                "Forward Right Move Not Being Applied Correctly For Player 2");
        }

        [Test]
        public void ApplyForwardLeftJump()
        {
            board = new CheckerBoard(true);
            
            board.placePiece(genPos(0,0), 1);
            board.placePiece(genPos(1,1), 2);
            Assert.IsTrue(board.applyMove(genCoords(0, 0, 2, 2), 1), "Jump Left not Executing Correctly for Player 1");

            board = new CheckerBoard(true);
            board.placePiece(genPos(1, 1), 1);
            board.placePiece(genPos(2, 2), 2);
            Assert.IsTrue(board.applyMove(genCoords(2, 2, 0, 0), 2), "Jump Left not Executing Correctly for Player 2");
        }

        [Test]
        public void ApplyForwardRightJump()
        {
            board = new CheckerBoard(true);

            board.placePiece(genPos(0, 2), 1);
            board.placePiece(genPos(1, 1), 2);
            Assert.IsTrue(board.applyMove(genCoords(0, 2, 2, 0), 1), "Jump Right not Executing Correctly for Player 1");

            board = new CheckerBoard(true);
            board.placePiece(genPos(1, 1), 1);
            board.placePiece(genPos(2, 0), 2);
            Assert.IsTrue(board.applyMove(genCoords(2, 0, 0, 2), 2), "Jump Left not Executing Correctly for Player 2");
        }

        [Test]
        public void ApplyBackwardLeftJump()
        {
            board = new CheckerBoard(true);

            board.placePiece(genPos(0, 2), 1);
            board.placePiece(genPos(1, 1), 2);
            Assert.IsTrue(board.applyMove(genCoords(0, 2, 2, 0), 1), "Jump Right not Executing Correctly for Player 1");

            board = new CheckerBoard(true);
            board.placePiece(genPos(1, 1), 1);
            board.placePiece(genPos(2, 0), 2);
            Assert.IsTrue(board.applyMove(genCoords(2, 0, 0, 2), 2), "Jump Left not Executing Correctly for Player 2");
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