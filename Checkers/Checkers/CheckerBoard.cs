﻿using Checkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class CheckerBoard
    {
        public static Boolean godMode = false;
        private int[,] board;

        public CheckerBoard()
        {
            board = new int[8, 8];

            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (i % 2 == 0 && j % 2 == 1)
                        board[i, j] = 1;
                    else if (i % 2 == 1 && j % 2 == 0)
                        board[i, j] = 1;
                    else
                        board[i, j] = 0;
                }
            }

            for (int i = 5; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (i % 2 == 0 && j % 2 == 1)
                        board[i, j] = 2;
                    else if (i % 2 == 1 && j % 2 == 0)
                        board[i, j] = 2;
                    else
                        board[i, j] = 0;
                }
            }
        }

        public CheckerBoard(bool flag)
        {
            if (flag)
            {
                board = new int[8, 8];

                for (int i = 0; i <= 7; i++)
                {
                    for (int j = 0; j <= 7; j++)
                        board[i, j] = 0;
                }
            }
        }

        public void placePiece(int[] pos, int piece)
        {
            board[pos[0], pos[1]] = piece;
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
        public int[,] getBoard()
        {
            return this.board;
        }

        public bool applyMove(List<int> coords, int player)
        {
            int[] oldPosition = new int [2];
            oldPosition[0] = coords.ElementAt(0);
            oldPosition[1] = coords.ElementAt(1);
            int[] newPosition = new int [2];
            newPosition[0] = coords.ElementAt(2);
            newPosition[1] = coords.ElementAt(3);

            if (validateMove(oldPosition, newPosition, player))
            {
                Console.WriteLine("Is Valid");
                int moveType = getMoveType(newPosition, oldPosition, player);
                Console.WriteLine($"Move Type" + moveType);
                if (moveType == 1 || moveType == 2)
                {
                    board[newPosition[0], newPosition[1]] = board[oldPosition[0], oldPosition[1]];
                    board[oldPosition[0], oldPosition[1]] = 0;
                    crownIfPossible(newPosition, player);
                    return true;
                }
                else if (moveType == 3)
                {
                    //Keep Jumping Untill All Jumps of this piece
                    board[newPosition[0], newPosition[1]] = board[oldPosition[0], oldPosition[1]];
                    int[] toBeJumped = genLeftForwardPos(oldPosition, player);
                    board[toBeJumped[0], toBeJumped[1]] = 0;
                    board[oldPosition[0], oldPosition[1]] = 0;
                    crownIfPossible(newPosition, player);
                    return true;
                }
                else if (moveType == 4)
                {
                    //Keep Jumping Untill All Jumps of this piece
                    board[newPosition[0], newPosition[1]] = board[oldPosition[0], oldPosition[1]];
                    int[] toBeJumped = genRightForwardPos(oldPosition, player);
                    board[toBeJumped[0], toBeJumped[1]] = 0;
                    board[oldPosition[0], oldPosition[1]] = 0;
                    crownIfPossible(newPosition, player);
                    return true;
                }
                else if (moveType == 5 || moveType == 6)
                {

                    board[newPosition[0], newPosition[1]] = board[oldPosition[0], oldPosition[1]];
                    board[oldPosition[0], oldPosition[1]] = 0;
                    return true;
                }
                else if (moveType == 7)
                {
                    //Keep Jumping Untill All Jumps of this piece
                    board[newPosition[0], newPosition[1]] = board[oldPosition[0], oldPosition[1]];
                    int[] toBeJumped = genLeftBackwardPos(oldPosition, player);
                    board[toBeJumped[0], toBeJumped[1]] = 0;
                    board[oldPosition[0], oldPosition[1]] = 0;
                    return true;
                }
                else if (moveType == 8)
                {
                    board[newPosition[0], newPosition[1]] = board[oldPosition[0], oldPosition[1]];
                    int[] toBeJumped = genRightBackwardPos(oldPosition, player);
                    board[toBeJumped[0], toBeJumped[1]] = 0;
                    board[oldPosition[0], oldPosition[1]] = 0;
                    return true;
                }
            }
            return false;

        }

        public bool validateMove(int[] prev, int[] now, int player)
        {
            if (!godMode && (!(board[prev[0], prev[1]] == player || board[prev[0], prev[1]] == player + 2)))
                return false;
            //Determine Piece Type
            bool pieceKing = isKing(board[prev[0], prev[1]]);
            int moveType = getMoveType(now, prev, player);

            if (moveType == 1 || moveType == 2)
            {
                if (checkAnyJumpPossible(player))
                    return false;
                else
                {
                    if (moveType == 1)
                        return validateForwardLeft(now, prev, player, board[prev[0], prev[1]]);
                    else if (moveType == 2)
                        return validateForwardRight(now, prev, player, board[prev[0], prev[1]]);
                }
            }
            else if (moveType == 3)
            {
                return validateForwardJumpLeft(now, prev, player, board[prev[0], prev[1]]);
            }
            else if (moveType == 4)
            {
                return validateForwardJumpRight(now, prev, player, board[prev[0], prev[1]]);
            }
            else if (moveType == 5 && (godMode || pieceKing))
            {
                if (checkAnyJumpPossible(player))
                    return false;
                return validateBackwardLeft(now, prev, player, board[prev[0], prev[1]]);
            }
            else if (moveType == 6 && (godMode || pieceKing))
            {
                if (checkAnyJumpPossible(player))
                    return false;
                return validateBackwardRight(now, prev, player, board[prev[0], prev[1]]);
            }
            else if (moveType == 7 && (godMode || pieceKing))
                return validateBackwardJumpLeft(now, prev, player, board[prev[0], prev[1]]);
            else if (moveType == 8 && (godMode || pieceKing))
                return validateBackwardJumpRight(now, prev, player, board[prev[0], prev[1]]);

            else
                return false;
            return false;
        }

        private bool isKing(int piece)
        {
            if (piece == 3 || piece == 4)
                return true;
            else
                return false;
        }

        // 1 - Forward Left
        // 2 - Forward Right
        // 3 - Forward Jump Left
        // 4 - Forward Jump Right
        // 5 - Backward Left
        // 6 - Backward Right
        // 7 - Backward Jump Left
        // 8 - Backward Jump Right
        private int getMoveType(int[] now, int[] prev, int player)
        {
            int[] fl = genLeftForwardPos(prev, player);
            int[] fr = genRightForwardPos(prev, player);
            int[] fjl = genLeftForwardJumpPos(prev, player);
            int[] fjr = genRightForwardJumpPos(prev, player);
            int[] bl = genLeftBackwardPos(prev, player);
            int[] br = genRightBackwardPos(prev, player);
            int[] bjl = genLeftBackwardJumpPos(prev, player);
            int[] bjr = genRightBackwardJumpPos(prev, player);

            if (areEqualPos(now, fl))
                return 1;
            else if (areEqualPos(now, fr))
                return 2;
            else if (areEqualPos(now, fjl))
                return 3;
            else if (areEqualPos(now, fjr))
                return 4;
            else if (areEqualPos(now, bl))
                return 5;
            else if (areEqualPos(now, br))
                return 6;
            else if (areEqualPos(now, bjl))
                return 7;
            else if (areEqualPos(now, bjr))
                return 8;
            else
                return -1;

        }

        private bool crownIfPossible(int [] pos, int player)
        {
            if(player == 1 && pos[0] == 7)
            {
                board[pos[0], pos[1]] = 3;
                return true;
            }
            else if (player == 2 && pos[0] == 0)
            {
                board[pos[0], pos[1]] = 4;
                return true;
            }
            return false;
        }

        private bool areEqualPos(int[] a, int[] b)
        {
            if (a[0] == b[0] && a[1] == b[1])
                return true;
            else
                return false;
        }

        public bool validateForwardLeft(int[] now, int[] prev, int player, int piece)
        {
            bool leftForward = false;
            if (player == 1)
            {
                leftForward = (now[0] - prev[0] == 1) && (now[1] - prev[1] == 1) && (board[now[0], now[1]] == 0) && (board[prev[0], prev[1]] == piece);
            }
            else if (player == 2)
            {
                leftForward = (now[0] - prev[0] == -1) && (now[1] - prev[1] == -1) && (board[now[0], now[1]] == 0) && (board[prev[0], prev[1]] == piece);
            }
            return leftForward;
        }

        public bool validateForwardRight(int[] now, int[] prev, int player, int piece)
        {
            bool rightForward = false;

            if (player == 1)
            {
                rightForward = (now[0] - prev[0] == 1) && (now[1] - prev[1] == -1) && (board[now[0], now[1]] == 0) && (board[prev[0], prev[1]] == piece);
            }
            else if (player == 2)
            {
                rightForward = (now[0] - prev[0] == -1) && (now[1] - prev[1] == 1) && (board[now[0], now[1]] == 0) && (board[prev[0], prev[1]] == piece);
            }
            return rightForward;
        }

        public bool validateForwardJumpLeft(int[] now, int[] prev, int player, int piece)
        {
            bool jumpLeft = false;
            if (player == 1)
            {
                jumpLeft = (now[0] - prev[0] == 2) && (now[1] - prev[1] == 2) && ((board[now[0] - 1, now[1] - 1] == 2) || (board[now[0] - 1, now[1] - 1] == 4)) && (board[prev[0], prev[1]] == piece) && (board[now[0],now[1]] == 0);
            }
            else if (player == 2)
            {
                jumpLeft = (now[0] - prev[0] == -2) && (now[1] - prev[1] == -2) && ((board[now[0] + 1, now[1] + 1] == 1) || (board[now[0] + 1, now[1] + 1] == 3)) && (board[prev[0], prev[1]] == piece) && (board[now[0], now[1]] == 0);
            }
            return jumpLeft;
        }

        public bool validateBackwardJumpLeft(int[] now, int[] prev, int player, int piece)
        {
            bool jumpLeft = false;
            if (player == 1)
            {
                jumpLeft = (now[0] - prev[0] == -2) && (now[1] - prev[1] == 2) && ((board[now[0] + 1, now[1] - 1] == 2) || (board[now[0] + 1, now[1] - 1] == 4)) && (board[prev[0], prev[1]] == 3) && (board[now[0], now[1]] == 0);
            }
            else if (player == 2)
            {
                jumpLeft = (now[0] - prev[0] == 2) && (now[1] - prev[1] == -2) && ((board[now[0] - 1, now[1] + 1] == 1) || (board[now[0] - 1, now[1] + 1] == 3)) && (board[prev[0], prev[1]] == 4) && (board[now[0], now[1]] == 0);
            }
            return jumpLeft;
        }

        public bool validateForwardJumpRight(int[] now, int[] prev, int player, int piece)
        {
            bool jumpRight = false;

            if (player == 1)
            {
                jumpRight = (now[0] - prev[0] == 2) && (now[1] - prev[1] == -2) && ((board[now[0] - 1, now[1] + 1] == 2) || (board[now[0] - 1, now[1] + 1] == 4)) && (board[prev[0], prev[1]] == piece) && (board[now[0], now[1]] == 0);
            }
            else if (player == 2)
            {
                jumpRight = (now[0] - prev[0] == -2) && (now[1] - prev[1] == 2) && ((board[now[0] + 1, now[1] - 1] == 1 )|| (board[now[0] + 1, now[1] - 1] == 3)) && (board[prev[0], prev[1]] == piece) && (board[now[0], now[1]] == 0);
            }
            return jumpRight;
        }

        public bool validateBackwardJumpRight(int[] now, int[] prev, int player, int piece)
        {
            bool jumpRight = false;

            if (player == 1)
            {
                jumpRight = (now[0] - prev[0] == -2) && (now[1] - prev[1] == -2) && ((board[now[0] + 1, now[1] + 1] == 2) || (board[now[0] + 1, now[1] + 1] == 4)) && (board[prev[0], prev[1]] == 3) && (board[now[0], now[1]] == 0);
            }
            else if (player == 2)
            {
                jumpRight = (now[0] - prev[0] == 2) && (now[1] - prev[1] == 2) && (board[now[0] - 1, now[1] - 1] == 1 || board[now[0] - 1, now[1] - 1] == 3) && (board[prev[0], prev[1]] == 4) && (board[now[0], now[1]] == 0);
            }
            return jumpRight;
        }

        public bool validateBackwardLeft(int[] now, int[] prev, int player, int piece)
        {
            bool backwardLeft = false;
            if (player == 1)
            {
                backwardLeft = (now[0] - prev[0] == -1) && (now[1] - prev[1] == 1) && (board[prev[0], prev[1]] == piece) && (board[now[0], now[1]] == 0);
            }
            else if (player == 2)
            {
                backwardLeft = (now[0] - prev[0] == 1) && (now[1] - prev[1] == -1) && (board[prev[0], prev[1]] == piece) && (board[now[0], now[1]] == 0);
            }
            return backwardLeft;
        }

        public bool validateBackwardRight(int[] now, int[] prev, int player, int piece)
        {
            bool backwardRight = false;
            if (player == 1 && piece == 3)
            {
                backwardRight = (now[0] - prev[0] == -1) && (now[1] - prev[1] == -1) && (board[prev[0], prev[1]] == piece) && (board[now[0], now[1]] == 0);
            }
            else if (player == 2 && piece == 4)
            {
                backwardRight = (now[0] - prev[0] == 1) && (now[1] - prev[1] == 1) && (board[prev[0], prev[1]] == piece) && (board[now[0], now[1]] == 0);
            }
            return backwardRight;
        }

        private bool noPiecesLeft(int player)
        {
            return allAvailablePieces(player).Count() == 0;

        }

        public List<int[]> allAvailablePieces(int player)
        {
            List<int[]> pieces = new List<int[]>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] == player || board[i, j] == (player + 2))
                    {
                        int[] pos = new int[2];
                        pos[0] = i;
                        pos[1] = j;
                        pieces.Add(pos);
                    }
                }
            }
            return pieces;
        }

        public List<int[]> allAvailableKings(int player)
        {
            List<int[]> kings = new List<int[]>();
            List<int[]> allPieces = allAvailablePieces(player);
            foreach (var a in allPieces)
            {
                if (board[a[0], a[1]] == player + 2)
                    kings.Add(a);
            }

            return kings;
        }

        private bool checkValidPosition(int[] pos)
        {
            if (pos[0] < 0 || pos[0] > 7 || pos[1] < 0 || pos[1] > 7)
                return false;
            else
                return true;
        }
        private int[] genLeftForwardJumpPos(int[] pos, int player)
        {
            int[] leftJumpPos = new int[2];
            if (player == 1)
            {
                leftJumpPos[0] = pos[0] + 2;
                leftJumpPos[1] = pos[1] + 2;
            }
            else if (player == 2)
            {
                leftJumpPos[0] = pos[0] - 2;
                leftJumpPos[1] = pos[1] - 2;
            }
            return leftJumpPos;
        }
        private int[] genLeftBackwardJumpPos(int[] pos, int player)
        {
            int[] leftJumpPos = new int[2];
            if (player == 1)
            {
                leftJumpPos[0] = pos[0] - 2;
                leftJumpPos[1] = pos[1] + 2;
            }
            else if (player == 2)
            {
                leftJumpPos[0] = pos[0] + 2;
                leftJumpPos[1] = pos[1] - 2;
            }
            return leftJumpPos;
        }
        private int[] genRightForwardJumpPos(int[] pos, int player)
        {
            int[] rightJumpPos = new int[2];
            if (player == 1)
            {
                rightJumpPos[0] = pos[0] + 2;
                rightJumpPos[1] = pos[1] - 2;
            }
            else if (player == 2)
            {
                rightJumpPos[0] = pos[0] - 2;
                rightJumpPos[1] = pos[1] + 2;
            }
            return rightJumpPos;
        }

        private int[] genRightBackwardJumpPos(int[] pos, int player)
        {
            int[] rightJumpPos = new int[2];
            if (player == 1)
            {
                rightJumpPos[0] = pos[0] - 2;
                rightJumpPos[1] = pos[1] - 2;
            }
            else if (player == 2)
            {
                rightJumpPos[0] = pos[0] + 2;
                rightJumpPos[1] = pos[1] + 2;
            }
            return rightJumpPos;
        }

        private int[] genLeftForwardPos(int[] pos, int player)
        {
            int[] leftForwardPos = new int[2];

            if (player == 1)
            {
                leftForwardPos[0] = pos[0] + 1;
                leftForwardPos[1] = pos[1] + 1;
            }
            else if (player == 2)
            {
                leftForwardPos[0] = pos[0] - 1;
                leftForwardPos[1] = pos[1] - 1;
            }
            return leftForwardPos;
        }

        private int[] genRightForwardPos(int[] pos, int player)
        {
            int[] rightForwardPos = new int[2];

            if (player == 1)
            {
                rightForwardPos[0] = pos[0] + 1;
                rightForwardPos[1] = pos[1] - 1;
            }
            else if (player == 2)
            {
                rightForwardPos[0] = pos[0] - 1;
                rightForwardPos[1] = pos[1] + 1;
            }
            return rightForwardPos;
        }

        public int[] genLeftBackwardPos(int[] pos, int player)
        {
            int[] leftBackwardPos = new int[2];
            if (player == 1)
            {
                leftBackwardPos[0] = pos[0] - 1;
                leftBackwardPos[1] = pos[1] + 1;
            }
            else if (player == 2)
            {
                leftBackwardPos[0] = pos[0] + 1;
                leftBackwardPos[1] = pos[1] - 1;
            }
            return leftBackwardPos;
        }

        private int[] genRightBackwardPos(int[] pos, int player)
        {
            int[] rightBackwardPos = new int[2];
            if (player == 1)
            {
                rightBackwardPos[0] = pos[0] - 1;
                rightBackwardPos[1] = pos[1] - 1;
            }
            else if (player == 2)
            {
                rightBackwardPos[0] = pos[0] + 1;
                rightBackwardPos[1] = pos[1] + 1;
            }
            return rightBackwardPos;
        }

        public bool checkForwardJumpPossible(int[] pos, int player)
        {
            bool leftJump = false;
            bool rightJump = false;
            int[] leftJumpPos = genLeftForwardJumpPos(pos, player);
            if (checkValidPosition(leftJumpPos))
                leftJump = validateForwardJumpLeft(leftJumpPos, pos, player, board[pos[0], pos[1]]);
            else
                leftJump = false;

            int[] rightJumpPos = genRightForwardJumpPos(pos, player);
            if (checkValidPosition(rightJumpPos))
                rightJump = validateForwardJumpRight(rightJumpPos, pos, player, board[pos[0], pos[1]]);
            else
                rightJump = false;

            return leftJump || rightJump;
        }

        public bool checkBackwardJumpPossible(int[] pos, int player)
        {
            if (board[pos[0], pos[1]] != 3 && board[pos[0], pos[1]] != 4)
                return false;
            bool leftJump = false;
            bool rightJump = false;
            int[] leftJumpPos = genLeftBackwardJumpPos(pos, player);
            if (checkValidPosition(leftJumpPos))
                leftJump = validateBackwardJumpLeft(leftJumpPos, pos, player, board[pos[0], pos[1]]);
            else
                leftJump = false;

            int[] rightJumpPos = genRightBackwardJumpPos(pos, player);
            if (checkValidPosition(rightJumpPos))
                rightJump = validateBackwardJumpRight(rightJumpPos, pos, player, board[pos[0], pos[1]]);
            else
                rightJump = false;

            return leftJump || rightJump;
        }

        public bool checkForwardPossible(int[] pos, int player)
        {
            bool leftForward = false;
            bool rightForward = false;


            int[] leftForwardPos = genLeftForwardPos(pos, player);
            if (checkValidPosition(leftForwardPos))
                leftForward = validateForwardLeft(leftForwardPos, pos, player, board[pos[0], pos[1]]);
            else
                leftForward = false;

            int[] rightForwardPos = genRightForwardPos(pos, player);
            if (checkValidPosition(rightForwardPos))
                rightForward = validateForwardRight(rightForwardPos, pos, player, board[pos[0], pos[1]]);
            else
                rightForward = false;

            return leftForward || rightForward;
        }

        public bool checkBackwardPossible(int[] pos, int player)
        {
            if (board[pos[0], pos[1]] != 3 && board[pos[0], pos[1]] != 4)
                return false;
            bool leftBackward = false;
            bool rightBackward = false;

            int[] leftBackwardPos = genLeftBackwardPos(pos, player);

            if (checkValidPosition(leftBackwardPos))
                leftBackward = validateBackwardLeft(leftBackwardPos, pos, player, board[pos[0], pos[1]]);
            else
                leftBackward = false;

            int[] rightBackwardPos = genRightBackwardPos(pos, player);

            if (checkValidPosition(rightBackwardPos))
                rightBackward = validateBackwardRight(rightBackwardPos, pos, player, board[pos[0], pos[1]]);
            else
                rightBackward = false;
            return leftBackward || rightBackward;
        }

        public bool checkAnyJumpPossiblePiece(int[] pos, int player)
        {
            return checkForwardJumpPossible(pos, player) || checkBackwardJumpPossible(pos, player);
        }

        public bool checkAnyJumpPossible(int player)
        {
            List<int[]> availablePieces = allAvailablePieces(player);
            bool validJump = false;
            foreach (var a in availablePieces)
            {
                if (checkForwardJumpPossible(a, player) || checkBackwardJumpPossible(a, player))
                {
                    validJump = true;
                    break;
                }
            }
            return validJump;
        }

        public bool checkAnyForwardPossible(int player)
        {
            List<int[]> availablePieces = allAvailablePieces(player);

            bool validForward = false;
            foreach (var a in availablePieces)
            {
                if (checkForwardPossible(a, player))
                {
                    validForward = true;
                    break;
                }
            }
            return validForward;
        }

        public bool checkAnyBackwardPossible(int player)
        {
            List<int[]> allKings = allAvailableKings(player);
            bool validBackward = false;
            if (allKings.Count() > 0)
            {
                foreach (var a in allKings)
                {
                    if (checkBackwardPossible(a, player))
                    {
                        validBackward = true;
                        break;
                    }

                }
            }
            return validBackward;
        }
        public bool checkAnyPossibleMoves(int player)
        {

            return checkAnyForwardPossible(player) || checkAnyJumpPossible(player) || checkAnyBackwardPossible(player);
        }
        public bool checkWin(int player)
        {
            if (player == 1)
            {
                return noPiecesLeft(2) || !checkAnyPossibleMoves(2);
            }
            else if (player == 2)
            {
                return noPiecesLeft(1) || !checkAnyPossibleMoves(1);
            }
            return false;
        }

        public int getResult()
        {
            if (checkWin(1))
                return 1;
            else if (checkWin(2))
                return 2;
            else
                return -1;
        }
    }
}


