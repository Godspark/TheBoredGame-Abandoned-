using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TheBoredGame
{
    class EmptySpace : BoardObject
    {
        // Constructor
        public EmptySpace(int boardColumn, int boardRow, Player belongsTo)
            : base(boardColumn, boardRow, belongsTo)
        {
            IsCollidable = false;
        }
    }
}
