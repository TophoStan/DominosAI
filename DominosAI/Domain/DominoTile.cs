using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Domain
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
}
