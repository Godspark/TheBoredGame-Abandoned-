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
    class MenuScreen : GameScreen
    {
        Texture2D menuTexture;

        public override void LoadContent(ContentManager contentManager)
        {
            menuTexture = contentManager.Load<Texture2D>("menuScreen");
        }

        public override GameScreenManager.ScreenTransition Update(GameTime gameTime, GameScreenManager gameScreenManager)
        {
            base.Update(gameTime, gameScreenManager); //Do NOT use the return value from here!

            if (previousKeyboardState != currentKeyboardState)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Up) || currentGamePadState.IsButtonDown(Buttons.LeftThumbstickUp))
                {

                }
                else if (currentKeyboardState.IsKeyDown(Keys.Down) || currentGamePadState.IsButtonDown(Buttons.LeftThumbstickDown))
                {

                }
                if (currentKeyboardState.IsKeyDown(Keys.Enter))
                {
                    return GameScreenManager.ScreenTransition.MenuToExit;
                }
                if (currentKeyboardState.IsKeyDown(Keys.Space))
                {
                    return GameScreenManager.ScreenTransition.MenuToCombat;
                }
            }
            return GameScreenManager.ScreenTransition.NoChange;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color backgroundColor)
        {
            spriteBatch.Draw(menuTexture, new Rectangle(0, 0, 1000, 1000), backgroundColor);
        }

    }
}
