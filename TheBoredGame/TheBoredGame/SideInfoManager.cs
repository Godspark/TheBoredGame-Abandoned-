using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace TheBoredGame
{
    class SideInfoManager
    {
        List<String> infoTexts;
        Vector2 frameStartPosition;
        int xFrameOffset, yFrameOffset;
        SpriteFont spriteFont;
        public SpriteFont SpriteFont
        {
            get { return spriteFont; }
            set { spriteFont = value; }
        }

        public SideInfoManager(int xPosition, int yPosition)
        {
            infoTexts = new List<String>();
            frameStartPosition = new Vector2(xPosition, yPosition);
            xFrameOffset = 15;
            yFrameOffset = 15;
        }

        public void LoadContent(ContentManager contentManager)
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color backgroundColor, Color textColor)
        {
            int yLineOffset = 0;
            foreach (String infoText in infoTexts)
            {
                spriteBatch.DrawString(spriteFont, infoText, new Vector2(frameStartPosition.X + xFrameOffset, frameStartPosition.Y + yFrameOffset + yLineOffset), textColor);
                //spriteBatch.DrawString(spriteFont, infoText, new Vector2(0,0), backgroundColor);
                yLineOffset += 20;
            }
        }

        public void AddInfoText(String infoText)
        {
            infoTexts.Add(infoText);
        }
    }
}
