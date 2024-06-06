using System;
using System.Collections.Generic;
using System.Linq;

namespace DominoGame
{
    public class DominoTile
    {
        public int Left { get; private set; }
        public int Right { get; private set; }

        public DominoTile(int left, int right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"[{Left}|{Right}]";
        }

        public bool IsDouble()
        {
            return Left == Right;
        }

        public bool Matches(int number)
        {
            return Left == number || Right == number;
        }

        public void Flip()
        {
            int temp = Left;
            Left = Right;
            Right = temp;
        }
    }

    public class DominoSet
    {
        private List<DominoTile> tiles;
        private Random random;

        public DominoSet()
        {
            tiles = new List<DominoTile>();
            random = new Random();
            for (int i = 1; i <= 6; i++)
            {
                for (int j = i; j <= 6; j++)
                {
                    tiles.Add(new DominoTile(i, j));
                }
            }
        }

        public List<DominoTile> DrawTiles(int count)
        {
            var drawnTiles = tiles.OrderBy(t => random.Next()).Take(count).ToList();
            tiles = tiles.Except(drawnTiles).ToList();
            return drawnTiles;
        }

        public DominoTile DrawTile()
        {
            if (tiles.Count == 0)
                return null;

            var tile = tiles[random.Next(tiles.Count)];
            tiles.Remove(tile);
            return tile;
        }
    }

    public class Game
    {
        private List<DominoTile> playerHand;
        private List<DominoTile> computerHand;
        private List<DominoTile> board;
        private DominoSet dominoSet;

        public Game()
        {
            dominoSet = new DominoSet();
            playerHand = dominoSet.DrawTiles(7);
            computerHand = dominoSet.DrawTiles(7);
            board = new List<DominoTile>();
        }

        public void Play()
        {
            bool playerTurn = true;

            while (playerHand.Count > 0 && computerHand.Count > 0)
            {
                if (playerTurn)
                {
                    PlayerMove();
                }
                else
                {
                    ComputerMove();
                }

                playerTurn = !playerTurn;
            }

            if (playerHand.Count == 0)
            {
                Console.WriteLine("You win!");
            }
            else if (computerHand.Count == 0)
            {
                Console.WriteLine("Computer wins!");
            }
            else
            {
                Console.WriteLine("It's a draw!");
            }
        }

        private void PlayerMove()
        {
            bool validMove = false;

            while (!validMove)
            {
                Console.WriteLine("Your hand: " + string.Join(" ", playerHand));
                Console.WriteLine("Board: " + string.Join(" ", board));

                Console.WriteLine("Enter the index of the tile you want to play: ");
                int index = int.Parse(Console.ReadLine());

                if (index < 0 || index >= playerHand.Count)
                {
                    Console.WriteLine("Invalid index. Try again.");
                    continue;
                }

                var selectedTile = playerHand[index];

                if (IsValidMove(selectedTile))
                {
                    PlayTile(playerHand, selectedTile);
                    validMove = true;
                }
                else
                {
                    Console.WriteLine("Invalid move. Try again.");
                }
            }
        }

        private void ComputerMove()
        {
            var playableTiles = GetPlayableTiles(computerHand);
            if (playableTiles.Count == 0)
            {
                Console.WriteLine("Computer has no playable tiles. Drawing a tile...");
                var drawnTile = dominoSet.DrawTile();
                if (drawnTile != null)
                {
                    computerHand.Add(drawnTile);
                }
                else
                {
                    Console.WriteLine("No more tiles to draw. Computer passes turn...");
                }

                return;
            }

            var selectedTile = playableTiles.First();
            PlayTile(computerHand, selectedTile);

            Console.WriteLine("Computer plays " + selectedTile);
        }

        private void PlayTile(List<DominoTile> hand, DominoTile tile)
        {
            if (board.Count == 0)
            {
                board.Add(tile);
            }
            else
            {
                if (tile.Matches(board.First().Left))
                {
                    if (tile.Right != board.First().Left)
                    {
                        tile.Flip();
                    }
                    board.Insert(0, tile);
                }
                else if (tile.Matches(board.Last().Right))
                {
                    if (tile.Left != board.Last().Right)
                    {
                        tile.Flip();
                    }
                    board.Add(tile);
                }
            }

            hand.Remove(tile);
        }

        private List<DominoTile> GetPlayableTiles(List<DominoTile> hand)
        {
            if (board.Count == 0)
            {
                return hand;
            }

            int left = board.First().Left;
            int right = board.Last().Right;

            return hand.Where(tile => tile.Matches(left) || tile.Matches(right)).ToList();
        }

        private bool IsValidMove(DominoTile tile)
        {
            if (board.Count == 0)
            {
                return true;
            }

            int left = board.First().Left;
            int right = board.Last().Right;

            return tile.Matches(left) || tile.Matches(right);
        }

        static void Main(string[] args)
        {
            Game game = new Game();
            game.Play();
        }
    }
}
