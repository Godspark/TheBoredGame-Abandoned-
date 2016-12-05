using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheBoredGame
{
    class Tile
    {
        // Attributes and Accessors
        private int drawnXPos = 0;
        private int drawnYPos = 0;

        protected int boardRow, boardColumn;
        public int BoardColumn
        {
            get { return boardColumn; }
            set { boardColumn = value; }
        }
        public int BoardRow
        {
            get { return boardRow; }
            set { boardRow = value; }
        }

        Texture2D tileTexture;

        // Constructor
        public Tile(Texture2D tileTexture, int boardColumn, int boardRow)
        {
            this.tileTexture = tileTexture;
            this.boardColumn = boardColumn;
            this.boardRow = boardRow;
            this.drawnXPos = boardColumn * 150;
            this.drawnYPos = boardRow * 150;
        }

        // Functions
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color backgroundColor)
        {
            spriteBatch.Draw(tileTexture, new Rectangle(drawnXPos, drawnYPos, 150, 150), backgroundColor);
        }
        
    }
}
