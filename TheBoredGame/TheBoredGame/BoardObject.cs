using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoredGame
{
    abstract class BoardObject
    {
        // Attributes and Accessors
        protected Boolean isCollidable;
        public Boolean IsCollidable
        {
            get { return isCollidable; }
            set { isCollidable = value; }
        }

        protected Player belongsTo;
        public Player BelongsTo
        {
            get { return belongsTo; }
            set { belongsTo = value; }
        }

        protected int boardRow, boardColumn;
        public int BoardColumnIndex
        {
            get { return boardColumn; }
            set { boardColumn = value; }
        }
        public int BoardRowIndex
        {
            get { return boardRow; }
            set { boardRow = value; }
        }

        // Constructor
        public BoardObject(int boardColumn, int boardRow, Player belongsTo)
        {
            this.boardColumn = boardColumn;
            this.boardRow = boardRow;
            this.belongsTo = belongsTo;
        }
    }
}
