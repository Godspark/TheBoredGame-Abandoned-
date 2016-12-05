using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheBoredGame
{
    class Unit : BoardObject
    {
        // Attributes and Accessors
        private int drawnXPos = 0;
        private int drawnYPos = 0;

        Texture2D unitTexture, dotTexture;
        BoardManager.ObjectType objectType;
        String name;
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        int exhaustionCounter, maxExhaustion;
        Boolean drawDot;

        // Constructor
        public Unit(Texture2D unitTexture, Texture2D dotTexture, BoardManager.ObjectType objectType, int maxExhaustion, int boardColumn, int boardRow, Player belongsTo)
            : base(boardColumn, boardRow, belongsTo)
        {
            IsCollidable = true;
            this.unitTexture = unitTexture;
            this.objectType = objectType;
            name = objectType.ToString();
            name = name.Replace('_', ' ');
            base.BoardColumnIndex = boardColumn;
            base.BoardRowIndex = boardRow;
            this.drawnXPos = boardColumn * 150 + 25;
            this.drawnYPos = boardRow * 150 + 25;
            this.exhaustionCounter = 0;
            this.maxExhaustion = maxExhaustion;
            this.dotTexture = dotTexture;
            drawDot = false;
        }

        public void Update(GameTime gameTime)
        {
            this.drawnXPos = boardColumn * 150 + 25;
            this.drawnYPos = boardRow * 150 + 25;
            drawDot = (exhaustionCounter == maxExhaustion);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color backgroundColor)
        {
            spriteBatch.Draw(unitTexture, new Rectangle(drawnXPos, drawnYPos, 100, 100), backgroundColor);
            if (drawDot)
                spriteBatch.Draw(dotTexture, new Rectangle(drawnXPos + 90, drawnYPos + 90, 10, 10), backgroundColor);
        }

        public Boolean IsExhausted()
        {
            return (exhaustionCounter == maxExhaustion);
        }

        public Boolean IncrementExhaustionCounter()
        {
            exhaustionCounter++;
            if (exhaustionCounter >= maxExhaustion)
            {
                exhaustionCounter = maxExhaustion;
                return false;
            }
            else
                return true;
                
        }

        public void ResetExhaustionCounter()
        {
            exhaustionCounter = 0;
        }
    }
}
