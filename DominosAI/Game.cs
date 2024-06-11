using Domino.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino
{
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

                if (!playerHand.Any(tile => IsValidMove(tile)) && !dominoSet.HasTiles() &&
                    !computerHand.Any(tile => IsValidMove(tile)) && !dominoSet.HasTiles())
                {
                    Console.WriteLine("It's a draw!");
                    return;
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
            Console.WriteLine("PC hand: " + string.Join(" ", computerHand));
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

            //Minimax Algorithm
            //kijk naar de playerhand en de computerhand
            //Welke tile kan de computer spelen die de player de minste punten geeft en de computer de meeste punten geeft
            //Als de computer geen tile kan spelen die de player de minste punten geeft, speel dan de tile die de computer de meeste punten geeft



           
            var selectedTile = playableTiles.First();
            PlayTile(computerHand, selectedTile);

            Console.WriteLine("Computer plays " + selectedTile 
                + " \n " +
                "\n----------------------------------------------------------- \n" );

            
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
    }
}
