using Domino.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino
{
    class MiniMaxHelper
    {
        public class GameState
        {
            public List<DominoTile> Board { get; set; }
            public List<DominoTile> PlayerHand { get; set; }
            public List<DominoTile> ComputerHand { get; set; }
            public bool IsPlayerTurn { get; set; }

            public GameState(List<DominoTile> board, List<DominoTile> playerHand, List<DominoTile> computerHand, bool isPlayerTurn)
            {
                Board = new List<DominoTile>(board);
                PlayerHand = new List<DominoTile>(playerHand);
                ComputerHand = new List<DominoTile>(computerHand);
                IsPlayerTurn = isPlayerTurn;
            }

            public (int, int) GetBoardEnds()
            {
                if (Board.Count == 0) return (-1, -1);
                int leftEnd = Board.First().Left;
                int rightEnd = Board.Last().Right;
                return (leftEnd, rightEnd);
            }
        }

        public static List<DominoTile> GetPossibleMoves(GameState state)
        {
            var possibleMoves = new List<DominoTile>();
            var (leftEnd, rightEnd) = state.GetBoardEnds();

            var hand = state.IsPlayerTurn ? state.PlayerHand : state.ComputerHand;

            foreach (var tile in hand)
            {
                if (tile.Matches(leftEnd) || tile.Matches(rightEnd))
                {
                    possibleMoves.Add(tile);
                }
            }

            return possibleMoves;
        }

        public static int Evaluate(GameState state)
        {
            // Simple evaluation: difference in number of dominoes
            return state.ComputerHand.Count - state.PlayerHand.Count;
        }

        public static GameState ApplyMove(GameState state, DominoTile move, bool isPlayerMove)
        {
            var newBoard = new List<DominoTile>(state.Board);
            var newPlayerHand = new List<DominoTile>(state.PlayerHand);
            var newComputerHand = new List<DominoTile>(state.ComputerHand);

            if (newBoard.Count == 0)
            {
                newBoard.Add(move);
            }
            else
            {
                if (move.Matches(newBoard.First().Left))
                {
                    if (move.Right != newBoard.First().Left)
                    {
                        move.Flip();
                    }
                    newBoard.Insert(0, move);
                }
                else if (move.Matches(newBoard.Last().Right))
                {
                    if (move.Left != newBoard.Last().Right)
                    {
                        move.Flip();
                    }
                    newBoard.Add(move);
                }
            }

            if (isPlayerMove)
            {
                newPlayerHand.Remove(move);
            }
            else
            {
                newComputerHand.Remove(move);
            }

            return new GameState(newBoard, newPlayerHand, newComputerHand, !isPlayerMove);
        }

        public static bool IsTerminal(GameState state)
        {
            return state.PlayerHand.Count == 0 || state.ComputerHand.Count == 0 || GetPossibleMoves(state).Count == 0;
        }

        public static int Minimax(GameState state, int depth, bool maximizingPlayer)
        {
            if (depth == 0 || IsTerminal(state))
            {
                return Evaluate(state);
            }

            if (maximizingPlayer)
            {
                int maxEval = int.MinValue;
                foreach (var move in GetPossibleMoves(state))
                {
                    var newState = ApplyMove(state, move, true);
                    int eval = Minimax(newState, depth - 1, false);
                    maxEval = Math.Max(maxEval, eval);
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                foreach (var move in GetPossibleMoves(state))
                {
                    var newState = ApplyMove(state, move, false);
                    int eval = Minimax(newState, depth - 1, true);
                    minEval = Math.Min(minEval, eval);
                }
                return minEval;
            }
        }

        public static DominoTile FindBestMove(GameState state, int depth)
        {
            DominoTile bestMove = null;
            int bestValue = int.MinValue;

            foreach (var move in GetPossibleMoves(state))
            {
                var newState = ApplyMove(state, move, true);
                int moveValue = Minimax(newState, depth - 1, false);

                if (moveValue > bestValue)
                {
                    bestValue = moveValue;
                    bestMove = move;
                }
            }

            return bestMove;
        }

    }
}
