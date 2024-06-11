using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Domain
{
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
                    if (i != 0 && j != 0)
                    {
                        tiles.Add(new DominoTile(i, j));
                    }
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

        public bool HasTiles()
        {
            return tiles.Count > 0;
        }
    }
}
