using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TheBoredGame
{
    class WinScreen : GameScreen
    {
        Texture2D winTexture;

        public override void LoadContent(ContentManager contentManager)
        {
            winTexture = contentManager.Load<Texture2D>("winScreen");
        }

        public override GameScreenManager.ScreenTransition Update(GameTime gameTime, GameScreenManager gameScreenManager)
        {
            base.Update(gameTime, gameScreenManager); //Do NOT use the return value from here!

            if (previousKeyboardState != currentKeyboardState)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Enter))
                {
                    return GameScreenManager.ScreenTransition.WinToMenu;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Space))
                {
                    return GameScreenManager.ScreenTransition.WinToMenu;
                }
            }
            return GameScreenManager.ScreenTransition.NoChange;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color backgroundColor)
        {
            spriteBatch.Draw(winTexture, new Rectangle(0, 0, 1000, 1000), backgroundColor);
        }
    }
}
