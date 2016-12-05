using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheBoredGame
{
    class Highlight
    {
        //Enumerator
        public enum Area
        {
            Highlight, Up, Down, Left, Right
        };

        public enum HighlightTexture
        {
            RedHighlight, YellowHighlight, UpMoveArrow, DownMoveArrow, LeftMoveArrow, RightMoveArrow, FightDirection, NothingDirection
        };
        
        //Attributes and accessors
        Boolean drawHighlight;
        public Boolean DrawHighlight
        {
            get { return drawHighlight; }
            set { drawHighlight = value; }
        }

        Rectangle drawnHighlightRectangle, upRectangle, downRectangle, leftRectangle, rightRectangle;
        
        int boardColumn, boardRow;
        public int BoardRow
        {
            get { return boardRow; }
        }
        public int BoardColumn
        {
            get { return boardColumn; }
        }
        
        Dictionary<HighlightTexture, Texture2D> highlightTextures;
        Dictionary<Area, Texture2D> activeTextures;

        public Highlight()
        {
            highlightTextures = new Dictionary<HighlightTexture, Texture2D>();
            activeTextures = new Dictionary<Area, Texture2D>();
            drawHighlight = false;
        }

        public void LoadContent(ContentManager contentManager)
        {
            highlightTextures[HighlightTexture.RedHighlight] = contentManager.Load<Texture2D>("highlightUnitRed");
            highlightTextures[HighlightTexture.YellowHighlight] = contentManager.Load<Texture2D>("highlightUnitYellow");
            highlightTextures[HighlightTexture.UpMoveArrow] = contentManager.Load<Texture2D>("upMoveArrow");
            highlightTextures[HighlightTexture.DownMoveArrow] = contentManager.Load<Texture2D>("downMoveArrow");
            highlightTextures[HighlightTexture.LeftMoveArrow] = contentManager.Load<Texture2D>("leftMoveArrow");
            highlightTextures[HighlightTexture.RightMoveArrow] = contentManager.Load<Texture2D>("rightMoveArrow");
            highlightTextures[HighlightTexture.FightDirection] = contentManager.Load<Texture2D>("fightIcon");
            highlightTextures[HighlightTexture.NothingDirection] = contentManager.Load<Texture2D>("nothingIcon");

            activeTextures[Area.Highlight] = highlightTextures[HighlightTexture.YellowHighlight];
            activeTextures[Area.Up] = highlightTextures[HighlightTexture.NothingDirection];
            activeTextures[Area.Down] = highlightTextures[HighlightTexture.NothingDirection];
            activeTextures[Area.Left] = highlightTextures[HighlightTexture.NothingDirection];
            activeTextures[Area.Right] = highlightTextures[HighlightTexture.NothingDirection];
        }

        public void Update(GameTime gameTime)
        {
            this.drawnHighlightRectangle = new Rectangle(boardColumn * 150 + 21, boardRow * 150 + 21, 110, 110);
            Rectangle emptyRectangle = new Rectangle(0,0,0,0);
            
            if (activeTextures[Area.Up] != highlightTextures[HighlightTexture.NothingDirection])
                this.upRectangle = new Rectangle(boardColumn * 150 + 68, boardRow * 150 - 15, 15, 30);
            else
                this.upRectangle = emptyRectangle;

            if (activeTextures[Area.Down] != highlightTextures[HighlightTexture.NothingDirection])
                this.downRectangle = new Rectangle(boardColumn * 150 + 68, boardRow * 150 + 135, 15, 30);
            else
                this.downRectangle = emptyRectangle;

            if (activeTextures[Area.Left] != highlightTextures[HighlightTexture.NothingDirection])
                this.leftRectangle = new Rectangle(boardColumn * 150 - 15, boardRow * 150 + 68, 30, 15);
            else
                this.leftRectangle = emptyRectangle;

            if (activeTextures[Area.Right] != highlightTextures[HighlightTexture.NothingDirection])
                this.rightRectangle = new Rectangle(boardColumn * 150 + 135, boardRow * 150 + 68, 30, 15);
            else
                this.rightRectangle = emptyRectangle;
        }
        
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color backgroundColor)
        {
            if (drawHighlight)
            {
                spriteBatch.Draw(activeTextures[Area.Highlight], drawnHighlightRectangle, backgroundColor);
            }
        }
        public void DrawMoveArrows(GameTime gameTime, SpriteBatch spriteBatch, Color backgroundColor)
        {
            spriteBatch.Draw(activeTextures[Area.Up], upRectangle, backgroundColor);
            spriteBatch.Draw(activeTextures[Area.Down], downRectangle, backgroundColor);
            spriteBatch.Draw(activeTextures[Area.Left], leftRectangle, backgroundColor);
            spriteBatch.Draw(activeTextures[Area.Right], rightRectangle, backgroundColor);
        }

        public void ChangePosition(int boardColumn, int boardRow)
        {
            this.boardColumn = boardColumn;
            this.boardRow = boardRow;
        }
        public void ChangeActiveHighlightTexture(Area area, HighlightTexture newActiveTexture)
        {
            activeTextures[area] = highlightTextures[newActiveTexture];
        }

    }
}
