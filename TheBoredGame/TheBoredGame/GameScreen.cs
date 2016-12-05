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
    class GameScreen
    {
        
        protected KeyboardState currentKeyboardState;
        protected KeyboardState previousKeyboardState;
        protected GamePadState currentGamePadState;
        protected GamePadState previousGamePadState;

        public virtual void LoadContent(ContentManager contentManager)
        {

        }

        public virtual GameScreenManager.ScreenTransition Update(GameTime gameTime, GameScreenManager gameScreenManager)
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            previousGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            return GameScreenManager.ScreenTransition.NoChange;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color backgroundColor)
        {

        }
    }
}
